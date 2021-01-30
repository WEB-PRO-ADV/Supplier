using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supplier.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImgUrl { get; set; }
        public string ImgName { get; set; }
        public int FactoryId { get; set; }
        public Factory Factory { get; set; }
        public int CategoryId {get; set;}
        public Category Category { get; set; }
        public ICollection<ProductUniqueSpec> ProductUniqueSpecs { set; get; }
        public ICollection<ProductSpec> ProductSpec { get; set; }

    }
}
