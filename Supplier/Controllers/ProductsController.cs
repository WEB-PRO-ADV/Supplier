using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(SupplierDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

            var productSpecs = _context.ProductSpecs.Where(ps => ps.ProductId == product.Id).ToList();

            List<CategorySpec> categorySpecs = new List<CategorySpec>();
            foreach (var productSpec in productSpecs)
            {
                CategorySpec categorySpec = _context.CategorySpecs.Where(cs => cs.Id == productSpec.CategorySpecId).FirstOrDefault();
                categorySpecs.Add(categorySpec);
            }

            var productUniqueSpecs = _context.ProductUniqueSpecs.Where(pus => pus.ProductId == product.Id).ToList();

            var Model = new ProductDetailsViewModel();
            Model.Product = product;
            Model.ProductUniqueSpecs = productUniqueSpecs;
            Model.ProductSpecs = productSpecs;
            Model.CategorySpecs = categorySpecs;


            if (product == null)
            {
                return NotFound();
            }

            return View(Model);
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
        public async Task<IActionResult> Create(IFormFile image, IFormCollection form)
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
                product.ImgName = form["Product.Name"];
                string uniqueFileName = null;
                if(image != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                    product.ImgUrl = uniqueFileName;
                }
                

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
            var EditProductViewModel = new EditProductViewModel();
            EditProductViewModel.Product = product;
            EditProductViewModel.ProductSpec = _context.ProductSpecs.Where(ps => ps.ProductId == product.Id).ToList();
            EditProductViewModel.OldProductUniqueSpec = _context.ProductUniqueSpecs.Where(pus => pus.ProductId == product.Id).ToList();
            EditProductViewModel.Category = _context.Categories.Where(c => c.Id == product.CategoryId).FirstOrDefault();

            List<CategorySpec> categorySpecs = new List<CategorySpec>();
            foreach(var prodSpec in EditProductViewModel.ProductSpec)
            {
                categorySpecs.Add(_context.CategorySpecs.Where(cs => cs.CategoryId == EditProductViewModel.Category.Id).FirstOrDefault());
            }

            ViewData["CategorySpecs"] = categorySpecs;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["FactoryId"] = new SelectList(_context.Factories, "Id", "Name", product.FactoryId);
            return View(EditProductViewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection form)
        {
            int productId = Convert.ToInt32(form["Product.Id"]);

            var product = _context.Products.Where(p => p.Id == productId).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //Product product = new Product();
                int oldCategoryId = product.CategoryId;
                product.Id = productId;
                product.Name = form["Product.Name"];
                product.Code = form["Product.Code"];
                product.Description = form["Product.Description"];
                product.Price = Convert.ToDouble(form["Product.Price"]);
                product.ImgUrl = form["Product.ImgUrl"];
                product.ImgName = form["Product.ImgName"];
                product.FactoryId = Convert.ToInt32(form["Product.FactoryId"]);

                product.CategoryId = Convert.ToInt32(form["Product.CategoryId"]);

                int cnt = 0;
                if (product.CategoryId == oldCategoryId)
                {
                    var tmpCtr = form["CategorySpecs.Count"];
                    if (tmpCtr.Count == 0)
                    {
                        tmpCtr = form["CategorySpecsCtr"];
                    }
                    cnt = Convert.ToInt32(tmpCtr);
                    int ndx = 0;
                    for (int i = 0; i <= cnt; i++)
                    {
                        var categorySpecValue = form["ProductSpec[" + i + "].Value"];
                        if (categorySpecValue.Count > 0)
                        {
                            var tmpCategorySpecId = form["CategorySpec.Id"][ndx++];
                            var categorySpecId = 0;
                            if (tmpCategorySpecId != null && tmpCategorySpecId != "")
                            {
                                categorySpecId = Convert.ToInt32(tmpCategorySpecId);
                                var ProductSpec = _context.ProductSpecs.Where(ps => ps.ProductId == productId && ps.CategorySpecId == categorySpecId).FirstOrDefault();
                                ProductSpec.Value = form["ProductSpec[" + i + "].Value"];
                                _context.Update(ProductSpec);
                            }
                        }
                    }
                }
                else
                {
                    var ProductSpecs = _context.ProductSpecs.Where(ps => ps.ProductId == productId).ToList();
                    foreach (var productSpec in ProductSpecs)
                    {
                        _context.ProductSpecs.Remove(productSpec);
                    }
                    cnt = Convert.ToInt32(form["CategorySpecsCtr"]);
                    int ndx = 0;
                    for (int i = 0; i <= cnt; i++)
                    {
                        var categorySpecValue = form["ProductSpec[" + i + "].Value"];
                        if (categorySpecValue.Count > 0)
                        {
                            var categorySpecId = form["CategorySpec.Id"][ndx++];
                            ProductSpec ProductSpec = new ProductSpec();
                            ProductSpec.CategorySpecId = Convert.ToInt32(categorySpecId);
                            ProductSpec.Value = categorySpecValue;
                            ProductSpec.ProductId = productId;
                            _context.Update(ProductSpec);
                        }
                    }

                }


                var OldUniqueSpecs = _context.ProductUniqueSpecs.Where(us => us.ProductId == productId).ToList();
                cnt = OldUniqueSpecs.Count;
                foreach (var oldUniqueSpec in OldUniqueSpecs)
                {
                    bool flag = false;
                    for (int i = 0; i <= cnt; i++)
                    {
                        var tmp = form["OldProductUniqueSpec[" + i + "].Id"];
                        if (tmp.Count > 0)
                        {
                            int tmpId = Convert.ToInt32(tmp);
                            if (tmpId == oldUniqueSpec.Id)
                            {
                                oldUniqueSpec.Name = form["OldProductUniqueSpec[" + i + "].Name"];
                                oldUniqueSpec.Value = form["OldProductUniqueSpec[" + i + "].Value"];
                                _context.Update(oldUniqueSpec);
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (!flag)
                    {
                        _context.ProductUniqueSpecs.Remove(oldUniqueSpec);
                    }
                }



                cnt = Convert.ToInt32(form["UniqueSpecsCtr"]);
                for (int i = 0; i <= cnt; i++)
                {
                    var uniqueSpecName = form["ProductUniqueSpec[" + i + "].Name"];
                    var uniqueSpecValue = form["ProductUniqueSpec[" + i + "].Value"];
                    if (uniqueSpecName.Count > 0 && uniqueSpecValue.Count > 0)
                    {
                        ProductUniqueSpec tmp = new ProductUniqueSpec();
                        tmp.Name = uniqueSpecName;
                        tmp.Value = uniqueSpecValue;
                        tmp.ProductId = productId;
                        _context.Add(tmp);
                    }
                }

                _context.Update(product);
                await _context.SaveChangesAsync();
                
            }
            return RedirectToAction("Index");
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

            var productSpecs = _context.ProductSpecs.Where(ps => ps.ProductId == product.Id).ToList();

            List<CategorySpec> categorySpecs = new List<CategorySpec>();
            foreach (var productSpec in productSpecs)
            {
                CategorySpec categorySpec = _context.CategorySpecs.Where(cs => cs.Id == productSpec.CategorySpecId).FirstOrDefault();
                categorySpecs.Add(categorySpec);
            }

            var productUniqueSpecs = _context.ProductUniqueSpecs.Where(pus => pus.ProductId == product.Id).ToList();

            var Model = new ProductDetailsViewModel();
            Model.Product = product;
            Model.ProductUniqueSpecs = productUniqueSpecs;
            Model.ProductSpecs = productSpecs;
            Model.CategorySpecs = categorySpecs;

            if (product == null)
            {
                return NotFound();
            }

            return View(Model);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            var uniqueSpecs = _context.ProductUniqueSpecs.Where(pus => pus.ProductId == id).ToList();
            foreach(var spec in uniqueSpecs)
            {
                spec.Product = null;
                spec.ProductId = 0;
                _context.ProductUniqueSpecs.Remove(spec);
            }

            var specs = _context.ProductSpecs.Where(ps => ps.ProductId == id).ToList();
            foreach(var spec in specs)
            {
                spec.Product = null;
                spec.ProductId = 0;
                spec.CategorySpec = null;
                spec.CategorySpecId = 0;
                _context.ProductSpecs.Remove(spec);
            }
            product.Factory = null;
            product.FactoryId = 0;
            product.Category = null;
            product.CategoryId = 0;
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

        public IActionResult EditGetCategorySpecs(int productId)
        {
            var product = _context.Products.Where(p => p.Id == productId).FirstOrDefault();
            if(product == null)
            {
                return NotFound();
            }

            var category = _context.Categories.Where(c => c.Id == product.CategoryId).FirstOrDefault();



            CategorySpecsViewModel ViewModel = new CategorySpecsViewModel();

            List<CategorySpec> categorySpecs = new List<CategorySpec>();
            var ProductSpec = _context.ProductSpecs.Where(ps => ps.ProductId == product.Id).ToList();
            //foreach (var prodSpec in ProductSpec)
            //{
            //    categorySpecs.Add(_context.CategorySpecs.Where(cs => cs.Id == Id).FirstOrDefault());
            //}
            ViewData["CategorySpecs"] = categorySpecs;

            return PartialView(ViewModel);
        }
    }
}
