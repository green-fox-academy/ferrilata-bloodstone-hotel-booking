using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Hosting;

namespace HotelBookingApp.Services
{
    public class ThumbnailService : IThumbnailService
    {
        private readonly string blobContainerName = "thumbnails";
        private string accessKey = string.Empty;
        private CloudStorageAccount account;
        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;
        private ApplicationContext applicationContext;

        public ThumbnailService(IConfiguration configuration, ApplicationContext applicationContext, IHostingEnvironment env)
        {
            accessKey = configuration.GetConnectionString(AzureConnectionStringProvider.GetAzureConnectionString(env));
            account = CloudStorageAccount.Parse(this.accessKey);
            blobClient = account.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(blobContainerName);
            this.applicationContext = applicationContext;
        }
        public async Task UploadAsync(Hotel hotel, Stream stream)
        {
            var size = 200;
            var fileName = hotel.HotelId.ToString() + ".jpg";
            stream.Seek(0, SeekOrigin.Begin);
            var bitmap = new Bitmap(System.Drawing.Image.FromStream(stream));
            Bitmap resizedBitmap = ImageResizer.ResizeImage(bitmap, size, size);
            var tempPath = Path.GetTempPath();
            var absolutePath = tempPath + "/" + hotel.HotelId.ToString();
            resizedBitmap.Save(absolutePath);
            await blobContainer.CreateIfNotExistsAsync();
            await SetBlobPermissionToPublic();
            var cloudBlockBlob = blobContainer.GetBlockBlobReference(fileName);

            using (var thumbnailStream = new FileStream(absolutePath, FileMode.Open))
            {
                var imageUrlOnAzure = cloudBlockBlob.Uri.ToString();
                await cloudBlockBlob.UploadFromStreamAsync(thumbnailStream);
                hotel.Thumbnail = true;
                hotel.ThumbnailUrl = imageUrlOnAzure;
                applicationContext.Hotels.Update(hotel);
                await applicationContext.SaveChangesAsync();
            }
        }

         public async Task SetBlobPermissionToPublic()
        {
            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            await blobContainer.SetPermissionsAsync(permissions);
        }

        public async Task DeleteAsync(int hotelId)
        {
            var fileName = hotelId.ToString() + ".jpg";
            var blob = blobContainer.GetBlockBlobReference(fileName);
            await blob.DeleteAsync();
        }

        public async Task UpdateThumbnail(int hotelId, IFormFile file)
        {
            await UploadAsync(applicationContext.Hotels.Where(h => h.HotelId == hotelId).First(), file.OpenReadStream());
        }

        public async Task UpdateThumbnailFromUrl(int hotelId, string url)
        {
            var webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData(url);
            Stream stream = new MemoryStream(imageBytes);
            await UploadAsync(applicationContext.Hotels.Where(h => h.HotelId == hotelId).First(), stream);
        }
    }
}
