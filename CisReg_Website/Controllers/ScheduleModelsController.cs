using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CisReg_Website.Domain;
using CisReg_Website.Models;
using MongoDB.Bson;

namespace CisReg_Website.Controllers
{
    public class ScheduleModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScheduleModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScheduleModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Schedules.ToListAsync());
        }

        // GET: ScheduleModels/Details/5
        public async Task<IActionResult> Details(ObjectId id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduleModel = await _context.Schedules
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scheduleModel == null)
            {
                return NotFound();
            }

            return View(scheduleModel);
        }

        // GET: ScheduleModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScheduleModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,professionalId,Time,StatusPacient")] ScheduleModel scheduleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scheduleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scheduleModel);
        }

        // GET: ScheduleModels/Edit/5
        public async Task<IActionResult> Edit(ObjectId id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduleModel = await _context.Schedules.FindAsync(id);
            if (scheduleModel == null)
            {
                return NotFound();
            }
            return View(scheduleModel);
        }

        // POST: ScheduleModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ObjectId id, [Bind("Id,professionalId,Time,StatusPacient")] ScheduleModel scheduleModel)
        {
            if (id != scheduleModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scheduleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleModelExists(scheduleModel.Id))
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
            return View(scheduleModel);
        }

        // GET: ScheduleModels/Delete/5
        public async Task<IActionResult> Delete(ObjectId id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduleModel = await _context.Schedules
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scheduleModel == null)
            {
                return NotFound();
            }

            return View(scheduleModel);
        }

        // POST: ScheduleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ObjectId id)
        {
            var scheduleModel = await _context.Schedules.FindAsync(id);
            if (scheduleModel != null)
            {
                _context.Schedules.Remove(scheduleModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult AddToCalendar(string doctorName)
        {
            return RedirectToAction("SchuduleProfessional", "ScheduleModels", new { doctorName = doctorName });
        }
        public IActionResult SchuduleProfessional(string doctorName)
        {
            // Passa o nome do médico para a view
            ViewBag.DoctorName = doctorName;
            return View("SchuduleProfessional"); // Isso renderizará Schedule.cshtml
        }

        private bool ScheduleModelExists(ObjectId id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }
    }
}
