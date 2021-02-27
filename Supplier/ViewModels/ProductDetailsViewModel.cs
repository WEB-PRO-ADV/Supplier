using Supplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supplier.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public List<CategorySpec> CategorySpecs { get; set; }
        public List<ProductSpec> ProductSpecs { get; set; }
        public List<ProductUniqueSpec> ProductUniqueSpecs { get; set; }
    }
}
