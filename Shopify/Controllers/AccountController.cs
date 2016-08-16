using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Shopify.Models;
using ShopifyAPIAdapterLibrary;

namespace Shopify.Controllers
{
    public class AccountController : Controller
    {
        private string tempOauthToken;
        private string tempsecret;
        private string permanent_token;
        private string permanentSecret;

        /// <summary>
        /// show the login form
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// posted from the login form
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // strip the .myshopify.com in case they added it
                string shopName = model.ShopName.Replace(".myshopify.com", String.Empty);

                // prepare the URL that will be executed after authorization is requested

                Uri requestUrl = this.Url.RequestContext.HttpContext.Request.Url;
                UriBuilder u1 = new UriBuilder(requestUrl.Authority);
                u1.Port = -1;  
                Uri returnURL = new Uri(string.Format("{0}://{1}{2}",
                                                        requestUrl.Scheme,
                                                        u1.Host,
                                                        this.Url.Action("ShopifyAuthCallback", "Account")));

                var authorizer = new ShopifyAPIAuthorizer(shopName, ConfigurationManager.AppSettings["Shopify.ConsumerKey"], ConfigurationManager.AppSettings["Shopify.ConsumerSecret"]);
                var authUrl = authorizer.GetAuthorizationURL(new string[] { ConfigurationManager.AppSettings["Shopify.Scope"] }, returnURL.ToString());
                return Redirect(authUrl);
            }

            return View(model);
        }
        public ActionResult Etsy()
        {
            return View();
        }
        /// <summary>
        /// posted from the etsy form
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Etsy(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // strip the .myshopify.com in case they added it
                string shopName = model.ShopName.Replace(".myshopify.com", String.Empty);

                // prepare the URL that will be executed after authorization is requested

                Uri requestUrl = this.Url.RequestContext.HttpContext.Request.Url;
                UriBuilder u1 = new UriBuilder(requestUrl.Authority);
                u1.Port = -1;
                Uri returnURL = new Uri(string.Format("{0}://{1}{2}",
                                                        requestUrl.Scheme,
                                                        u1.Host!=""?u1.Host:u1.Scheme,
                                                        this.Url.Action("EtsyAuthCallback", "Account")));

                var authorizer = new Etsy_portal(ConfigurationManager.AppSettings["Etsy.ConsumerKey"], ConfigurationManager.AppSettings["Etsy.ConsumerSecret"]);
                

                var authUrl = authorizer.GetConfirmUrl(out tempOauthToken,out tempsecret, returnURL.ToString());
                HttpCookie myCookie = new HttpCookie("tempOauthToken");
                HttpCookie tempsecretcookie = new HttpCookie("tempsecret");
                DateTime now = DateTime.Now;

                myCookie.Value = tempOauthToken;
                myCookie.Expires = now.AddYears(50); // For a cookie to effectively never expire
                tempsecretcookie.Value = tempsecret;
                tempsecretcookie.Expires = now.AddYears(50); // For a cookie to effectively never expire

                // Add the cookie.
                Response.Cookies.Add(myCookie);
                Response.Cookies.Add(tempsecretcookie);
                return Redirect(authUrl);
            }

            return View(model);
        }
        /// <summary>
        /// This action is called by shopify after the authorization has finished
        /// </summary>
        /// <param name="code"></param>
        /// <param name="shop"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShopifyAuthCallback(string code, string shop, string error)
        {
            if (!String.IsNullOrEmpty(error))
            {
                this.TempData["Error"] = error;
                return RedirectToAction("Login");
            }
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(shop))
                return RedirectToAction("Index", "Home");

            var shopName = shop.Replace(".myshopify.com", String.Empty);
            var authorizer = new ShopifyAPIAuthorizer(shopName, ConfigurationManager.AppSettings["Shopify.ConsumerKey"], ConfigurationManager.AppSettings["Shopify.ConsumerSecret"]);

            ShopifyAuthorizationState authState = authorizer.AuthorizeClient(code);
            if (authState != null && authState.AccessToken != null)
            {
                Shopify.ShopifyAuthorize.SetAuthorization(this.HttpContext, authState);
            }

            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// This action is called by Etsy after the authorization has finished
        /// </summary>
        /// <param name="code"></param>
        /// <param name="shop"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EtsyAuthCallback(string oauth_verifier, string oauth_token)
        {
            HttpCookie myCookie = Request.Cookies["tempOauthToken"];

            HttpCookie tempsecret = Request.Cookies["tempsecret"];
            var authorizer = new Etsy_portal(ConfigurationManager.AppSettings["Etsy.ConsumerKey"], ConfigurationManager.AppSettings["Etsy.ConsumerSecret"]);
            authorizer.ObtainTokenCredentials(myCookie.Value, tempsecret.Value, oauth_verifier,out permanent_token, out permanentSecret);
             
            return RedirectToAction("Index", "Home");
        }
    }
}
