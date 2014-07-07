using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.MediaServices.Client;
using System.IO;
using System.Configuration;

namespace AzureMediaLib
{
    public class MediaService
    {
        private static readonly string _mediaFiles = Path.GetFullPath(ConfigurationManager.AppSettings["SupportFolder"]); //the folder to fetch medial contents
        private static MediaServicesCredentials _cachedCredentials = null;
        private static CloudMediaContext _context = null;

        // Media Services account information.
        private static readonly string _mediaServicesAccountName = ConfigurationManager.AppSettings["accountName"];
        private static readonly string _mediaServicesAccountKey = ConfigurationManager.AppSettings["accountKey"];
        private static readonly string _singleMP4File = Path.Combine(_mediaFiles, @"walkon.mp4"); //sample vedio file

        static MediaService()
        {
            //init contexts.
            _cachedCredentials = new MediaServicesCredentials(_mediaServicesAccountName, _mediaServicesAccountKey);
            _context = new CloudMediaContext(_cachedCredentials);
        }

        #region Upload files

        /// <summary>
        /// Can be replaced by CreateFromFile() from Media extension lib!
        /// </summary>
        /// <param name="uploadFilePath"></param>
        /// <returns></returns>
        public static string uploadMedia(string uploadFilePath)
        {
            //to-do: validations
            var uploadAsset = _context.Assets.Create(Path.GetFileNameWithoutExtension(uploadFilePath) + DateTime.UtcNow.ToString(), AssetCreationOptions.None);
            var assetFile = uploadAsset.AssetFiles.Create(Path.GetFileName(uploadFilePath));
            assetFile.Upload(uploadFilePath);

            return uploadAsset.Id;
        }

        public static Dictionary<string, string> uploadMedias(List<string> uploadFilePathList)
        {
            var resultAssetIds = new Dictionary<string, string>();
            foreach (var uploadFilePath in uploadFilePathList)
            {
                var assetID = uploadMedia(uploadFilePath);
                resultAssetIds.Add(uploadFilePath, assetID);
            }

            return resultAssetIds;
        }



        #endregion


        #region Transcode and Upload + Transcode

        /// <summary>
        /// Note: Assets should only contain either a single file or if they are multi-file, then they are typically
        /// a set of files with a manifest like Smooth Streaming format or Apple HLS format.  
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="durationOfPublish"></param>
        /// <param name="funcUpload"></param>
        /// <param name="funcJobExe"></param>
        public static void uploadFolderAndTranscode(string folderPath, int durationOfPublish,
           Action<IAssetFile, UploadProgressChangedEventArgs> funcUpload = null,
           Action<IJob> funcJobExe = null)
        {

            // Create a new asset and upload a mezzanine file from a local path. 
            IAsset inputAsset = _context.Assets.CreateFromFolder(
                folderPath,
                AssetCreationOptions.None,
                funcUpload
                //(af, p) =>
                //{
                //    Console.WriteLine("Uploading '{0}' - Progress: {1:0.##}%", af.Name, p.Progress);
                //}
                );

            // Prepare a job with a single task to transcode the previous mezzanine asset into a multi-bitrate asset. 
            IJob job = _context.Jobs.CreateWithSingleTask(MediaProcessorNames.WindowsAzureMediaEncoder, MediaEncoderTaskPresetStrings.H264AdaptiveBitrateMP4Set720p,
                inputAsset, "Adaptive Bitrate MP4 for" + Path.GetDirectoryName(folderPath), AssetCreationOptions.None);

            job.Submit();   // Submit the job and wait until it is completed. 
            job = job.StartExecutionProgressTask(
                funcJobExe,
                //j =>
                //{
                //    Console.WriteLine("Job state: {0}", j.State);
                //    Console.WriteLine("Job progress: {0:0.##}%", j.GetOverallProgress());
                //},
                CancellationToken.None).Result;

            // The OutputMediaAssets[0] contains the first (and in this case the only) output asset produced by the encoding job.
            //IAsset outputAsset = job.OutputMediaAssets[0];
            Console.WriteLine("Asset count = " + job.OutputMediaAssets.Count());
            string outputFolder = Path.Combine(_mediaFiles, @"job-output");

            foreach (var outputAsset in job.OutputMediaAssets)
            {
                outputFolder += outputAsset.Name;
                // Publish the output asset by creating an Origin locator.  Define the Read only access policy and specify that the asset can be accessed for 30 days.  
                _context.Locators.Create(LocatorType.OnDemandOrigin, outputAsset,
                    AccessPermissions.Read, TimeSpan.FromDays(durationOfPublish));

                // Generate the Smooth Streaming, HLS and MPEG-DASH URLs for adaptive streaming.  
                Uri smoothStreamingUri = outputAsset.GetSmoothStreamingUri(); //for MS XBOX etc.
                Uri hlsUri = outputAsset.GetHlsUri(); //For Apple devices
                Uri mpegDashUri = outputAsset.GetMpegDashUri();  //The ISO standard

                // To stream your content based on the set of adaptive bitrate MP4 files, you must first get 
                // at least one On-demand Streaming reserved unit. 
                // For more information, see http://msdn.microsoft.com/en-us/library/jj889436.aspx.
                Console.WriteLine("Output is now available for adaptive streaming:");
                // Show the streaming URLs. 
                Console.WriteLine(smoothStreamingUri);
                Console.WriteLine(hlsUri);
                Console.WriteLine(mpegDashUri);

                //
                // Create a SAS locator that is used for progressive download or to download files to a local directory.  
                // The content that you want to progressively download cannot be encrypted.
                _context.Locators.Create(
                    LocatorType.Sas,
                    outputAsset,
                    AccessPermissions.Read,
                    TimeSpan.FromDays(30));

                // Get all the MP4 files in the output asset.
                IEnumerable<IAssetFile> mp4AssetFiles = outputAsset
                        .AssetFiles
                        .ToList()
                        .Where(af => af.Name.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase));

                // Generate the Progressive Download URLs for each MP4. 
                List<Uri> mp4ProgressiveDownloadUris = mp4AssetFiles.Select(af => af.GetSasUri()).ToList();

                Console.WriteLine("You can progressively download the following MP4 files:");
                mp4ProgressiveDownloadUris.ForEach(uri => Console.WriteLine(uri));

                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                Console.WriteLine("Downloading output asset files to local folder...");

                // Download the output asset to a local folder. 
                outputAsset.DownloadToFolder(
                    outputFolder,
                    (af, p) =>
                    {
                        Console.WriteLine("Downloading '{0}' - Progress: {1:0.##}%", af.Name, p.Progress);
                    });

            }

