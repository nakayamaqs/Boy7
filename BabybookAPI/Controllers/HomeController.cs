using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Hosting;
using AzureStorageLib;
using BabybookAPI.BLL;
using BabybookAPI.Models;

namespace BabybookAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        public ActionResult UploadFile()
        {
            ViewBag.Title = "Upload File to Azure Storage";
            //string containerName = "babyimages".ToLower(); //All letters in a container name must be lowercase.
            var blobService = new BlobService(ConfigurationManager.AppSettings["AzureStorageConnection"]); //AzureStorageBLL.CreateBlobServiceClient();
            //blobService.CreatePublicContainer(containerName);
            blobService.UploadFileToBlob(BabybookConfig.ContainerName,  HostingEnvironment.MapPath("~/Images/ps1.jpg"), "2014/");
            var result = blobService.ListBlobs(BabybookConfig.ContainerName, false).Split(new[] { '\r', '\n' },StringSplitOptions.RemoveEmptyEntries);
            ViewBag.BlobList = result;
            return View();
        }

        public ActionResult UploadFiles()
        {
            ViewBag.Title = "Upload Files to Azure Storage";
            //string containerName = "babyimages".ToLower(); //All letters in a container name must be lowercase.
            var blobService = new BlobService(ConfigurationManager.AppSettings["AzureStorageConnection"]); //AzureStorageBLL.CreateBlobServiceClient();
            //blobService.CreatePublicContainer(BabybookConfig.ContainerName);
            AzureStorageBLL.UploadFilesByFolder(blobService, BabybookConfig.ContainerName, HostingEnvironment.MapPath("~/Images/"), "2014/");
            //blobService.UploadFileToBlob(containerName, HostingEnvironment.MapPath("~/Images/ps1.jpg"), "2014/");
            var result = blobService.ListBlobs(BabybookConfig.ContainerName, false).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            ViewBag.BlobList = result;
            return View();
        }
    }
}
