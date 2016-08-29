using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopify.Models
{
    public class EtsyProduct
    {
        public string url { get; set; }
        public string[] tags { get; set; }
        public string category_id { get; set; }
        public string[] materials { get; set; }
        public string title { get; set; }
        public int quantity { get; set; }
        public string description { get; set; }
        public float price { get; set; }
        public double shipping_template_id { get; set; }
        public string who_made { get; set; }
        public bool is_supply { get; set; }
        public bool should_auto_renew { get; set; }
        
        public string when_made { get; set; }
        public string state { get; set; }

        public List<string> images { get; set; }
        public EtsyProduct()
        {

        }
    }
    public class EtsyProductViewModel
    {
        public EtsyProduct product { get; set; }
    }
}