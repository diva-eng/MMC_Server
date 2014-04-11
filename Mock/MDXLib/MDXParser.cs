using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;

using MDXLib.data;
using Newtonsoft.Json;

namespace MDXLib.API
{
    public class MDXParser
    {
        public string userAPICode;
        private Uri serverAddress = new Uri("http://motiondex.com/api/");
        HttpClient client;
        public MDXParser(string userAPI)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.ExpectContinue = false;
            userAPICode = userAPI;
        }
        public string GetFromServer(RequestType type, string api)
        {
            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("apicode", userAPICode),
                    new KeyValuePair<string,string>("apiid", api)
                });
            var result = client.PostAsync(new Uri(serverAddress, filePath[(int)type]), content).Result;
            result.EnsureSuccessStatusCode();
            return result.Content.ReadAsStringAsync().Result;
        }
        public string GetPostString(string api)
        {
            return GetFromServer(RequestType.PostFromAPI, api);
        }
        public MDXPost GetPost(string api)
        {
            string json = GetPostString(api);
            return JsonConvert.DeserializeObject<MDXPost>(json);
        }
        public string GetFileString(string api)
        {
            return GetFromServer(RequestType.FileFromAPI, api);
        }
        public MDXFile GetFile(string api)
        {
            string json = GetFileString(api);
            return JsonConvert.DeserializeObject<MDXFile>(json);
        }
        private string[] filePath = new string[]
        {
            "fileFromID.php",
            "postFromID.php"
        };
    }
    public enum RequestType
    {
        FileFromAPI,
        PostFromAPI
    }
}
