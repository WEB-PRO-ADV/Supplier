using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supplier.Models;

namespace Supplier.Api
{
    [Route("api/product")]
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
        public async Task<IActionResult> all()
        {
            try
            {
                var p = _context.Products.ToList();
                //foreach(var tmp in p)
                //{
                //    tmp.Category = _context.Categories.Where(c => c.Id == tmp.CategoryId).FirstOrDefault();
                //}
                return Ok(p);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpGet("ById/{id}")]
        public async Task<IActionResult> ById(int id)
        {
            try
            {
                var p = _context.Products.Find(id);
                return Ok(p);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpGet("ByName/{Name}")]
        public async Task<IActionResult> ByName(string name)
        {
            try
            {
                var p = _context.Products.Where(x => x.Name == name).ToList();
                return Ok(p);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpGet("ByCat/{Name}")]
        public async Task<IActionResult> ByCat(string name)
        {
            try
            {
                var p = _context.Products.Where(p => p.Category.Name == name).ToList();
                return Ok(p);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
