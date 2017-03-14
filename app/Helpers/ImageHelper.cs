using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace MyStarwarsApi.Helpers
{
    public static class ImageHelper
    {

        public static Bitmap bitmapFromBase64String(String imageString) 
        {
            Byte[] bitmapData = Convert.FromBase64String(_fixBase64ForImage(imageString));
            MemoryStream streamBitmap = new MemoryStream(bitmapData);
            return new Bitmap((Bitmap)System.Drawing.Image.FromStream(streamBitmap));
        }

        private static String _fixBase64ForImage(String imageString)
        {
            StringBuilder sb = new StringBuilder(imageString, imageString.Length);
            sb.Replace("\r\n", String.Empty);
            sb.Replace(" ",String.Empty);
            return sb.ToString();
        }

        public static ImageFormat getImageFormatFromBase64String(String imageString)
        {
            String  data = imageString.Substring(0,5);
            ImageFormat format = null;

            switch(data.ToUpper())
            {
                case "IVBOR":
                    format = ImageFormat.Png;
                    break;
                case "/9J/4":
                    format = ImageFormat.Jpeg;
                    break;
            }
            return format;
        }

        public static ImageFormat getImageFormatFromString(String format)
        {
            ImageFormat ifor = null;
            switch(format)
            {
                case "Png":
                    ifor = ImageFormat.Png;
                    break;
                case "Jpeg":
                    ifor = ImageFormat.Jpeg;
                    break;
            }
            return ifor;
        }
    }
}