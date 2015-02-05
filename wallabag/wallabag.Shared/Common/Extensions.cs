using System;
using System.Collections;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace wallabag.Common
{
    class WebViewExtensions
    {
        public static string GetHTML(DependencyObject obj)
        {
            return (string)obj.GetValue(HTMLProperty);
        }

        public static void SetHTML(DependencyObject obj, string value)
        {
            obj.SetValue(HTMLProperty, value);
        }

        // Using a DependencyProperty as the backing store for HTML.  This enables animation, styling, binding, etc... 
        public static readonly DependencyProperty HTMLProperty =
            DependencyProperty.RegisterAttached("HTML", typeof(string), typeof(WebViewExtensions), new PropertyMetadata(0, new PropertyChangedCallback(OnHTMLChanged)));

        private static void OnHTMLChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebView wv = d as WebView;
            if (wv != null)
            {
                wv.NavigateToString((string)e.NewValue);
            }
        }
    }
    public static class HttpClientExtensions
    {
        public async static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri requestUri, IHttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            return await client.SendRequestAsync(request);
        }
    }
    public static class CollectionExtensions
    {
        public static string ToCommaSeperatedString(this ICollection collection)
        {
            if (collection.Count > 0)
            {
                string result = string.Empty;
                foreach (string s in collection)
                {
                    result += s + ",";
                }
                if (result.EndsWith(","))
                {
                    result = result.Remove(result.Length - 1);
                }
                return result;
            }
            else return string.Empty;
        }
    }
}
