using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supplier.Models
{
    public class ProductSpec
    {
        public int Id { get; set; }
        public String Value { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CategorySpecId { get; set; }
        public CategorySpec CategorySpec { get; set; }

    }
}
