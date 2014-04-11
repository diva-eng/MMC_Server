using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace MDXLib.data
{
    //Basic Data model for MDX responses
    public class MDXFile
    {
        protected Uri serverAddress = new Uri("http://motiondex.com/resources");
        public string type;
        public string apiid;
        public string filename;
        public string fullpreview;
        public string preview;
        public Uri GetPreview()
        {
            return PathBuilder(preview);
        }
        public Uri GetFullPreview()
        {
            return PathBuilder(fullpreview);
        }
        public Uri GetFile()
        {
            return PathBuilder(filename);
        }
        private Uri PathBuilder(string file)
        {
            UriBuilder builder = new UriBuilder(this.serverAddress);
            builder.Path += "/" + type;
            builder.Path += "/" + apiid;
            builder.Path += "/" + file;
            return builder.Uri;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class MDXPost
    {
        public string name;
        public string type;
        public string apiid;
        public string uploader;
        public string filename;
        public string preview;
        public string fullpreview;
        public string embed;
        public string date;
        public string description;
        public string artist;
        public string artisturl;
        public string memo;
        public string[] tags;
        public int hit;
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class MDXSong
    {
    }
}
