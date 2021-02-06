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

                
                int cnt = Convert.ToInt32(form["UniqueSpecsCtr"]);

                for (int i = 0; i <= cnt; i++)
                {
                    var uniqueSpecName = form["ProductUniqueSpec[" + i + "].Name"];
                    var uniqueSpecValue = form["ProductUniqueSpec[" + i + "].Value"];
                    if(uniqueSpecName.Count > 0 && uniqueSpecValue.Count > 0)
                    {
                        ProductUniqueSpec tmp = new ProductUniqueSpec();
                        tmp.Name = uniqueSpecName;
                        tmp.Value = uniqueSpecValue;
                        tmp.ProductId = id;
                        _context.Add(tmp);
                    }
                }
                await _context.SaveChangesAsync();

                cnt = Convert.ToInt32(form["CategorySpecsCtr"]);
                int ndx = 0;
                for (int i = 0; i <= cnt; i++)
                {
                    var categorySpecValue = form["ProductSpec[" + i + "].Value"];
                    if (categorySpecValue.Count > 0)
                    {
                        var categorySpecId = form["CategorySpec.Id"][ndx++];
                        ProductSpec tmp = new ProductSpec();
                        tmp.CategorySpecId = Convert.ToInt32(categorySpecId);
                        tmp.Value = categorySpecValue;
                        tmp.ProductId = id;
                        _context.Add(tmp);
                    }
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
            var NewProductViewModel = new NewProductViewModel();
            NewProductViewModel.Product = product;
            NewProductViewModel.ProductSpec = _context.ProductSpecs.Where(ps => ps.ProductId == product.Id).ToList();
            NewProductViewModel.ProductUniqueSpec = _context.ProductUniqueSpecs.Where(pus => pus.ProductId == product.Id).ToList();
            NewProductViewModel.Category = _context.Categories.Where(c => c.Id == product.CategoryId).FirstOrDefault();

            var categorySpecs = _context.CategorySpecs.Where(cs => cs.CategoryId == NewProductViewModel.Category.Id).ToList();
            ViewData["CategorySpecs"] = categorySpecs;


            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["FactoryId"] = new SelectList(_context.Factories, "Id", "Name", product.FactoryId);
            return View(NewProductViewModel);
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