            Console.WriteLine("Downloaded files are located in '{0}'.", Path.GetFullPath(outputFolder));
        }

        /// <summary>
        /// Transcode the mezzanine asset and upload
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="durationOfPublish"></param>
        /// <param name="funcUpload"></param>
        /// <param name="funcJobExe"></param>
        public static void uploadAndTranscode(string srcPath, int durationOfPublish,
            Action<IAssetFile, UploadProgressChangedEventArgs> funcUpload = null,
            Action<IJob> funcJobExe = null)
        {

            // Create a new asset and upload a mezzanine file from a local path. 
            IAsset inputAsset = _context.Assets.CreateFromFile(
                srcPath,
                AssetCreationOptions.None,
                funcUpload
                //(af, p) =>
                //{
                //    Console.WriteLine("Uploading '{0}' - Progress: {1:0.##}%", af.Name, p.Progress);
                //}
                );


            // Prepare a job with a single task to transcode the previous mezzanine asset into a multi-bitrate asset. 
            IJob job = _context.Jobs.CreateWithSingleTask(MediaProcessorNames.WindowsAzureMediaEncoder, MediaEncoderTaskPresetStrings.H264AdaptiveBitrateMP4Set720p,
                inputAsset, "Adaptive Bitrate MP4 for" + Path.GetFileName(srcPath), AssetCreationOptions.None);

            job.Submit();   // Submit the job and wait until it is completed. 
            job = job.StartExecutionProgressTask(
                funcJobExe,
                //j =>
                //{
                //    Console.WriteLine("Job state: {0}", j.State);
                //    Console.WriteLine("Job progress: {0:0.##}%", j.GetOverallProgress());
                //},
                CancellationToken.None).Result;

            // The OutputMediaAssets[0] contains the first (and in this case the only) output asset produced by the encoding job.
            IAsset outputAsset = job.OutputMediaAssets[0];

            // Publish the output asset by creating an Origin locator.  Define the Read only access policy and specify that the asset can be accessed for 30 days.  
            _context.Locators.Create(LocatorType.OnDemandOrigin, outputAsset,
                AccessPermissions.Read, TimeSpan.FromDays(durationOfPublish));

            // Generate the Smooth Streaming, HLS and MPEG-DASH URLs for adaptive streaming.  
            Uri smoothStreamingUri = outputAsset.GetSmoothStreamingUri(); //for MS XBOX etc.
            Uri hlsUri = outputAsset.GetHlsUri(); //For Apple devices
            Uri mpegDashUri = outputAsset.GetMpegDashUri();  //The ISO standard

            // To stream your content based on the set of adaptive bitrate MP4 files, you must first get 
            // at least one On-demand Streaming reserved unit. 
            // For more information, see http://msdn.microsoft.com/en-us/library/jj889436.aspx.
            Console.WriteLine("Output is now available for adaptive streaming:");
            // Show the streaming URLs. 
            Console.WriteLine(smoothStreamingUri);
            Console.WriteLine(hlsUri);
            Console.WriteLine(mpegDashUri);

            //
            // Create a SAS locator that is used for progressive download or to download files to a local directory.  
            // The content that you want to progressively download cannot be encrypted.
            _context.Locators.Create(
                LocatorType.Sas,
                outputAsset,
                AccessPermissions.Read,
                TimeSpan.FromDays(30));

            // Get all the MP4 files in the output asset.
            IEnumerable<IAssetFile> mp4AssetFiles = outputAsset
                    .AssetFiles
                    .ToList()
                    .Where(af => af.Name.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase));

