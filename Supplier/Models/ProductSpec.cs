using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Supplier.Models
{
    public class ProductSpec
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Value is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Value")]
        public String Value { get; set; }
        public int ProductId { get; set; }
        public int CategorySpecId { get; set; }
        public Product Product { get; set; }
        public CategorySpec CategorySpec { get; set; }
    }
}
