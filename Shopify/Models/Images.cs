using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopify.Models
{
    public class Images
    {
        public string src { get; set; }
        public Images(string _src)
        {
            src = _src;
        }
        public Images()
        { }
    }
}