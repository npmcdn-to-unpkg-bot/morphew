using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopify.Models
{
    public class Variant
    {
        public string option1 { get; set; }
        public string price { get; set; }
        public string sku { get; set; }
        public string title { get; set; }
        public double grams { get; set; }
        public double weight { get; set; }
        public string weight_unit { get; set; }
        public string inventory_quantity { get; set; }
        public string barcode { get; set; }
        public string fulfillment_service { get; set; }
        public string inventory_management { get; set; }
        public string inventory_policy { get; set; }
        public string position { get; set; }
        public bool requires_shipping { get; set; }
        public bool taxable { get; set; }
        public Variant()
        {
            option1 = "Default Title";
          
            inventory_quantity = "1";
            title = "Default Title";
            weight_unit = "lb";
            grams = 2268;
            weight = 5;
            fulfillment_service = "manual";
            position = "1";
            inventory_management = "shopify";
            inventory_policy = "deny";
            fulfillment_service = "manual";
            requires_shipping = true;
            taxable = true;
        }

        public Variant(string _price, string _sku, string _barcode)
        {
            option1 = "default";
            price = _price;
            sku = _sku;
            inventory_quantity = "1";
            barcode = _barcode;
            weight_unit = "lb";
            grams = 2268;
            weight = 5;
            fulfillment_service = "manual";
            position = "1";
            inventory_management = "shopify";
            inventory_policy = "deny";
            fulfillment_service = "manual";
            requires_shipping = true;
            taxable = true;


        }
    }
}