using System;
using System.Collections.Generic;
using System.Text;

using ViewModels.Common;

namespace ViewModels.Catalog.Products
{
    public class GetPublicProductPagingRequest : PagingRequestBase

    {
        public string LanguageId { get; set; }
        public int? CategoryId { get; set; }
       // public string Keyword { get; set; }
    }
}
