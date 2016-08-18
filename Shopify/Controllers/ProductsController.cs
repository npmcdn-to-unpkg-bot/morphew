using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopify.Shopify;
using ShopifyAPIAdapterLibrary;
using Shopify.Models;

namespace Shopify.Controllers
{
    
    public class ProductsController : Controller
    {
        ShopifyAPIClient _shopify;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ShopifyAuthorizationState authState = ShopifyAuthorize.GetAuthorizationState(this.HttpContext);
            if (authState != null)
            {
                _shopify = new ShopifyAPIClient(authState, new JsonDataTranslator());
            }
        }

        public ActionResult Index()
        {
            // get all shopify products
            
            return View();
        }
        [HttpPost]
        public bool pushpush(List<Product> products)
        {
            //string[] collections = new string[] { "black", "red", "blue", "floral", "print", "pink", "purple", "yellow",
            //    "grey", "green", "asian", "bridal", "cocktail", "mod","cat" };
            
            dynamic collectionsResponse = _shopify.Get("/admin/custom_collections.json");
            
            foreach (var item in products)
            {
                try
                {
                    ProductViewModel send = new ProductViewModel();
                    send.product = item;
                    dynamic createproductResponse = this._shopify.Post("/admin/products.json", send);
                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var json = serializer.Serialize(collectionsResponse.custom_collections);
                    if (createproductResponse.error == null)
                    {
                        //send new arrival
                        Collect cc = new Collect();
                        cc.collection_id = 26468335;
                        cc.product_id = createproductResponse.product.id;

                        this._shopify.Post("/admin/collects.json", new CollectViewModel(cc));
                        //send clothing
                        cc.collection_id = 26417119;
                        cc.product_id = createproductResponse.product.id;
                        this._shopify.Post("/admin/collects.json", new CollectViewModel(cc));
                        //assign the collections ids 
                        //Console.WriteLine(item.title);
                    }
                }
                catch(Exception e)
                {
                    //Console.WriteLine(e.ToString());
                }
            }

            return false;
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection Product)
        {
            try
            {
                // From the shopify docs, this is the expected format
                //{
                //  "product": {
                //    "title": "Apple main product"
                //  }
                //}
                //so create the .NET equivalent of that
                dynamic productData = new
                {
                    product = new
                    {
                        title = Product["Title"]
                    }
                };
                dynamic createproductResponse = this._shopify.Post("/admin/products.json", productData);

                //check response for errors

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // handle the exception
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            dynamic productData = this._shopify.Get(String.Format("/admin/custom_Products/{0}.json", id));

            ViewBag.product = productData.product;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection Product)
        {
            try
            {
                // TODO: Add update logic here
                dynamic productData = new
                {
                    product = new
                    {
                        title = Product["Title"]
                    }
                };
                dynamic updateproductResponse = this._shopify.Put(String.Format("/admin/Products/{0}.json", id), productData);

                //TODO: Check the repsponse and make sure the action you wanted to happen happened

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            dynamic productData = this._shopify.Get(String.Format("/admin/Products/{0}.json", id));

            ViewBag.product = productData.product;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection Product)
        {
            try
            {
                dynamic deleteproductResponse = this._shopify.Delete(String.Format("/admin/Products/{0}.json", id));

                //TODO: Check the repsponse and make sure the action you wanted to happen happened

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
