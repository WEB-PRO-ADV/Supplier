using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Supplier.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<CategorySpec> CategorySpecs { get; set; }
    }
}
