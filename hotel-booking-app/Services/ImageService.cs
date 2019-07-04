using HotelBookingApp.Data;
using HotelBookingApp.Models.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class ImageService : IImageService
    {
        private readonly string blobContainerName = "rawimages";
        private readonly string queueContainerName = "imagequeue";
        private readonly string[] validExtensions = { "jpg", "jpeg", "png" };
        private string accessKey = string.Empty;
        private CloudStorageAccount account;
        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;
        private CloudQueueClient queueClient;
        private ApplicationContext applicationContext;
        private IThumbnailService thumbnailService;

        public ImageService(IConfiguration configuration, ApplicationContext applicationContext, IThumbnailService thumbnailService)
        {
            accessKey = configuration.GetConnectionString("AzureStorageKey");
            account = CloudStorageAccount.Parse(this.accessKey);
            blobClient = account.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(blobContainerName);
            queueClient = account.CreateCloudQueueClient();
            this.applicationContext = applicationContext;
            this.thumbnailService = thumbnailService;
        }

        public async Task UploadAsync(string imageName, Stream stream)
        {
            await blobContainer.CreateIfNotExistsAsync();
            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            await blobContainer.SetPermissionsAsync(permissions);

            var cloudBlockBlob = blobContainer.GetBlockBlobReference(imageName);
            await cloudBlockBlob.UploadFromStreamAsync(stream);

            var queueReference = queueClient.GetQueueReference(queueContainerName);
            await queueReference.CreateIfNotExistsAsync();
            await queueReference.AddMessageAsync(new CloudQueueMessage(imageName));
        }

        public async Task<List<ImageDetails>> GetImageListAsync(int hotelId)
        {
            var imageList = new List<ImageDetails>();
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                await blobContainer.CreateIfNotExistsAsync();
                var results = await blobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
                blobContinuationToken = results.ContinuationToken;
                GetBlobDirectories(imageList, hotelId);
            } while (blobContinuationToken != null);
            return imageList;
        }

        public void GetBlobDirectories(List<ImageDetails> imageList, int hotelId)
        {
            foreach (IListBlobItem item in blobContainer.ListBlobs())
            {
                if (item is CloudBlobDirectory)
                {
                    GetImagesFromBlob(item, imageList, hotelId);
                }
            }
        }

        public void GetImagesFromBlob(IListBlobItem item, List<ImageDetails> imageList, int hotelId)
        {
            CloudBlobDirectory directory = (CloudBlobDirectory)item;
            IEnumerable<IListBlobItem> blobs = directory.ListBlobs(true);
            foreach (var blob in blobs)
            {
                var id = GetHotelImageFolder(blob.Uri);
                if (id == hotelId)
                {
                    imageList.Add(new ImageDetails
                    {
                        Name = blob.Uri.Segments[blob.Uri.Segments.Length - 1],
                        Path = blob.Uri.ToString()
                    });
                }
            }
        }

        public async Task<List<IFormFile>> UploadImagesAsync(List<IFormFile> files, int hotelId)
        {
            var hotel = applicationContext.Hotels.Where(h => h.HotelId == hotelId).FirstOrDefault();
            var wrongFiles = new List<IFormFile>();
            var filePath = Path.GetTempFileName();
            foreach (var file in files)
            {
                if (CheckImageExtension(file) && file.Length > 0 && hotel != null)
                {
                    await SaveImagesIntoTempFolder(filePath, file);
                    using (var stream = new FileStream(filePath, FileMode.Open))
                    {
                        if (CheckImageSize(stream))
                        {
                            var azurePath = GenerateAzurePath(hotelId, file);
                            await UploadAsync(azurePath, stream);
                            if (!hotel.Thumbnail)
                            {
                                await thumbnailService.uploadAsync(hotel, stream);
                            }
                        }
                        else
                        {
                            wrongFiles.Add(file);
                        }
                    }
                }
                else
                {
                    wrongFiles.Add(file);
                }
            };
            return wrongFiles;
        }

        public async Task DeleteFileAsync(string path)
        {
            var pathInBlob = path.Split(blobContainerName + "/")[1];
            var blob = blobContainer.GetBlockBlobReference(pathInBlob);
            await blob.DeleteAsync();
        }

        public async Task DeleteAllFileAsync(int hotelId)
        {
            var imageList = await GetImageListAsync(hotelId);
            foreach (var image in imageList)
            {
                await DeleteFileAsync(image.Path);
            }
        }

        private static int GetHotelImageFolder(Uri uri)
        {
            var pathSegments = uri.ToString().Split("/");
            var folder = pathSegments[pathSegments.Length - 2];
            return Convert.ToInt32(folder);
        }

        private static string GetExtension(IFormFile file)
        {
            if (file != null)
            {
                var fileNameSegments = file.FileName.Split(".");
                return fileNameSegments[fileNameSegments.Length - 1];
            }
            return null;
        }

        private static bool CheckImageSize(FileStream stream)
        {
            return stream.Length < 4194304;
        }

        private bool CheckImageExtension(IFormFile file)
        {
            var extensions = new List<string>(validExtensions);
            return extensions.Contains(GetExtension(file));
        }

        private static string GenerateAzurePath(int hotelId, IFormFile file)
        {
            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            return hotelId.ToString() + "/" + timeStamp + "." + GetExtension(file);
        }

        private async static Task SaveImagesIntoTempFolder(string filePath, IFormFile file)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}
