using System;
using System.Collections.Specialized;
using System.Net;

namespace OMG.Util
{
    public class WebHook : IDisposable
    {
        private WebClient WebClient { get; } =  new WebClient();
        private static NameValueCollection ValuesCollection { get; } = new NameValueCollection();
        public string WebHookString { get; set; }
        public string UserName { get; set; }
        public string ProfilePicture { get; set; }

        public void SendMessage(string message)
        {
            ValuesCollection.Add("username", UserName);
            ValuesCollection.Add("avatar_url", ProfilePicture);
            ValuesCollection.Add("content", message);

            WebClient.UploadValues(WebHookString, ValuesCollection);
        }

        public void Dispose() => WebClient.Dispose();
    }
}