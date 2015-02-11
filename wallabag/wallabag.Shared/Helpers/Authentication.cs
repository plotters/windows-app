using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace wallabag.Helpers
{
    class Authentication
    {
        private static Authentication _instance;
        private Authentication()
        {
            Username = "wallabag";
            Password = "wallabag";
            wallabagUrl = new Uri("http://v2.wallabag.org");
        }
        public static Authentication Instance { get { return _instance ?? (_instance = new Authentication()); } }

        public string Username { get; set; }
        public string Password { get; set; }
        public Uri wallabagUrl { get; set; }

        private string sha1(string text)
        {
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(text, BinaryStringEncoding.Utf8);
            HashAlgorithmProvider hashAlgorithm = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            IBuffer hashBuffer = hashAlgorithm.HashData(buffer);

            var strHashBase64 = CryptographicBuffer.EncodeToBase64String(hashBuffer);
            return strHashBase64;
        }
        private string md5(string text)
        {
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(text, BinaryStringEncoding.Utf8);
            HashAlgorithmProvider hashAlgorithm = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer hashBuffer = hashAlgorithm.HashData(buffer);

            var strHashBase64 = CryptographicBuffer.EncodeToBase64String(hashBuffer);
            return strHashBase64;
        }
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }


        public async Task<string> Salt()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("wallabag for WinRT"));

                var response = await client.GetAsync(new Uri(string.Format("{0}/api/salts/{1}.json", wallabagUrl, Username)));
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
                }
            }
            return string.Empty;
        }

        public async Task generateHeaders(HttpRequestHeaderCollection headers)
        {
            string encryptedPassword = sha1(Password + Username + await Salt());
            string salt = await Salt();
            string nonce = md5("nonce_" + DateTime.Now.Ticks).Substring(0, 16);
            string created = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string digest = Convert.ToBase64String(GetBytes(sha1(Convert.FromBase64String(nonce) + created + encryptedPassword)));

            headers.Add("HTTP_AUTHORIZATION", "Authorization profile=\"UsernameToken\"");
            headers.Add("HTTP_x-wsse", string.Format("X-WSSE: UsernameToken Username=\"{0}\", PasswordDigest=\"{1}\", Nonce=\"{2}\", Created=\"{3}\"", Username, digest, nonce, created));
            int senseless = headers.Count;
        }
    }
}
