using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using WebApplication.Models.DBSet;

namespace WebApplication.Controllers
{
    public class EmploeesController : Controller
    {
        private readonly AppDbcontext _context;

        public EmploeesController(AppDbcontext context)
        {
            _context = context;
        }

        // GET: Emploees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Emploees.ToListAsync());
        }

        // GET: Emploees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emploee = await _context.Emploees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emploee == null)
            {
                return NotFound();
            }

            return View(emploee);
        }

        // GET: Emploees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Emploees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Family,City,GenderEmploey")] Emploee emploee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emploee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(emploee);
        }

        // GET: Emploees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emploee = await _context.Emploees.FindAsync(id);
            if (emploee == null)
            {
                return NotFound();
            }
            return View(emploee);
        }

        // POST: Emploees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Family,City,GenderEmploey")] Emploee emploee)
        {
            if (id != emploee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emploee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmploeeExists(emploee.Id))
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
            return View(emploee);
        }

        // GET: Emploees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emploee = await _context.Emploees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emploee == null)
            {
                return NotFound();
            }

            return View(emploee);
        }

        // POST: Emploees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emploee = await _context.Emploees.FindAsync(id);
            _context.Emploees.Remove(emploee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmploeeExists(int id)
        {
            return _context.Emploees.Any(e => e.Id == id);
        }
    }
}
