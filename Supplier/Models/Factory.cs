using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Supplier.Models
{
    public class Factory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Factory Name")]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
