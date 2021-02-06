using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Supplier.Models;

namespace Supplier.ViewModels
{
    public class CategorySpecsViewModel
    {
        public int CategorySpecsCtr { get; set; }
        public CategorySpec CategorySpec { get; set; }
        public List<ProductSpec> ProductSpec { get; set; }
    }
}
