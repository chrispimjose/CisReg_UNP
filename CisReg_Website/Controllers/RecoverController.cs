using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using CisReg_Website.Domain;
using Microsoft.EntityFrameworkCore;
using CisReg_Website.Models;
using System.Net.Mail;
using System.Net;
using CisReg_Website.Repositories;

namespace CisReg_Website.Controllers
{
    [Route("recover-password")]
    public class RecoverController (ApplicationDbContext context, ProfessionalRepository professionalRepository) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ProfessionalRepository _professionalRepository = professionalRepository;

        Random random = new Random();

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.CodeSended = false;
            ViewBag.ValidCode = false;

            return View();
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit(RecoverModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Erro no envio...";
                return View();
            }

            try
            {
                int recoverCode = random.Next(100000, 1000000);

                HttpContext.Session.SetString("Email", model.Email);

                var professionalEmail = await _context.Professionals
                    .FirstOrDefaultAsync(m => m.Email == model.Email);

                var professionalFirstName = professionalEmail.FirstName;
                var professionalLastName = professionalEmail.LastName;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("cisregproject@gmail.com", "behb vygq qexi hznb"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("cisregproject@gmail.com", "Suporte CISREG"),
                    Subject = "Recuperação de senha",
                    Body = $"Olá {professionalFirstName} {professionalLastName}, segue abaixo o código para recuperação de sua conta.\n\n Código: {recoverCode}",
                    IsBodyHtml = false,
                };

                if (professionalEmail == null)
                {
                    throw new ArgumentException("Email não cadastrado.");
                }

                mailMessage.To.Add(model.Email);

                await smtpClient.SendMailAsync(mailMessage);

                ViewBag.ValidCode = false;
                ViewBag.CodeSended = true;

                HttpContext.Session.SetInt32("RecoveryCode", recoverCode);

                return View("Index");
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            catch (SmtpException smtpEx)
            {
                ViewBag.ErrorMessage = $"Erro no envio do código: {smtpEx}";
            }

            return View("Index");
        }

        [HttpPost("validate-code")]
        public IActionResult ValidateCode(RecoverModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Erro no envio...";
                return View("Index");
            }

            try
            {
                var storedCode = HttpContext.Session.GetInt32("RecoveryCode");

                if (storedCode == null || model.Code != storedCode)
                {
                    ViewBag.ErrorMessage = "Código Inválido.";
                    return View("Index", model);
                }

                ViewBag.CodeSended = true;
                ViewBag.ValidCode = true;

                return View("Index");

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(RecoverModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Erro no envio...";
                return View("Index");
            }
            try
            {
                var storedEmail = HttpContext.Session.GetString("Email");

                var updateResult = await _professionalRepository.UpdatePasswordAsync(storedEmail, model.Password);

                if (!updateResult)
                {
                    ViewBag.ErrorMessage = "Erro ao atualizar a senha. Verifique se o e-mail está correto.";
                    return View("Index");
                }

                return RedirectToAction("Index", "Login");

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao tentar alterar a senha: {ex.Message}";
            }

            return View("Index");
        }

    }
}