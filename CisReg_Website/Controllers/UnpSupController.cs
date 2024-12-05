using CisReg_Website.Domain;
using CisReg_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CisReg_Website.Controllers
{
    public class UnpSupController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UnpSupController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Submit([Bind("FirstName", "LastName", "Email", "Password", "BirthDate", "CPF", "Phone", "Address", "Position", "Department", "WorkShift", "Permission")]SupUnp supUnp)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Erro no envio...";
                return View();
            }
            try
            {
                var suppUnp = await _context.SupUnp.FirstOrDefaultAsync(s => s.Email == supUnp.Email);

                if (suppUnp != null)
                {
                    _context.Add(supUnp);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                
            }
            return View(supUnp);
        }

    }
}
