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
    [Route("api/category")]
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
                var p = _context.Categories.ToList();
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
                var p = _context.Categories.Find(id);
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
                var p = _context.Categories.Where(x => x.Name == name).ToList();
                return Ok(p);
            }
            catch
            {
                return BadRequest();
            }
        }        

    }
}
