using Supplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Supplier.ViewModels
{
    public class NewCategoryviewModel
    {
        public Category Category { get; set; }
        public List<CategorySpec> CategorySpecs { get; set; }
        public int CatSpecsCtr { get; set; }
    }
}
