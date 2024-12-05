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
    public class SupUnpsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupUnpsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Phone,Address,Position,Department,WorkShift,Id,Email,CPF,Password,FirstName,LastName,Permission")] SupUnp supUnp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supUnp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supUnp);
        }

    }
}
