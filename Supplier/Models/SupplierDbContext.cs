using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Supplier.Models;

namespace Supplier.Models
{
    public class SupplierDbContext : IdentityDbContext
    {

        public SupplierDbContext(DbContextOptions<SupplierDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategorySpec> CategorySpecs { get; set; }
        public DbSet<ProductSpec> ProductSpecs { get; set; }
        public DbSet<ProductUniqueSpec> ProductUniqueSpecs { get; set; }
        public DbSet<Factory> Factories { get; set; }
    }
}
