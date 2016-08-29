using System;

namespace ShopifyAPIAdapterLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class EtsyAuthorizationState
    {
        /// <summary>
        /// 
        /// </summary>
        public EtsyAuthorizationState()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string AccessToken { get; set; }
        public string Secret { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
    }
}
