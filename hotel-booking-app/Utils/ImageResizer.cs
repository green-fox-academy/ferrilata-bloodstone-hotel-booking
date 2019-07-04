using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace HotelBookingApp.Utils
{
    public class ImageResizer
    {
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var originalBitmap = new Bitmap(image, new Size(image.Width, image.Height));
            if (image.Height < height && image.Width < width) return originalBitmap;
            using (image)
            {
                var xRatio = (double)image.Width / width;
                var yRatio = (double)image.Height / height;
                var ratio = Math.Max(xRatio, yRatio);
                var resizedX = (int)Math.Floor(image.Width / ratio);
                var resizedY = (int)Math.Floor(image.Height / ratio);
                var resizedBitmap = new Bitmap(resizedX, resizedY, PixelFormat.Format32bppArgb);
                using (var graphics = Graphics.FromImage(resizedBitmap))
                {
                    graphics.Clear(Color.Transparent);

                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    graphics.DrawImage(image,
                        new Rectangle(0, 0, resizedX, resizedY),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);
                }
                return resizedBitmap;
            }
        }
    }
}

