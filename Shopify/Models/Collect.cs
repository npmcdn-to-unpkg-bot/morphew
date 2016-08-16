using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopify.Models
{
    public class Collect
    {
        public decimal product_id { get; set; } 
        public decimal collection_id { get; set; }
        public Collect()
        {

        }
    }
    public class CollectViewModel
    {
        public Collect collect; 

        public CollectViewModel(Collect cc)
        {
            this.collect = cc;
        }
    }
}