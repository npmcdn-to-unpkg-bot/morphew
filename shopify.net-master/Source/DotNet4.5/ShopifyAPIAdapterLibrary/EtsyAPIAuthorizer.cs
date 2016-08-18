using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using Google.Apis;

namespace ShopifyAPIAdapterLibrary
{
    /// <summary>
    /// this class is used to obtain the authorization
    /// from the shopify customer to make api calls on their behalf
    /// </summary>
    public class EtsyAPIAuthorizer
    {
        private string _apiKey;
        private string _secret;

        /// <summary>
        /// Creates an instance of this class in order to obtain the authorization
        /// from the shopify customer to make api calls on their behalf
        /// </summary>
        /// <param name="shopName">name of the shop to make the calls for.</param>
        /// <param name="apiKey">the unique api key of your app (obtained from the partner area when you create an app).</param>
        /// <param name="secret">the secret associated with your api key.</param>
        /// <remarks>make sure that the shop name parameter is the only the subdomain part of the myshopify.com url.</remarks>
        public EtsyAPIAuthorizer(string apiKey, string secret)
        {
            if (apiKey == null)
                throw new ArgumentNullException("apiKey", "Make sure you have this in your config file.");
            if (secret == null)
                throw new ArgumentNullException("secret", "Make sure you have this in your config file.");

            if (apiKey.Length == 0)
                throw new ArgumentException("Make sure you have this in your config file.", "apiKey");
            if (secret.Length == 0)
                throw new ArgumentException("Make sure you have this in your config file.", "secret");

            
           
            this._apiKey = apiKey;
            this._secret = secret;
        }

        /// <summary>
        /// Get the URL required by you to redirect the User to in which they will be 
        /// presented with the ability to grant access to your app with the specified scope
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        public string GetAuthorizationURL(string[] scope, string redirectUrl = null)
        {
            var oauth = new Manager();
            // the URL to obtain a temporary "request token"
            var rtUrl = "https://openapi.etsy.com/v2/oauth/request_token?scope=email_r%20listings_r";
            oauth["consumer_key"] = _apiKey;
            oauth["consumer_secret"] = _secret;
            oauth["callback_url"] = HttpUtility.UrlEncode(redirectUrl);
            OAuthResponse oar = oauth.AcquireRequestToken(rtUrl, "POST");
            return HttpUtility.UrlDecode(oar["login_url"]);
        }

        /// <summary>
        /// After the shop owner has authorized your app, Shopify will give you a code.
        /// Use this code to get your authorization state that you will use to make API calls
        /// </summary>
        /// <param name="code">a code given to you by shopify</param>
        /// <returns>Authorization state needed by the API client to make API calls</returns>
        public EtsyAuthorizationState AuthorizeClient(string code)
        {
            return new EtsyAuthorizationState
            {
                AccessToken = code
            };

        }
    }

}
