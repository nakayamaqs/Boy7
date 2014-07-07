using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MediaServices.Client;
using Microsoft.WindowsAzure;

namespace AzureMediaConsole
{
    class Program
    {

        private static readonly string _mediaFiles = Path.GetFullPath(ConfigurationManager.AppSettings["SupportFolder"]);
        private static readonly string _singleMP4File = Path.Combine(_mediaFiles, @"walkon.mp4");

        private static MediaServicesCredentials _cachedCredentials = null;
        private static CloudMediaContext _context = null;

        // Media Services account information.
        private static readonly string _mediaServicesAccountName = ConfigurationManager.AppSettings["accountName"];
        private static readonly string _mediaServicesAccountKey = ConfigurationManager.AppSettings["accountKey"];

        static void Main(string[] args)
        {
            //var resultsURLs = AzureMediaLib.MediaService.GetProgressiveDownloadURLs("nb:jid:UUID:3880ae6a-e5a4-6642-a786-40437555c8c8");

            AzureMediaLib.MediaService.uploadFolderAndTranscode(_mediaFiles, 30);

            //string configuration2 = File.ReadAllText(Path.GetFullPath(@"../..\configs\Thumbnails.xml"));
            _cachedCredentials = new MediaServicesCredentials(_mediaServicesAccountName, _mediaServicesAccountKey);
            _context = new CloudMediaContext(_cachedCredentials);

            //DownloadAssetForTest();
            Console.WriteLine("finish download test.");
            Console.ReadLine();

            Console.WriteLine("Creating new asset from local file...");

            // Create a new asset and upload a mezzanine file from a local path. 
            IAsset inputAsset = _context.Assets.CreateFromFile(
                _singleMP4File,
                AssetCreationOptions.None,
                (af, p) =>
                {
                    Console.WriteLine("Uploading '{0}' - Progress: {1:0.##}%", af.Name, p.Progress);
                });

            Console.WriteLine("Asset created.");

            // Prepare a job with a single task to transcode the previous mezzanine asset 
            // into a multi-bitrate asset. 
            //IJob job = _context.Jobs.CreateWithSingleTask(
            //    MediaProcessorNames.WindowsAzureMediaEncoder,
            //    MediaEncoderTaskPresetStrings.H264AdaptiveBitrateMP4Set720p,
            //    inputAsset,
            //    "Sample Adaptive Bitrate MP4",
            //    AssetCreationOptions.None);

            //create thumbnail
            string configuration = File.ReadAllText(Path.GetFullPath(@"../..\configs\Thumbnails.xml"));

            var job = _context.Jobs.CreateWithSingleTask(
                MediaProcessorNames.WindowsAzureMediaEncoder,
                MediaEncoderTaskPresetStrings.Thumbnails,
                inputAsset,
                "Sample thumbnail 1",
                AssetCreationOptions.None);


            Console.WriteLine("Submitting transcoding job...");

            // Submit the job and wait until it is completed. 
            job.Submit();
            job = job.StartExecutionProgressTask(
                j =>
                {
                    Console.WriteLine("Job state: {0}", j.State);
                    Console.WriteLine("Job progress: {0:0.##}%", j.GetOverallProgress());
                },
                CancellationToken.None).Result;

            Console.WriteLine("Transcoding job finished.");

            // The OutputMediaAssets[0] contains the first 
            // (and in this case the only) output asset 
            // produced by the encoding job.
            IAsset outputAsset = job.OutputMediaAssets[0];

            Console.WriteLine("Publishing output asset...");

            // Publish the output asset by creating an Origin locator.  
            // Define the Read only access policy and
            // specify that the asset can be accessed for 30 days.  
            _context.Locators.Create(
                LocatorType.OnDemandOrigin,
                outputAsset,
                AccessPermissions.Read,
                TimeSpan.FromDays(30));

            // Generate the Smooth Streaming, HLS and MPEG-DASH URLs for adaptive streaming.  
            Uri smoothStreamingUri = outputAsset.GetSmoothStreamingUri();
            Uri hlsUri = outputAsset.GetHlsUri();
            Uri mpegDashUri = outputAsset.GetMpegDashUri();

            // To stream your content based on the set of 
            // adaptive bitrate MP4 files, you must first get 
            // at least one On-demand Streaming reserved unit. 
            // For more information, see http://msdn.microsoft.com/en-us/library/jj889436.aspx.
            Console.WriteLine("Output is now available for adaptive streaming:");

            // Show the streaming URLs. 
            Console.WriteLine(smoothStreamingUri);
            Console.WriteLine(hlsUri);
            Console.WriteLine(mpegDashUri);

            //
            // Create a SAS locator that is used for progressive download
            // or to download files to a local directory.  
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
            List<Uri> mp4ProgressiveDownloadUris =
                mp4AssetFiles.Select(af => af.GetSasUri()).ToList();

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

        static void DownloadAssetForTest()
        {
            string outputFolder = Path.Combine(_mediaFiles, @"job-output-test");
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            DownloadAssetToLocal("nb:jid:UUID:3880ae6a-e5a4-6642-a786-40437555c8c8", outputFolder);
        }

        static void DownloadAssetToLocal(string jobId, string outputFolder)
        {
            // This method illustrates how to download a single asset. 
            // However, you can iterate through the OutputAssets
            // collection, and download all assets if there are many. 

            // Get a reference to the job. 
            IJob job = GetJob(jobId);
            var downloadTasks = new List<Task>();

            // Get a reference to the first output asset. If there were multiple 
            // output media assets you could iterate and handle each one.
           // IAsset outputAsset = job.OutputMediaAssets[0];
 
            foreach (var outputAsset in job.OutputMediaAssets)
            {
                IAccessPolicy accessPolicy = _context.AccessPolicies.Create("File Download Policy", TimeSpan.FromDays(30), AccessPermissions.Read);
                ILocator locator = _context.Locators.CreateSasLocator(outputAsset, accessPolicy);
                BlobTransferClient blobTransfer = new BlobTransferClient
                {
                    NumberOfConcurrentTransfers = 10,
                    ParallelTransferThreadCount = 10
                };

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

        static IJob GetJob(string jobId)
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

        static void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine(string.Format("{0} % download progress. ", e.Progress));
        }
    }
}
