using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;

namespace MDXLib.API
{
    public class MDXParser
    {
        public string userAPICode;
        private Uri serverAddress = new Uri("http://motiondex.com/");
        HttpClient client;
        public MDXParser(string userAPI)
        {
            client = new HttpClient();
            userAPICode = userAPI;
        }
        public string GetFile(string api)
        {
            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("apicode", userAPICode),
                    new KeyValuePair<string,string>("apiid", api)
                });
            var result = client.PostAsync(new Uri(serverAddress, RequestType.FileFromAPI.ToString()), content).Result;
            result.EnsureSuccessStatusCode();
            return result.Content.ReadAsStringAsync().Result;
        }
    }
    public enum RequestType
    {
        FileFromAPI = "fileFromID.php"
    }
}
