using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Moses_Bugtracker.Helpers
{
    public static class FileUploadValidator
    {
      
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null)
                return false;

            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
                return false;

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                            ImageFormat.Png.Equals(img.RawFormat) ||
                            ImageFormat.Gif.Equals(img.RawFormat) ||
                            ImageFormat.Bmp.Equals(img.RawFormat) ||
                            ImageFormat.Tiff.Equals(img.RawFormat);
                }
            }

            catch
            {
                return false;
            }


        }

        public static bool IsWebFriendlyFile(HttpPostedFileBase file)
        {
            if (file == null)
                return false;

            if (file.ContentLength > 3 * 1024 * 1024)
                return false;

            var fileExt = Path.GetExtension(file.FileName);
            var validExt = (/*fileExt == ".pdf" ||*/
                           fileExt == ".doc" ||
                           fileExt == ".docx" ||
                           fileExt == ".txt" ||
                           fileExt == ".xls" ||
                           fileExt == ".xlsx");

            return validExt || IsWebFriendlyImage(file);
        }
    }
}
