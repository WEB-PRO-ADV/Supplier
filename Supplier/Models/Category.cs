using System.Collections.Generic;

namespace Supplier.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<CategorySpec> CategorySpecs { get; set; }
    }
}
