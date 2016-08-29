using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace ShopifyAPIAdapterLibrary
{
    /// <summary>
    /// This class is used to make Shopify API calls 
    /// </summary>
    /// <remarks>
    /// You will first need to use the ShopifyAPIAuthorizer to obtain the required authorization.
    /// </remarks>
    /// <seealso cref="http://api.shopify.com/"/>
    public class EtsyAPIClient
    {
        /// <summary>
        /// Creates an instance of this class for use with making API Calls
        /// </summary>
        /// <param name="state">the authorization state required to make the API Calls</param>
        public EtsyAPIClient(EtsyAuthorizationState state)
        {
            this.State = state;
        }

        /// <summary>
        /// Creates an instance of this class for use with making API Calls
        /// </summary>
        /// <param name="state">the authorization state required to make the API Calls</param>
        /// <param name="translator">the translator used to transform the data between your C# client code and the Etsy API</param>
        public EtsyAPIClient(EtsyAuthorizationState state, IDataTranslator translator)
        {
            this.State = state;
            this.Translator = translator;
        }

        public EtsyAPIClient(EtsyAuthorizationState authState, JavaScriptSerializer javaScriptSerializer)
        {
            this.authState = authState;
            this.javaScriptSerializer = javaScriptSerializer;
        }

        /// <summary>
        /// Make an HTTP Request to the Etsy API
        /// </summary>
        /// <param name="method">method to be used in the request</param>
        /// <param name="path">the path that should be requested</param>
        /// <seealso cref="http://api.Etsy.com/"/>
        /// <returns>the server response</returns>
        public object Call(HttpMethods method, string path)
        {
            return Call(method, path, null);
        }

        /// <summary>
        /// Make an HTTP Request to the Etsy API
        /// </summary>
        /// <param name="method">method to be used in the request</param>
        /// <param name="path">the path that should be requested</param>
        /// <param name="callParams">any parameters needed or expected by the API</param>
        /// <seealso cref="http://api.Etsy.com/"/>
        /// <returns>the server response</returns>
        public object Call(HttpMethods method, string path, object callParams)
        {
            string url = String.Format("https://openapi.etsy.com/v2/", path);
            var restClient = new RestClient(url);
            restClient.Authenticator = OAuth1Authenticator.ForProtectedResource(State.ConsumerKey, State.ConsumerSecret, State.AccessToken,
              State.Secret);
            RestRequest request = new RestRequest(path);
            if (method == HttpMethods.POST)
            {
                request.Method = Method.POST;
                request.AddHeader("Accept", "application/json"); 
                request.Parameters.Clear();
                request.AddParameter("application/json", Translator.Encode(callParams), ParameterType.RequestBody); 
            }
                IRestResponse queueResponse = restClient.Execute(request);//.Execute<Queue>(request);
            string result = queueResponse.Content;
 

            //At least one endpoint will return an empty string, that we need to account for.
            if (string.IsNullOrWhiteSpace(result))
                return null;

            if (Translator != null)
                return Translator.Decode(result);

            return result;
        }
        //UploadImages to etsy
        public string UploadPic(string pathImage, string listingId)
        {
            string url = String.Format("https://openapi.etsy.com/v2/listings/{0}", listingId);
            var restClient = new RestClient(url);
            restClient.Authenticator = OAuth1Authenticator.ForProtectedResource(State.ConsumerKey, State.ConsumerSecret, State.AccessToken,
              State.Secret);
            RestRequest request = new RestRequest("/images");
            request.Method = Method.POST;
            //request.RequestFormat = RestSharp.DataFormat.Json;   

            request.AlwaysMultipartFormData = true;

            request.AddFileBytes("image", file_get_byte_contents(pathImage), "image.jpg", "image/jpg");
            //request.JsonSerializer.ContentType = "multipart/form-dataheader";

            var response = restClient.Execute(request);

            return response.Content ;
        }

        static byte[] file_get_byte_contents(string fileName)
        {
            byte[] sContents;
            if (fileName.ToLower().IndexOf("https:") > -1)
            {
                // URL 
                System.Net.WebClient wc = new System.Net.WebClient();
                sContents = wc.DownloadData(fileName);
            }
            else
            {
                // Get file size
                FileInfo fi = new FileInfo(fileName);

                // Disk
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                sContents = br.ReadBytes((int)fi.Length);
                br.Close();
                fs.Close();
            }

            return sContents;
        }
        /// <summary>
        /// Make a Get method HTTP request to the Etsy API
        /// </summary>
        /// <param name="path">the path where the API call will be made.</param>
        /// <seealso cref="http://api.Etsy.com/"/>
        /// <returns>the server response</returns>
        public object Get(string path)
        {
            return Get(path, null);
        }

        /// <summary>
        /// Make a Get method HTTP request to the Etsy API
        /// </summary>
        /// <param name="path">the path where the API call will be made.</param>
        /// <param name="callParams">the querystring params</param>
        /// <seealso cref="http://api.Etsy.com/"/>
        /// <returns>the server response</returns>
        public object Get(string path, NameValueCollection callParams)
        {
            return Call(HttpMethods.GET, path, callParams);
        }

        /// <summary>
        /// Make a Post method HTTP request to the Etsy API
        /// </summary>
        /// <param name="path">the path where the API call will be made.</param>
        /// <param name="data">the data that this path will be expecting</param>
        /// <seealso cref="http://api.Etsy.com/"/>
        /// <returns>the server response</returns>
        public object Post(string path, object data)
        {
            return Call(HttpMethods.POST, path, data);
        }

        /// <summary>
        /// Make a Put method HTTP request to the Etsy API
        /// </summary>
        /// <param name="path">the path where the API call will be made.</param>
        /// <param name="data">the data that this path will be expecting</param>
        /// <seealso cref="http://api.Etsy.com/"/>
        /// <returns>the server response</returns>
        public object Put(string path, object data)
        {
            return Call(HttpMethods.PUT, path, data);
        }

        /// <summary>
        /// Make a Delete method HTTP request to the Etsy API
        /// </summary>
        /// <param name="path">the path where the API call will be made.</param>
        /// <seealso cref="http://api.Etsy.com/"/>
        /// <returns>the server response</returns>
        public object Delete(string path)
        {
            return Call(HttpMethods.DELETE, path);
        }

        /// <summary>
        /// Get the content type that should be used for HTTP Requests
        /// </summary>
        private string GetRequestContentType()
        {
            if (Translator == null)
                return DefaultContentType;
            return Translator.GetContentType();
        }

        /// <summary>
        /// The enumeration of HTTP Methods used by the API
        /// </summary>
        public enum HttpMethods
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        /// <summary>
        /// The default content type used on the HTTP Requests to the Etsy API
        /// </summary>
        protected static readonly string DefaultContentType = "application/json";
        private EtsyAuthorizationState authState;
        private JavaScriptSerializer javaScriptSerializer;

        /// <summary>
        /// The state required to make API calls.  It contains the access token and
        /// the name of the shop that your app will make calls on behalf of
        /// </summary>
        protected EtsyAuthorizationState State { get; set; }

        /// <summary>
        /// Used to translate the data sent and recieved by the Etsy API
        /// </summary>
        /// <example>
        /// This could be used to translate from C# objects to XML or JSON.  Thus making your code
        /// that consumes this class much more clean
        /// </example>
        protected IDataTranslator Translator { get; set; }
    }


}
