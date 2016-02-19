using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHash
{
   

    public static class DHashAlg
    {
        public static Int64 Do(string path)
        {
            Int64 res = 0;
            Bitmap bitmap = null;
            using (var loadBitmap = new Bitmap(path))
            {
                bitmap = Util.ResizeImage(loadBitmap, 9, 8);
            }
            int i = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Color c1 = bitmap.GetPixel(x, y);
                    byte a1 = (byte)((c1.R + c1.G + c1.B) / 3f);
                    Color c2 = bitmap.GetPixel(x + 1, y);
                    byte a2 = (byte)((c2.R + c2.G + c2.B) / 3f);
                    if (a1 > a2)
                    {
                        res |= ((Int64)1 << i);
                    }
                    i++;
                }
            }
            return res;
        }

    }

    public static class AHashAlg
    {
        public static Int64 Do(string path)
        {
            Int64 res = 0;
            Bitmap bitmap = null;
            using (var loadBitmap = new Bitmap(path))
            {
                bitmap = Util.ResizeImage(loadBitmap, 10, 10);
            }
            for (int x = 1; x < 8; x++)
            {
                for (int y = 1; y < 8; y++)
                {
                    byte s1 = GetSat(bitmap.GetPixel(x - 1, y));
                    byte s2 = GetSat(bitmap.GetPixel(x - 1, y - 1));
                    byte s3 = GetSat(bitmap.GetPixel(x, y - 1));
                    byte s4 = GetSat(bitmap.GetPixel(x + 1, y));
                    byte s5 = GetSat(bitmap.GetPixel(x + 1, y - 1));
                    byte sa = (byte)((s1 + s2 + s3 + s4 + s5) / 5f);


                    if (GetSat(bitmap.GetPixel(x, y)) > sa)
                    {
                        res |= ((Int64)1 << ((y - 1) * 8 + (x - 1)));
                    }
                }
            }
            return res;
        }
        public static byte GetSat(Color c)
        {
            return (byte)((c.R + c.G + c.B) / 3f);
        }
    }

    public static class WHashAlg
    {
        public static Int64 Do(string path)
        {
            Int64 res = 0;
            Bitmap bitmap = null;
            using (var loadBitmap = new Bitmap(path))
            {
                bitmap = Util.ResizeImage(loadBitmap, 16, 16);
            }
            int i = 0;
            for (int y = 0; y < 16; y += 2)
            {
                for (int x = 0; x < 16; x += 2)
                {
                    byte max = 0;
                    Int64 v = 0;
                    for (int y1 = 0; y1 < 2; y1++)
                    {
                        for (int x1 = 0; x1 < 2; x1++)
                        {
                            Color c = bitmap.GetPixel(x + x1, y + y1);
                            byte a = (byte)((c.R + c.G + c.B) / 3f);
                            if (a > max)
                            {
                                max = a;
                                v = (Int64)x1 | ((Int64)y1 << 1);
                            }
                        }
                    }
                    res |= v << i;
                    i += 2;
                }
            }
            return res;
        }
    }
    public static class Util
    {

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, new ImageAttributes());
                }
            }

            return destImage;
        }
        public static int HammingDistance(Int64 a, Int64 b)
        {
            int res = 0;
            Int64 val = a ^ b;
            while (val != 0)
            {
                res++;
                val &= val - 1;
            }
            return res;
        }
    }
}
