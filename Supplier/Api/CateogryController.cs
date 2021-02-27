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
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly SupplierDbContext _context;

        public CategoryController(SupplierDbContext context)
        {
            _context = context;
        }

        [Produces("application/json")]
        [HttpGet("")]
        public async Task<IActionResult> all()
        {
            try
            {
                var categories = _context.Categories.ToList();
                var apiCategories = new List<CategoryApiModel>();

                foreach(var category in categories)
                {
                    var apiCategory = new CategoryApiModel();
                    apiCategory.Name = category.Name;
                    apiCategory.Description = category.Description;

                    var specs = _context.CategorySpecs.Where(cs => cs.CategoryId == category.Id).ToList();

                    apiCategory.Specifications = new List<string>();
                    foreach (var spec in specs)
                    {
                        apiCategory.Specifications.Add(spec.Name);
                    }

                    apiCategories.Add(apiCategory);
                }
                return Ok(apiCategories);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpGet("Name/{name}")]
        public async Task<IActionResult> Name(string name)
        {
            try
            {
                var category = _context.Categories.Where(c => c.Name == name).FirstOrDefault();
                if(category == null)
                {
                    return BadRequest();
                }
                var apiCategory = new CategoryApiModel();
                apiCategory.Name = category.Name;
                apiCategory.Description = category.Description;

                var specs = _context.CategorySpecs.Where(cs => cs.CategoryId == category.Id).ToList();

                apiCategory.Specifications = new List<string>();
                foreach (var spec in specs)
                {
                    apiCategory.Specifications.Add(spec.Name);
                }
                return Ok(apiCategory);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
