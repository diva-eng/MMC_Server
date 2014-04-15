using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using MDXLib.Data;

namespace MDXLib.File
{
    public class MDXFileUtil
    {
        public static List<string> UnpackAndGetFilePath(MDXFile file)
        {
            return null;
        }
        public static string Download(MDXFile file)
        {
            WebClient client = new WebClient();
            string save = file.type + "/" + file.filename;
            client.DownloadFile(file.GetFile(), file.filename);
            return save;
        }
    }
}
