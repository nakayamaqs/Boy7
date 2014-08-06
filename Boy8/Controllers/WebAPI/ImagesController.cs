using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Boy8.BLL;

namespace Boy8.Controllers.WebAPI
{
    public class ImagesController : ApiController
    {
        // GET api/Images/
        public string Get()
        {
            //string containerName = "babyimages".ToLower(); //All letters in a container name must be lowercase.
            //var blobService = new BlobService(ConfigurationManager.AppSettings["AzureStorageConnection"]); //AzureStorageBLL.CreateBlobServiceClient();
            //var result = blobService.FlatListBlobs(containerName);

            var thePictures = BabyStorage.GetPictures("babyimages").Take(12).ToList();
            return JsonConvert.SerializeObject(thePictures);
        }
    }
}