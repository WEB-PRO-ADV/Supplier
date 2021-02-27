using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supplier.APIModels
{
    public class CategoryApiModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Specifications { get; set; }
    }
}
