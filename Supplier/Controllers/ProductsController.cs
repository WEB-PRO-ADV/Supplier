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
    public class ProductsController : Controller
    {
        private readonly SupplierDbContext _context;

        public ProductsController(SupplierDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var supplierDbContext = _context.Products.Include(p => p.Category).Include(p => p.Factory);
            return View(await supplierDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Factory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["FactoryId"] = new SelectList(_context.Factories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product();
                product.Name = form["Product.Name"];
                product.Code = form["Product.Code"];
                product.Description = form["Product.Description"];
                product.Price = Convert.ToDouble(form["Product.Price"]);
                product.ImgUrl = form["Product.ImgUrl"];
                product.ImgName = form["Product.ImgName"];
                product.FactoryId = Convert.ToInt32(form["Product.FactoryId"]);
                product.CategoryId = Convert.ToInt32(form["Product.CategoryId"]);
                _context.Add(product);
                await _context.SaveChangesAsync();

                int id = product.Id;

                var uniqueSpecsNames = form["ProductUniqueSpec.Name"];
                var uniqueSpecsValues = form["ProductUniqueSpec.Value"];
                var cnt = uniqueSpecsNames.Count();

                for (int  i = 0; i < cnt; i ++)
                {
                    ProductUniqueSpec tmp = new ProductUniqueSpec();
                    tmp.Name = uniqueSpecsNames[i];
                    tmp.Value = uniqueSpecsValues[i];
                    tmp.ProductId = id;
                    _context.Add(tmp);
                }
                await _context.SaveChangesAsync();
                
                var categorySpecsIds = form["CategorySpec.Id"];
                var categorySpecsValues = form["ProductSpec.Value"];
                cnt = categorySpecsIds.Count();

                for (int i = 0; i < cnt; i++)
                {
                    ProductSpec tmp = new ProductSpec();
                    tmp.CategorySpecId = Convert.ToInt32(categorySpecsIds[i]);
                    tmp.Value = categorySpecsValues[i];
                    tmp.ProductId = id;
                    _context.Add(tmp);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            ViewData["FactoryId"] = new SelectList(_context.Factories, "Id", "Id", product.FactoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Code,Description,Price,ImgUrl,ImgName,FactoryId,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            ViewData["FactoryId"] = new SelectList(_context.Factories, "Id", "Id", product.FactoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Factory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public IActionResult GetCategorySpecs(int id)
        {
            CategorySpecsViewModel ViewModel = new CategorySpecsViewModel();

            var categorySpecs = _context.CategorySpecs.Where(s => s.Category.Id == id).ToList();
            ViewData["CategorySpecs"] = categorySpecs;

            return PartialView(ViewModel);
        }

        public IActionResult AddProductUniqueSpec(int id)
        {
            ViewData["SpecId"] = id;
            return PartialView();
        }

    }
}
