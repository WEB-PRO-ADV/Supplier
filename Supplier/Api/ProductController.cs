using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supplier.APIModels;
using Supplier.Models;

namespace Supplier.Api
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly SupplierDbContext _context;

        public ProductController(SupplierDbContext context)
        {
            _context = context;
        }

        [Produces("application/json")]
        [HttpGet("")]
        public IActionResult All()
        {
            try
            {
                var products = _context.Products.ToList();
                var apiProducts = new List<ProductApiModel>();
                foreach (var product in products)
                {
                    var apiProduct = new ProductApiModel();
                    apiProduct.Code = product.Code;
                    apiProduct.Name = product.Name;
                    apiProduct.Description = product.Description;
                    apiProduct.Price = product.Price;
                    apiProduct.ImgUrl = product.ImgUrl;
                    apiProduct.Factory = _context.Factories.Where(f => f.Id == product.FactoryId).FirstOrDefault().Name;
                    apiProduct.Category = _context.Categories.Where(c => c.Id == product.CategoryId).FirstOrDefault().Name;

                    var uniqueSpecs = _context.ProductUniqueSpecs.Where(pus => pus.ProductId == product.Id).ToList();
                    var specs = _context.ProductSpecs.Where(ps => ps.ProductId == product.Id).ToList();
                    var apiSpecs = new List<SpecificationsApiModel>();

                    foreach (var spec in uniqueSpecs)
                    {
                        var apiSpec = new SpecificationsApiModel();
                        apiSpec.Name = spec.Name;
                        apiSpec.Value = spec.Value;
                        apiSpecs.Add(apiSpec);
                    }

                    foreach (var spec in specs)
                    {
                        var apiSpec = new SpecificationsApiModel();
                        apiSpec.Name = _context.CategorySpecs.Where(cs => cs.Id == spec.CategorySpecId).FirstOrDefault().Name;
                        apiSpec.Value = spec.Value;
                        apiSpecs.Add(apiSpec);
                    }
                    apiProduct.Specifications = apiSpecs;
                    apiProducts.Add(apiProduct);
                }
                return Ok(apiProducts);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpGet("Code/{code}")]
        public IActionResult Code(string code)
        {
            try
            {
                var product = _context.Products.Where(p => p.Code == code).FirstOrDefault();
                var apiProduct = new ProductApiModel();
                if (product != null)
                {
                    apiProduct.Code = product.Code;
                    apiProduct.Name = product.Name;
                    apiProduct.Description = product.Description;
                    apiProduct.Price = product.Price;
                    apiProduct.ImgUrl = product.ImgUrl;
                    apiProduct.Factory = _context.Factories.Where(f => f.Id == product.FactoryId).FirstOrDefault().Name;
                    apiProduct.Category = _context.Categories.Where(c => c.Id == product.CategoryId).FirstOrDefault().Name;

                    var uniqueSpecs = _context.ProductUniqueSpecs.Where(pus => pus.ProductId == product.Id).ToList();
                    var specs = _context.ProductSpecs.Where(ps => ps.ProductId == product.Id).ToList();
                    var apiSpecs = new List<SpecificationsApiModel>();

                    foreach (var spec in uniqueSpecs)
                    {
                        var apiSpec = new SpecificationsApiModel();
                        apiSpec.Name = spec.Name;
                        apiSpec.Value = spec.Value;
                        apiSpecs.Add(apiSpec);
                    }

                    foreach (var spec in specs)
                    {
                        var apiSpec = new SpecificationsApiModel();
                        apiSpec.Name = _context.CategorySpecs.Where(cs => cs.Id == spec.CategorySpecId).FirstOrDefault().Name;
                        apiSpec.Value = spec.Value;
                        apiSpecs.Add(apiSpec);
                    }
                    apiProduct.Specifications = apiSpecs;
                    return Ok(apiProduct);
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpGet("Category/{Name}")]
        public IActionResult Category(string name)
        {
            try
            {
                Category category = _context.Categories.Where(c => c.Name == name).FirstOrDefault();
                if (category == null)
                {
                    return NotFound();
                }
                var products = _context.Products.Where(p => p.CategoryId == category.Id).ToList();
                var apiProducts = new List<ProductApiModel>();
                foreach (var product in products)
                {
                    var apiProduct = new ProductApiModel();
                    apiProduct.Code = product.Code;
                    apiProduct.Name = product.Name;
                    apiProduct.Description = product.Description;
                    apiProduct.Price = product.Price;
                    apiProduct.ImgUrl = product.ImgUrl;
                    apiProduct.Factory = _context.Factories.Where(f => f.Id == product.FactoryId).FirstOrDefault().Name;
                    apiProduct.Category = _context.Categories.Where(c => c.Id == product.CategoryId).FirstOrDefault().Name;

                    var uniqueSpecs = _context.ProductUniqueSpecs.Where(pus => pus.ProductId == product.Id).ToList();
                    var specs = _context.ProductSpecs.Where(ps => ps.ProductId == product.Id).ToList();
                    var apiSpecs = new List<SpecificationsApiModel>();

                    foreach (var spec in uniqueSpecs)
                    {
                        var apiSpec = new SpecificationsApiModel();
                        apiSpec.Name = spec.Name;
                        apiSpec.Value = spec.Value;
                        apiSpecs.Add(apiSpec);
                    }

                    foreach (var spec in specs)
                    {
                        var apiSpec = new SpecificationsApiModel();
                        apiSpec.Name = _context.CategorySpecs.Where(cs => cs.Id == spec.CategorySpecId).FirstOrDefault().Name;
                        apiSpec.Value = spec.Value;
                        apiSpecs.Add(apiSpec);
                    }
                    apiProduct.Specifications = apiSpecs;
                    apiProducts.Add(apiProduct);
                }
                return Ok(apiProducts);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
