using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Boy8.BLL;

namespace AzureStorageLib
{
    public class AzureBlobStorageMultipartProvider : MultipartFormDataStreamProvider
    {
        public CloudBlobContainer Container;
        public AzureBlobStorageMultipartProvider(CloudBlobContainer container, string tempPath)
            : base(tempPath)
        {
            Container = container;
        }
        public override Task ExecutePostProcessingAsync()
        {
            // Upload the files to azure blob storage and remove them from local disk 
            foreach (var fileData in this.FileData)
            {
                string fileName = Path.GetFileName(fileData.Headers.ContentDisposition.FileName.Trim('"'));

                var filename = fileData.LocalFileName;



                // Retrieve reference to a blob 
                string fileNameBlob = Path.GetFileName(fileData.LocalFileName.Trim('"'));

                var blob = Container.GetBlobReferenceFromServer(fileNameBlob);
                blob.Properties.ContentType = fileData.Headers.ContentType.MediaType;

                blob.UploadFromFile(fileData.LocalFileName, FileMode.Create);
                blob.SetProperties();
                File.Delete(fileData.LocalFileName);
                //Files.Add(new FileDetails
                //{
                //    ContentType = blob.Properties.ContentType,
                //    Name = blob.Name,
                //    Size = blob.Properties.Length,
                //    Location = blob.Uri.AbsoluteUri
                //});
            }

            return base.ExecutePostProcessingAsync();
        }
    }
}