            // Generate the Progressive Download URLs for each MP4. 
            List<Uri> mp4ProgressiveDownloadUris = mp4AssetFiles.Select(af => af.GetSasUri()).ToList();

            Console.WriteLine("You can progressively download the following MP4 files:");
            mp4ProgressiveDownloadUris.ForEach(uri => Console.WriteLine(uri));

            string outputFolder = Path.Combine(_mediaFiles, @"job-output");

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            Console.WriteLine("Downloading output asset files to local folder...");

            // Download the output asset to a local folder. 
            outputAsset.DownloadToFolder(
                outputFolder,
                (af, p) =>
                {
                    Console.WriteLine("Downloading '{0}' - Progress: {1:0.##}%", af.Name, p.Progress);
                });

            Console.WriteLine("Downloaded files are located in '{0}'.", Path.GetFullPath(outputFolder));
        }

        #endregion

        /// <summary>
        /// Can be replaced by DownloadToFolder() in the medial extension lib!
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="outputFolder"></param>
        public static void DownloadAssetsToLocal(string jobId, string outputFolder, Action<Object, DownloadProgressChangedEventArgs> downloadProgressFunc = null)
        {
            // iterate through the OutputAssets collection, and download all assets if there are many. 
            // Get a reference to the job. 
            IJob job = GetJob(jobId);
            var downloadTasks = new List<Task>();

            // Get a reference to the first output asset.   // IAsset outputAsset = job.OutputMediaAssets[0];
            foreach (var outputAsset in job.OutputMediaAssets)
            {
                IAccessPolicy accessPolicy = _context.AccessPolicies.Create("File Download Policy", TimeSpan.FromDays(30), AccessPermissions.Read);
                ILocator locator = _context.Locators.CreateSasLocator(outputAsset, accessPolicy);
                BlobTransferClient blobTransfer = new BlobTransferClient
                {
                    NumberOfConcurrentTransfers = 10,
                    ParallelTransferThreadCount = 10
                };

                //downloadProgressFunc = downloadProgressFunc == null ? DownloadProgress : new EventHandler<Object,DownloadProgressChangedEventArgs>(new downloadProgressFunc; //figure out the right delegate
                foreach (IAssetFile outputFile in outputAsset.AssetFiles)
                {
                    // Use the following event handler to check download progress.
                    outputFile.DownloadProgressChanged += DownloadProgress;
                    string localDownloadPath = Path.Combine(outputFolder, outputFile.Name);
                    downloadTasks.Add(outputFile.DownloadAsync(Path.GetFullPath(localDownloadPath), blobTransfer, locator, CancellationToken.None));
                    outputFile.DownloadProgressChanged -= DownloadProgress;
                }
            }
            Task.WaitAll(downloadTasks.ToArray());
        }

        public static List<Uri> GetProgressiveDownloadURLs(string jobId, string fileExtensionFilter = ".mp4")
        {
            IJob job = GetJob(jobId);

            var downloadURLs = job.OutputMediaAssets.SelectMany(a => a.AssetFiles).Distinct()
                .Where(af => af.Name.EndsWith(fileExtensionFilter, StringComparison.OrdinalIgnoreCase)).Select(af => af.GetSasUri()).ToList();

            //var t1 = GetAdaptiveStreamingURLs(job.OutputMediaAssets[0]); //for test
            return downloadURLs;
        }

        /// <summary>
        /// Refer to: http://msdn.microsoft.com/en-us/library/jj889436.aspx
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static List<Uri> GetAdaptiveStreamingURLs(IAsset asset)
        {
            // Generate the Smooth Streaming, HLS and MPEG-DASH URLs for adaptive streaming.  
            Uri smoothStreamingUri = asset.GetSmoothStreamingUri(); //for MS XBOX etc.
            Uri hlsUri = asset.GetHlsUri(); //For Apple devices
            Uri mpegDashUri = asset.GetMpegDashUri();  //The ISO standard

            var result = new List<Uri>();
            result.Add(smoothStreamingUri);
            result.Add(hlsUri);
            result.Add(mpegDashUri);
            return result;
        }

        #region Helper

        protected static IJob GetJob(string jobId)
        {
            // Use a Linq select query to get an updated 
            // reference by Id. 
            var jobInstance =
                from j in _context.Jobs
                where j.Id == jobId
                select j;
            // Return the job reference as an Ijob. 
            IJob job = jobInstance.FirstOrDefault();
            return job;
        }

        protected static void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine(string.Format("{0} % download progress. ", e.Progress));
        }

        #endregion
    }
}
