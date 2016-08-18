using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopifyAPIAdapterLibrary;

namespace Shopify.Shopify
{
    public class EtsyAuthorize : AuthorizeAttribute
    {
        private static readonly string AuthSessionKey = "Etsy_auth_state";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionState"></param>
        /// <param name="state"></param>
        public static void SetAuthorization(System.Web.HttpContextBase httpContext, EtsyAuthorizationState state)
        {
            httpContext.Session[AuthSessionKey] = state;
        }

        /// <summary>
        /// Test to see if the current http context is authorized for access to Etsy API
        /// </summary>
        /// <param name="httpContext">current httpContext</param>
        /// <returns>true if the current http context is authorized for access to Etsy API, otherwise false</returns>
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var authState = GetAuthorizationState(httpContext);
            if (authState == null || String.IsNullOrWhiteSpace(authState.AccessToken))
                return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static EtsyAuthorizationState GetAuthorizationState(System.Web.HttpContextBase httpContext)
        {
            return httpContext.Session[AuthSessionKey] as EtsyAuthorizationState;
        }
    }
}