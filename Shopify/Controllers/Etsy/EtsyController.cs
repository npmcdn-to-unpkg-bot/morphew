using Shopify.Shopify;
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
            return View();
        }
    }
}