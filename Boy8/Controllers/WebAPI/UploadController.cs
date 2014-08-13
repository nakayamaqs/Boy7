using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Threading;
using System.Web.UI;
using System.IO;
using Boy8.Infrastructure.Filter;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Boy8.BLL;

namespace Boy8.Controllers.WebAPI
{
    [RoutePrefix("api/upload")]
    public class UploadController : ApiController
    {
        [Route("cloud")]
        // Enable both Get and Post so that our jquery call can send data, and get a status
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage CloudImages()
        {
            // Get a reference to the file that our jQuery sent.  Even with multiple files, they will all be their own request and be the 0 index
            HttpPostedFile file = HttpContext.Current.Request.Files[0];

            // do something with the file in this space 
            // {....}
            // end of file doing

            // Now we need to wire up a response so that the calling script understands what happened
            HttpContext.Current.Response.ContentType = "text/plain";
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = new { name = file.FileName };

            HttpContext.Current.Response.Write(serializer.Serialize(result));
            HttpContext.Current.Response.StatusCode = 200;

            // For compatibility with IE's "done" event we need to return a result as well as setting the context.response
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Route("Azure")]
        [AcceptVerbs("post")]
        [ValidateMimeMultipartContentFilter]
        public async Task<HttpResponseMessage> AzureImages()
        {
            try
            {               
                string root = HttpContext.Current.Server.MapPath("~/App_Data");
                var streamProvider = new MultipartFormDataStreamProvider(root); //HttpContext.Current.Server.MapPath("~/App_Data")
                await Request.Content.ReadAsMultipartAsync(streamProvider);
                StringBuilder sb = new StringBuilder(); // Holds the response body 
                
                // This illustrates how to get the form data. 
                foreach (var key in streamProvider.FormData.AllKeys)
                {
                    foreach (var val in streamProvider.FormData.GetValues(key))
                    {
                        sb.Append(string.Format("{0}: {1}\n", key, val));
                    }
                }

                // This illustrates how to get the file names for uploaded files. 
                foreach (var file in streamProvider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);                    
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    //sb.Append(string.Format("Uploaded file locally: {0} ({1} bytes) origin name: {2} \n", fileInfo.Name, fileInfo.Length,Path.GetExtension(file.Headers.ContentDisposition.FileName.Replace("\"", string.Empty))));

                    var filename = file.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                    using (var filestream = File.OpenRead(file.LocalFileName)) 
                    { 
                        BabyStorage.UploadPictures("babyimages", filename, filestream, "boy7test/2014/");
                        sb.Append(string.Format("Uploaded file to Azure: {0} ({1} bytes) folder: {2} \n", filename,  fileInfo.Length,  "boy7test/2014/"));
                    } 
                    File.Delete(file.LocalFileName); 
                }
         
                return new HttpResponseMessage()
                {
                    Content = new StringContent(sb.ToString())
                };

                //return new FileResult
                //{
                //    FileNames = streamProvider.FileData.Select(entry => entry.LocalFileName),
                //    Names = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.FileName),
                //    ContentTypes = streamProvider.FileData.Select(entry => entry.Headers.ContentType.MediaType),
                //    Description = streamProvider.FormData["description"],
                //    CreatedTimestamp = DateTime.UtcNow,
                //    UpdatedTimestamp = DateTime.UtcNow,
                //    DownloadLink = "TODO, will implement when file is persisited"
                //};
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("test")]
        [HttpPost]
        [ValidateMimeMultipartContentFilter]
        public async Task<HttpResponseMessage> PostFile()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                StringBuilder sb = new StringBuilder(); // Holds the response body

                // Read the form data and return an async task.
                var theDataStreamProvider = await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the form data.
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        sb.Append(string.Format("{0}: {1}\n", key, val));
                    }
                }

                // This illustrates how to get the file names for uploaded files.
                foreach (var file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));
                }

                Trace.WriteLine("Just about to return -- Finished!");
                return new HttpResponseMessage()
                {
                    Content = new StringContent(sb.ToString())
                };
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        public class FileResult
        {
            public IEnumerable<string> FileNames { get; set; }
            public string Description { get; set; }
            public DateTime CreatedTimestamp { get; set; }
            public DateTime UpdatedTimestamp { get; set; }
            public string DownloadLink { get; set; }
            public IEnumerable<string> ContentTypes { get; set; }
            public IEnumerable<string> Names { get; set; }
        }
    }
}
