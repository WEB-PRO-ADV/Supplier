using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Supplier.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Code is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Price")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Image Url is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Image")]
        public string ImgUrl { get; set; }
        [Required(ErrorMessage = "Image Name is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Image Name")]
        public string ImgName { get; set; }
        [Display(Name = "Factory")]
        [Required(ErrorMessage = "Factory is required")]
        public int FactoryId { get; set; }
        public Factory Factory { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId {get; set;}
        public Category Category { get; set; }
        public ICollection<ProductUniqueSpec> ProductUniqueSpecs { set; get; }
        public ICollection<ProductSpec> ProductSpec { get; set; }

    }
}
