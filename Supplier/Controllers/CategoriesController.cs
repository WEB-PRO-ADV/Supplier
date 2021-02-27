using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Supplier.Models;
using Supplier.ViewModels;


namespace Supplier.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly SupplierDbContext _context;

        public CategoriesController(SupplierDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            List<Category> categories = _context.Categories.ToList();
            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);

            var categorySpecs = _context.CategorySpecs.Where(cs => cs.CategoryId == category.Id).ToList();

            var Model = new CategoryDetailsViewModel();
            Model.Category = category;
            Model.CategorySpecs = categorySpecs;

            if (category == null)
            {
                return NotFound();
            }

            return View(Model);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category();
                category.Name = form["Category.Name"];
                category.Description = form["Category.Description"];
                _context.Add(category);
                await _context.SaveChangesAsync();

                int id = category.Id;
                int cnt = Convert.ToInt32(form["CatSpecsCtr"]);

                for (int i = 0; i <= cnt; i++)
                {
                    var catSpecName = form["CategorySpecs[" + i + "].Name"];
                    if (catSpecName.Count > 0)
                    {
                        CategorySpec tmp = new CategorySpec();
                        tmp.Name = catSpecName;
                        tmp.CategoryId = id;
                        _context.Add(tmp);
                    }
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            return View();
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, IFormCollection form)
        {
            var Category = await _context.Categories.FindAsync(id);
            if (Category.Id == 1)
            {
                return RedirectToAction(nameof(Index));

            }
            else
            {


                var ProductSpec = _context.ProductSpecs.ToList();
                var CategorySpec = _context.CategorySpecs.ToList();
                var Product = _context.Products.ToList();
                foreach (var item in ProductSpec)
                {
                    if (item.CategorySpecId == id)
                    {
                        int IdprodSpec = item.Id;
                        var ProductSpecs = await _context.ProductSpecs.FindAsync(IdprodSpec);
                        _context.ProductSpecs.Remove(ProductSpecs);
                    }
                }
                foreach (var item in CategorySpec)
                {
                    if (item.CategoryId == id)
                    {
                        int IdCatSpec = item.Id;
                        var CategorySpecs = await _context.ProductSpecs.FindAsync(IdCatSpec);
                        _context.ProductSpecs.Remove(CategorySpecs);
                    }
                }
                foreach (var item in Product)
                {
                    if (item.CategoryId == id)
                    {
                        item.CategoryId = 1;
                    }
                }
                _context.Categories.Remove(Category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        public IActionResult AddCategoryNewSpec(int id)
        {
            ViewData["SpecId"] = id;
            return PartialView();
        }
    }
}
