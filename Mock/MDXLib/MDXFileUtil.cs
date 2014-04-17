using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;

using MDXLib.Data;

namespace MDXLib.FileUtil
{
    public enum MDXType
    {
        PMX,
        PMD,
        VMD,
        VPD,
        PMDA
    }
    public class MDXFilePair
    {
        public string FilePath { set; get; }
        public MDXType FileType { set; get; }
        public MDXFilePair(string path, MDXType type) { FilePath = path; FileType = type; }
    }
    public static class MDXFileUtil
    {
        static string [] SupportedFile = { ".pmx", ".pmd", ".vmd", ".vpd", ".pmd"};
        public static List<MDXFilePair> UnpackAndGetFilePath(MDXFile file, bool filterBaseOnType = false)
        {
            string filePath = Download(file);
            //CATCH FILE ERROR
            if (filePath.Split(' ')[0] == "ERROR_DOWNLOADING")
                return null;
            //Check for existing file so no need to extract
            if (File.Exists(Path.Combine(filePath, file.filename)))
            {
                //Extract if zip
                if (Path.GetExtension(file.filename) == ".zip")
                {
                    ZipFile.ExtractToDirectory(Path.Combine(filePath, file.filename), Path.Combine(filePath, file.apiid));
                    File.Delete(Path.Combine(filePath, file.filename));
                }
                else
                {
                    File.Move(Path.Combine(filePath, file.filename), Path.Combine(filePath, file.apiid, file.filename));
                }
            }
            filePath = Path.Combine(filePath, file.apiid);
            DirectoryInfo FileDirectory = new DirectoryInfo(filePath);
            List<MDXFilePair> ReturnFiles = new List<MDXFilePair>();
            if (filterBaseOnType)
            {
            }
            else
            {
                IEnumerable<FileInfo> FilesFound = GetFilesByExtensions(FileDirectory, SupportedFile);
                foreach (FileInfo f in FilesFound)
                {
                    if(f.Extension == ".pmd")
                        ReturnFiles.Add(new MDXFilePair(f.FullName, MDXType.PMD));
                    if ((f.Extension == ".pmd") && file.type == "accessory")
                        ReturnFiles.Add(new MDXFilePair(f.FullName, MDXType.PMDA));
                    if (f.Extension == ".pmx")
                        ReturnFiles.Add(new MDXFilePair(f.FullName, MDXType.PMX));
                    if (f.Extension == ".vmd")
                        ReturnFiles.Add(new MDXFilePair(f.FullName, MDXType.VMD));
                    if (f.Extension == ".vpd")
                        ReturnFiles.Add(new MDXFilePair(f.FullName, MDXType.VPD));
                }
            }
            return ReturnFiles;
        }
        private static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles("*", SearchOption.AllDirectories);
            return files.Where(f => extensions.Contains(f.Extension));
        }
        public static string Download(MDXFile file)
        {
            CheckDir();
            WebClient client = new WebClient();
            string save = Path.Combine("data", file.type);
            if (!Directory.Exists(Path.Combine(save, file.apiid)))
            {
                Directory.CreateDirectory(Path.Combine(save, file.apiid));
                try
                {
                    client.DownloadFile(file.GetFile(), Path.Combine(save, file.filename));
                    client.DownloadFile(file.GetPreview(), Path.Combine(save, file.apiid, file.preview));
                    client.DownloadFile(file.GetFullPreview(), Path.Combine(save, file.apiid, file.fullpreview));
                }
                catch (Exception e)
                {
                    return "ERROR_DOWNLOADING Exception:" + e.Message;
                }
            }
            return save;
        }
        public static void CheckDir()
        {
            if (!Directory.Exists("data/model"))
                Directory.CreateDirectory("data/model");
            if (!Directory.Exists("data/motion"))
                Directory.CreateDirectory("data/motion");
            if (!Directory.Exists("data/pose"))
                Directory.CreateDirectory("data/pose");
            if (!Directory.Exists("data/accessory"))
                Directory.CreateDirectory("data/accessory");
            if (!Directory.Exists("data/song"))
                Directory.CreateDirectory("data/song");
        }
    }
}
