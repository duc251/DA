using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.Catalog.Products
{
    public class ProductCreateRequest
    {
        
        public decimal Price { get; set; }
        public decimal OriginalPrice { set; get; }
        public int Stock { get; set; }
        public string Nane { get; set; }
       // public int ViewCount { get; set; }
       // public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public string Details { set; get; }
        public string SeoTitle { get; set; }
        public string SeoDescription { set; get; }
        public string SeoAlias { get; set; }
        public string LanguageId { get; set; }
        public bool? IsFeatured { get; set; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
