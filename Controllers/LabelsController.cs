﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BDLab2;

namespace BDLab2.Controllers
{
    public class LabelsController : Controller
    {
        private readonly MusicDbContext _context;

        public LabelsController(MusicDbContext context)
        {
            _context = context;
        }

        // GET: Labels
        public async Task<IActionResult> Index()
        {
              return _context.Labels != null ? 
                          View(await _context.Labels.ToListAsync()) :
                          Problem("Entity set 'MusicDbContext.Labels'  is null.");
        }

        // GET: Labels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Labels == null)
            {
                return NotFound();
            }

            var label = await _context.Labels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (label == null)
            {
                return NotFound();
            }

            return View(label);
        }

        // GET: Labels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Labels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Label label)
        {
            if (ModelState.IsValid)
            {
                if (_context.Labels.Any(l => l.Name == label.Name))
                {
                    ModelState.AddModelError("Name", "A label with the same name already exists.");
                    return View(label);
                }


                _context.Add(label);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(label);
        }

        // GET: Labels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Labels == null)
            {
                return NotFound();
            }

            var label = await _context.Labels.FindAsync(id);
            if (label == null)
            {
                return NotFound();
            }
            return View(label);
        }

        // POST: Labels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Label label)
        {
            if (id != label.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_context.Labels.Any(l => l.Id != label.Id && l.Name == label.Name))
                {
                    ModelState.AddModelError("Name", "A label with the same name already exists.");
                    return View(label);
                }

                try
                {
                    _context.Update(label);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabelExists(label.Id))
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
            return View(label);
        }

        // GET: Labels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Labels == null)
            {
                return NotFound();
            }

            var label = await _context.Labels
                .Include(l => l.Artists) 
                .FirstOrDefaultAsync(m => m.Id == id);
            if (label == null)
            {
                return NotFound();
            }

            return View(label);
        }

        // POST: Labels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Labels == null)
            {
                return Problem("Entity set 'MusicDbContext.Labels'  is null.");
            }
            var label = await _context.Labels
                .Include(l => l.Artists) // Include the related songs
                .FirstOrDefaultAsync(l => l.Id == id);

            if (label != null)
            {
                _context.Artists.RemoveRange(label.Artists); // Remove the related songs
                _context.Labels.Remove(label);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool LabelExists(int id)
        {
          return (_context.Labels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
