using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using AzureStorageLib;
using System.Diagnostics;
using BabybookAPI.Models;

namespace BabybookAPI.Controllers
{
    [RoutePrefix("api/Images")]
    public class ImagesController : ApiController
    {
        BlobService blobService = new BlobService(ConfigurationManager.AppSettings["AzureStorageConnection"]); //AzureStorageBLL.CreateBlobServiceClient();

        // GET api/Images/
        public IEnumerable<string> Get()
        {
            //string containerName = "babyimages".ToLower(); //All letters in a container name must be lowercase.
            var result = blobService.FlatListBlobs(BabybookConfig.ContainerName);
            return result;
        }

        [Route("All")]
        public string GetAll()
        {
            Debug.WriteLine("Process GetAll images request");
            //string containerName = "babyimages".ToLower(); //All letters in a container name must be lowercase.
            var result = blobService.FlatListBlobsWithFullInformation(BabybookConfig.ContainerName);
            Debug.WriteLine("Finish GetAll images");
            return result;
        }

        public HttpResponseMessage Get(int id)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            String filePath = HostingEnvironment.MapPath("~/Images/" + GetImageName(id));
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                Image image = Image.FromStream(fileStream);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, ImageFormat.Jpeg);
                    result.Content = new ByteArrayContent(memoryStream.ToArray());
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                }
            }
            return result;
        }    
       
        [Route("SAS")]
        public string GetSAS()
        {
            Debug.WriteLine("Process GetSASUrl images request");
            //string containerName = "babyimages".ToLower(); //All letters in a container name must be lowercase.
            var result = blobService.GetSASUrl(BabybookConfig.ContainerName);
            Debug.WriteLine("Finish GetSASUrl images");
            return result;
        }

        private string GetImageName(int id)
        {
            if (id % 2 == 0) { return "bb.jpg"; }
            else return "b2.jpg";
        }
    }
}
