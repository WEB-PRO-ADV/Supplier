using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Supplier.Models;

namespace Supplier
{
    public class FactoriesController : Controller
    {
        private readonly SupplierDbContext _context;

        public FactoriesController(SupplierDbContext context)
        {
            _context = context;
        }

        // GET: Factories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Factories.ToListAsync());
        }

        // GET: Factories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factory = await _context.Factories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (factory == null)
            {
                return NotFound();
            }

            return View(factory);
        }

        // GET: Factories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Factories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Factory factory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(factory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(factory);
        }

        // GET: Factories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factory = await _context.Factories.FindAsync(id);
            if (factory == null)
            {
                return NotFound();
            }
            return View(factory);
        }

        // POST: Factories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Factory factory)
        {
            if (id != factory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(factory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FactoryExists(factory.Id))
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
            return View(factory);
        }

        // GET: Factories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factory = await _context.Factories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (factory == null)
            {
                return NotFound();
            }

            return View(factory);
        }

        // POST: Factories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var factory = await _context.Factories.FindAsync(id);
            _context.Factories.Remove(factory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FactoryExists(int id)
        {
            return _context.Factories.Any(e => e.Id == id);
        }
    }
}
