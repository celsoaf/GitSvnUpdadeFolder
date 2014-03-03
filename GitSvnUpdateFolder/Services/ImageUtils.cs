using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows;
using System.Drawing;

namespace GitSvnUpdateFolder.Services
{
    public class ImageUtils
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static BitmapSource ConvertByteArrayToImageSource(byte[] image)
        {
            if (image == null)
                return null;

            BitmapImage img = new BitmapImage();
            using (MemoryStream ms = new MemoryStream(image))
            {
                try
                {
                    img.BeginInit();
                    img.StreamSource = ms;
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    ms.Flush();
                    ms.Close();
                    img.StreamSource.Dispose();
                }
                catch (Exception ex)
                {
                    try { ms.Close(); }
                    catch { }
                    try { img.StreamSource.Dispose(); }
                    catch
                    { }
                    throw ex;
                }
            }
            return img;
        }

        public static ImageSource ConvertUriToImageSource(string uriString)
        {
            // Create source.
            var bi = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            bi.BeginInit();
            bi.UriSource = new Uri(uriString, UriKind.RelativeOrAbsolute);
            bi.EndInit();

            return bi;
        }

        public static ImageSource ConvertBitmapToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();

            System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(hBitmap);

            return bitmapSource;
        }


        public static ImageSource ConvertImageToImageSource(System.Drawing.Image bitmap)
        {
            IntPtr hBitmap = new Bitmap(bitmap).GetHbitmap();

            System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(hBitmap);

            return bitmapSource;
        }

        public static byte[] ConvertImageSourceToByteArray(ImageSource image)
        {
            BitmapSource bs = image as BitmapSource;
            if (bs != null)
            {
                byte[] aux;
                using (MemoryStream ms = new MemoryStream())
                {
                    BitmapEncoder encoder = new JpegBitmapEncoder();
                    //BitmapEncoder encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bs));
                    encoder.Save(ms);
                    aux = ms.ToArray();
                }
                return aux;
            }
            return null;
        }

        /// <summary>
        /// Creates a new ImageSource with the specified width/height
        /// </summary>
        /// <param name="source">Source image to resize</param>
        /// <param name="width">Width of resized image</param>
        /// <param name="height">Height of resized image</param>
        /// <returns>Resized image</returns>
        public static ImageSource CreateResizedImage(ImageSource source, int width, int height)
        {
            // Target Rect for the resize operation
            Rect rect = new Rect(0, 0, width, height);

            // Create a DrawingVisual/Context to render with
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(source, rect);
            }

            // Use RenderTargetBitmap to resize the original image
            RenderTargetBitmap resizedImage = new RenderTargetBitmap(
                (int)rect.Width, (int)rect.Height,  // Resized dimensions
                96, 96,                             // Default DPI values
                PixelFormats.Default);              // Default pixel format
            resizedImage.Render(drawingVisual);

            // Return the resized image
            return resizedImage;
        }

        public static ImageSource CreateResizedImageUsingMaxWidth(ImageSource source, int max)
        {
            int width = max;
            int height = max;
            if (source.Width <= max && source.Height <= max)
            {
                return source;
            }
            else
            {
                if (source.Width >= source.Height)
                {
                    double rel = source.Width / max;
                    width = max;
                    height = (int)(source.Height / rel);
                }
                else
                {
                    double rel = source.Height / max;
                    height = max;
                    width = (int)(source.Width / rel);
                }
            }
            System.Diagnostics.Debug.WriteLine("will convert image from {0}x{1} to {2}x{3}",
                source.Width, source.Height, width, height);
            // Target Rect for the resize operation
            Rect rect = new Rect(0, 0, width, height);

            // Create a DrawingVisual/Context to render with
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(source, rect);
            }

            // Use RenderTargetBitmap to resize the original image
            RenderTargetBitmap resizedImage = new RenderTargetBitmap(
                (int)rect.Width, (int)rect.Height,  // Resized dimensions
                96, 96,                             // Default DPI values
                PixelFormats.Default);              // Default pixel format
            resizedImage.Render(drawingVisual);

            // Return the resized image
            return resizedImage;
        }

        public static ImageSource CropImage3x3(ImageSource source)
        {
            if (source.Width >= source.Height)
            {
                int targuetWidth = (int)source.Height;
                CroppedBitmap cb = new CroppedBitmap((BitmapSource)source,
                    new Int32Rect((int)(source.Width / 2 - targuetWidth / 2), 0,
                        targuetWidth, (int)source.Height));
                return cb;
            }
            else
            {
                int targuetHeight = (int)source.Width;
                CroppedBitmap cb = new CroppedBitmap((BitmapSource)source,
                    new Int32Rect(0, (int)(source.Height / 2 - targuetHeight / 2),
                        (int)source.Width, targuetHeight));
                return cb;
            }

        }

    }
}
