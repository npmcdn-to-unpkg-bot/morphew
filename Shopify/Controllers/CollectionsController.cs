using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopify.Shopify;
using ShopifyAPIAdapterLibrary;

namespace Shopify.Controllers
{
    [ShopifyAuthorize]
    public class CollectionsController : Controller
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
            // get all shopify blogs
            dynamic blogResponse = _shopify.Get("/admin/custom_collections.json");
            ViewBag.Collections = blogResponse.custom_collections;
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // From the shopify docs, this is the expected format
                //{
                //  "blog": {
                //    "title": "Apple main blog"
                //  }
                //}
                //so create the .NET equivalent of that
                dynamic blogData = new
                {
                    custom_collection = new
                    {
                        title = collection["Title"]
                    }
                };
                dynamic createBlogResponse = this._shopify.Post("/admin/custom_collections.json", blogData);

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
            dynamic blogData = this._shopify.Get(String.Format("/admin/custom_collections/{0}.json", id));

            ViewBag.blog = blogData.blog;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                dynamic blogData = new
                {
                    blog = new
                    {
                        title = collection["Title"]
                    }
                };
                dynamic updateBlogResponse = this._shopify.Put(String.Format("/admin/custom_collections/{0}.json", id), blogData);

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
            dynamic blogData = this._shopify.Get(String.Format("/admin/custom_collections/{0}.json", id));

            ViewBag.blog = blogData.blog;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                dynamic deleteBlogResponse = this._shopify.Delete(String.Format("/admin/custom_collections/{0}.json", id));

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
