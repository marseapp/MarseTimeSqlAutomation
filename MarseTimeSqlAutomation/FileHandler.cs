using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;

namespace MarseTimeSqlAutomation
{
    class FileHandler
    {
        private static string pathDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string pathFileName = "rawTimeTable.zip";
        private static string extractPathFolderName = "output";

            
        public void DeleteData(string pathName)
        {
            if (File.Exists(pathName))
            {
                File.Delete(pathName);
            }
            string extractPathName = Path.Combine(pathDirectory, extractPathFolderName);
            if (Directory.Exists(extractPathName))
            {
                Directory.Delete(extractPathName, true);
            }
        }
        
        public string DownloadData()
        {
            
            string pathName = Path.Combine(pathDirectory, pathFileName);
            DeleteData(pathName);
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(Environment.GetEnvironmentVariable("DataSourceLocation")), pathName);
            }
            return pathName;
        }


        public string UnzipData(string pathName)
        {
            string extractPath = Path.Combine(pathDirectory, extractPathFolderName);
            ZipFile.ExtractToDirectory(pathName, extractPath);

            DirectoryInfo directory = new DirectoryInfo(extractPath);
            FileInfo[] Files = directory.GetFiles("*.xml");

            string str = "";
            foreach (FileInfo file in Files)
            {
                str = file.Name;
            }

            return Path.Combine(extractPath, str);

        }

        public string ReadData(string xmlFile)
        {
            string xmlString = System.IO.File.ReadAllText(xmlFile);
            // To convert an XML node contained in string xml into a JSON string   
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            return jsonText;
        }
    }
}
