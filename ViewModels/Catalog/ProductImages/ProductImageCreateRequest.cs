using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.Catalog.ProductImages
{
   public class ProductImageCreateRequest
    {
       
        public int ProductId { get; set; }
       
        public string Caption { get; set; }
        public bool IsDefault { get; set; }
     
        public int SortOrder { get; set; }
        public long FileSize { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
