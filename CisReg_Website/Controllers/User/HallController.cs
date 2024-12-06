using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CisReg_Website.Domain;
using CisReg_Website.Models;
using MongoDB.Bson;
using CisReg_Website.Repositories;

namespace CisReg_Website.Controllers.User
{
    public class HallController : Controller
    {
        private readonly ApplicationDbContext _context;
      

        public HallController(ApplicationDbContext context)
        {
            _context = context;
           
        }

        public IActionResult Index()
        {
            var halls = _context.Halls.ToList(); // Exemplo com Entity Framework
    return View("Listar", halls);
}
        

        public async Task<IActionResult> Details(ObjectId id)
        {
            var hall = await _context.Halls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CNPJ,CNES,Address,CityHallName,AgreementNumber,Id,CityHallManager,ResponsiblePhoneNumber,ResponsibleEmail")] HallModel hall)
        {
            if (ModelState.IsValid)
            {
                
                 // Ajuste conforme o modelo.
                _context.Add(hall);
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hall);
        }

        public async Task<IActionResult> Edit(ObjectId id)
        {
            var hall = await _context.Halls.FindAsync(id);
            if (hall == null)
            {
                return NotFound();
            }
            return View(hall);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ObjectId id, [Bind("CNPJ,CNES,Address,CityHallName,AgreementNumber,Id,CityHallManager,ResponsiblePhoneNumber,ResponsibleEmail")] HallModel hall)
        {
            if (id != hall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallExists(hall.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hall);
        }

        public async Task<IActionResult> Delete(ObjectId id)
        {
            var hall = await _context.Halls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ObjectId id)
        {
            var hall = await _context.Halls.FindAsync(id);
            if (hall != null)
            {
                _context.Halls.Remove(hall);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HallExists(ObjectId id)
        {
            return _context.Halls.Any(e => e.Id == id);
        }
    }
}
