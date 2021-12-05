
using System;
using System.Collections.Generic;
using System.Text;
using ViewModels.Catalog.Products;

namespace ViewModels.Common
{
    public class PagedResult<T> 
    {
       public List<T> items { get; set; }
        public List<ProductViewModel> Items { get; set; }
        public int TotalRecord { set; get; }
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
