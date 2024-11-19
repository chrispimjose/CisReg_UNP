﻿using CisReg_Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using static CisReg_Website.Models.HallModel;
using CisReg_Website.Domain;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace CisReg_Website.Controllers
{
    public class HallController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HallController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> UpdateAgreement(ObjectId id, int newAgreement, [Bind("Id,agreement")]  HallModel hallModel)
        {
            if (id != hallModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Buscar a vaga existente
                    var existinghalls = await _context.Halls.FindAsync(id);
                    if (existinghalls == null)
                    {
                        return NotFound();
                    }

                    // Atualizar o atributo 'AvailableHour' com o novo valor de 'idAvailableHour'
                    existinghalls.Agreement = newAgreement;

                    // Salvar as mudanças
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallsModelExists(hallModel.Id))
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
            return View(Index);
        }
        public async Task<IActionResult> Index()
        {
            var listaHalls = await _context.Halls.ToListAsync();
            // Verifica se encontrou algum município
            if (listaHalls.Count == 0)
            {
                TempData["AlertMessage"] = "Nenhum município encontrado.";
            }
            else
            {
                TempData["AlertMessage"] = $"Carregados {listaHalls.Count} municípios com sucesso!";
            }

            return View(listaHalls);
        }

        private bool HallsModelExists(ObjectId id)
        {
            return _context.Halls.Any(h => h.Id == id);
        }


    }
}
