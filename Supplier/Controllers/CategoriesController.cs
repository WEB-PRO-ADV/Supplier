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
            EditCategoryViewModel model = new EditCategoryViewModel();
            model.Category = _context.Categories.Where(c => c.Id == id).FirstOrDefault();
            model.OldCategorySpecs = _context.CategorySpecs.Where(cs => cs.CategoryId == id).ToList();

            if (model.Category == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection form)
        {
            int id = Convert.ToInt32(form["Category.Id"]);

            var category = _context.Categories.Where(c => c.Id == id).FirstOrDefault();
            if (category == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.Id = id;
                    category.Name = form["Category.Name"];
                    category.Description = form["Category.Description"];

                    var oldSpecs = _context.CategorySpecs.Where(cs => cs.CategoryId == id).ToList();
                    var oldSpecsCtr = oldSpecs.Count;

                    for (int i = 0; i < oldSpecsCtr; i++)
                    {
                        var tmpSpecId = form["OldCategorySpecs[" + i + "].Id"];
                        if (tmpSpecId.Count > 0)
                        {
                            int specId = Convert.ToInt32(form["OldCategorySpecs[" + i + "].Id"]);
                            if(specId == oldSpecs[i].Id)
                            {
                                oldSpecs[i].Name = form["OldCategorySpecs[" + i + "].Name"];
                                _context.Update(oldSpecs[i]);
                            }
                            else
                            {
                                var productSpecs = _context.ProductSpecs.Where(ps => ps.CategorySpecId == oldSpecs[i].Id).ToList();
                                foreach(var productSpec in productSpecs)
                                {
                                    _context.Remove(productSpec);
                                }
                                _context.Remove(oldSpecs[i]);
                            }
                        }
                        else
                        {
                            var productSpecs = _context.ProductSpecs.Where(ps => ps.CategorySpecId == oldSpecs[i].Id).ToList();
                            foreach (var productSpec in productSpecs)
                            {
                                _context.Remove(productSpec);
                            }
                            _context.Remove(oldSpecs[i]);
                        }

                    }

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
            return View();
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

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, IFormCollection form)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();
            if (category.Id == 1)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var catSpecs = _context.CategorySpecs.Where(cs => cs.CategoryId == category.Id).ToList();

                foreach(var catSpec in catSpecs)
                {
                    var prodsSpecs = _context.ProductSpecs.Where(ps => ps.CategorySpecId == catSpec.Id).ToList();
                    foreach (var prodsSpec in prodsSpecs)
                    {
                        _context.Remove(prodsSpec);
                    }
                    _context.Remove(catSpec);
                }

                var products = _context.Products.Where(p => p.CategoryId == category.Id).ToList();

                foreach(var product in products)
                {
                    product.CategoryId = 1;
                    _context.Update(product);
                }

                _context.Remove(category);
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
