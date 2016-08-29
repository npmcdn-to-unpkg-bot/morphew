using Shopify.Models;
using Shopify.Shopify;
using ShopifyAPIAdapterLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopify.Controllers.Etsy
{
    [EtsyAuthorize]
    public class EtsyController : Controller
    {
        // GET: EtsyHome
        public ActionResult Index()
        {
            //string listing_id = "dd";
            
            //    this._etsy.UploadPic(item.images[i], listing_id.ToString());
            

            return View();
        }
        public ActionResult Listings()
        {
            //dynamic shopResponse = _etsy.Get("/categories/Clothing ");
            dynamic shopResponse = _etsy.Get("shops/MORPHEWCONCEPT/listings/active");

            ViewBag.categories = GetCategories("clothing/women");
            ViewBag.shopResponse = shopResponse;
            return View();
        }
        public List<Category> GetCategories(string tag)
        {
            dynamic categories = _etsy.Get("taxonomy/categories/" + tag);
            List<Category> vasr = categories.results.ToObject<List<Category>>();

            return vasr;
        }
        [HttpPost]
        public bool pushpush(List<EtsyProduct> products)
        {
            foreach (var item in products)
            {
                item.quantity = 1;
                if (item.when_made == "1990s")
                    item.when_made = "1990_1996";
                dynamic createproductResponse = this._etsy.Post("listings", item);
                if (createproductResponse.results != null)
                {
                    long listing_id = createproductResponse.results[0].listing_id.Value;
                    for (int i = 0; i < item.images.Count; i++)
                    {
                        this._etsy.UploadPic(item.images[i], listing_id.ToString());
                    }
                    
                  
                }
            }
            return false;
        }
        public JsonResult GetCategoriesList(string tag)
        {
            dynamic categories = _etsy.Get("taxonomy/categories/" + tag);
            List<Category> vasr = categories.results.ToObject<List<Category>>();

            return Json(vasr, JsonRequestBehavior.AllowGet); ;
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            EtsyAuthorizationState authState = EtsyAuthorize.GetAuthorizationState(this.HttpContext);
            if (authState != null)
            {
                _etsy = new EtsyAPIClient(authState, new JsonDataTranslator());
            }
        }
        EtsyAPIClient _etsy;
    }
}