using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supplier.Models
{
    public class Factory
    {
        public int Id { get; set; }
<<<<<<< Updated upstream
=======
        [Required(ErrorMessage = "Name is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Factory Name")]
>>>>>>> Stashed changes
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
