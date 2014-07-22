using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text;
using System.IO;
using AzureStorageLib;

namespace BabybookAPI.BLL
{
    public class AzureStorageBLL
    {
        //public static BlobService CreateBlobServiceClient()
        //{
        //    var blobService = new BlobService(ConfigurationManager.AppSettings["AzureStorageConnection"]);
        //    return blobService;
        //}
        public static void UploadFilesByFolder(BlobService blobService, string container, string folderToUpload, string virtualFolder = "")
        {
            var allFiles = Directory.GetFiles(folderToUpload);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var f in allFiles)
            {
                dic.Add(virtualFolder + Path.GetFileName(f), f);
            }

            blobService.UploadFiles(container, dic);
        }
    }


}