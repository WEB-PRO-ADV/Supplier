using Supplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supplier.ViewModels
{
    public class NewProductViewModel
    {
        public Product Product { get; set; }
        public ProductUniqueSpec ProductUniqueSpec { get; set; }
        public Category Category { get; set; }
        public List<ProductSpec> ProductSpec { get; set; }
        public Factory Factory { get; set; }
    }
}
