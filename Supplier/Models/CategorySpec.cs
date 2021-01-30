using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supplier.Models
{
    public class CategorySpec
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductSpec> ProductSpecs { get; set; }

    }
}
