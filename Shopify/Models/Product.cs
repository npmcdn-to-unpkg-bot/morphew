using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopify.Models
{
    public class Product
    {
        public string tags { get; set; } 
        public string title { get; set; }
        public string body_html { get; set; }
        public string vendor { get; set; }
        public string product_type { get; set; }
        public string published { get; set; }
        public List<Variant> variants { get; set; }
        public List<Images> images { get; set; }
        public Product()
        {

        }
    }
    public class ProductViewModel
    {
        public Product product { get; set; }
    }
}