using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supplier.APIModels
{
    public class ProductApiModel
    {       
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImgUrl { get; set; }
        public string Factory { get; set; }
        public string Category { get; set; }

        public List<SpecificationsApiModel> Specifications { get; set; }
    }
}
