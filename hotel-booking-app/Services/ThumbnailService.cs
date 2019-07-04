using HotelBookingApp.Data;
using HotelBookingApp.Models.Hotel;
using HotelBookingApp.Utils;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

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

        public ThumbnailService(IConfiguration configuration, ApplicationContext applicationContext)
        {
            accessKey = configuration.GetConnectionString("AzureStorageKey");
            account = CloudStorageAccount.Parse(this.accessKey);
            blobClient = account.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(blobContainerName);
            this.applicationContext = applicationContext;
        }
        public async Task uploadAsync(Hotel hotel, Stream stream)
        {
            var size = 200;
            var fileName = hotel.HotelId.ToString() + ".jpg";
            stream.Seek(0, SeekOrigin.Begin);
            var bitmap = new Bitmap(System.Drawing.Image.FromStream(stream));
            Bitmap resizedBitmap = ImageResizer.ResizeImage(bitmap, size, size);
            resizedBitmap.Save(hotel.HotelId.ToString());
            await blobContainer.CreateIfNotExistsAsync();
            await SetBlobPermissionToPublic();
            var cloudBlockBlob = blobContainer.GetBlockBlobReference(fileName);

            using (var thumbnailStream = new FileStream(hotel.HotelId.ToString(), FileMode.Open))
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
    }
}
