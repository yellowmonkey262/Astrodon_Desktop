#region Usings

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

#endregion

namespace Astro.Library
{
    public class ImageUtils
    {
        public static byte[] ResizeToMaxSize(int maxWidth, int maxHeight, byte[] imagedata)
        {
            //always call resize even if size the same to avoid locs and to create jpgs from any format
            using (var img = LoadFromByteArray(imagedata))
            {
                var size = new Size();
                size.Width = img.Width;
                size.Height = img.Height;
                if (img.Width > maxWidth || img.Height > maxHeight)
                {
                    if (maxWidth > maxHeight)
                    {
                        size.Width = maxWidth;
                        size.Height = maxWidth;
                    }
                    else
                    {
                        size.Width = maxHeight;
                        size.Height = maxHeight;
                    }
                }
                using (var resized = ResizeImage(img, size))
                {
                    return ImageToByteArray(resized);
                }
            }
        }

        private static byte[] ImageToByteArray(Image myImage)
        {
            using (var b = new Bitmap(myImage.Width, myImage.Height))
            {
                b.SetResolution(myImage.HorizontalResolution, myImage.VerticalResolution);

                using (var g = Graphics.FromImage(b))
                {
                    g.Clear(Color.White);
                    g.DrawImageUnscaled(myImage, 0, 0);
                }

                // Now save b as a JPEG like you normally would

                using (var memStream = new MemoryStream())
                {
                    b.Save(memStream, ImageFormat.Jpeg);
                    var result = new byte[memStream.Length];
                    memStream.Position = 0;
                    memStream.Read(result, 0, result.Length);
                    return result;
                }
            }
        }

        public static Image LoadFromByteArray(byte[] jpg)
        {
            Image result;
            using (var memStream = new MemoryStream(jpg))
            {
                result = Image.FromStream(memStream);
            }
            return result;
        }

        private static Image ResizeImage(Image image, Size size)
        {
            int newWidth;
            int newHeight;
            var originalWidth = image.Width;
            var originalHeight = image.Height;
            var percentWidth = size.Width/(float) originalWidth;
            var percentHeight = size.Height/(float) originalHeight;
            var percent = percentHeight < percentWidth ? percentHeight : percentWidth;
            newWidth = (int) (originalWidth*percent);
            newHeight = (int) (originalHeight*percent);

            Image newImage = new Bitmap(newWidth, newHeight);
            using (var graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.Clear(Color.White);
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        public static Image ResizeToMaxSize(int maxWidth, int maxHeight, Image img)
        {
            var size = new Size
            {
                Width = img.Width,
                Height = img.Height
            };
            if (img.Width > maxWidth || img.Height > maxHeight)
            {
                if (maxWidth > maxHeight)
                {
                    size.Width = maxWidth;
                    size.Height = maxWidth;
                }
                else
                {
                    size.Width = maxHeight;
                    size.Height = maxHeight;
                }
            }
            else
            {
                if (img.Width < maxWidth || img.Height < maxHeight)
                {
                    if (maxWidth > maxHeight)
                    {
                        size.Width = maxWidth;
                        size.Height = maxWidth;
                    }
                    else
                    {
                        size.Width = maxHeight;
                        size.Height = maxHeight;
                    }
                }
            }
            return ResizeImage(img, size);
        }

        public static byte[] MaxSizeImage(byte[] jpg, int maxWidth, int maxHeight)
        {
            using (var img = LoadFromByteArray(jpg))
            {
                using (var resized = ResizeToMaxSize(maxWidth, maxHeight, img))
                {
                    return ImageToByteArray(resized);
                }
            }
        }

        /// <summary>
        ///     Will create a square of size x size and proportionally center the image in that square
        /// </summary>
        /// <param name="jpg">The JPG.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static byte[] CenterSquareImage(byte[] jpg, int size)
        {
            using (var img = LoadFromByteArray(jpg))
            {
                using (var resized = ResizeToMaxSize(size, size, img))
                {
                    if (resized.Width == size && resized.Height == size)
                    {
                        return ImageToByteArray(resized);
                    }

                    //create a blank canvas and center the image on that canvas
                    using (var bitmap = new Bitmap(size, size))
                    {
                        using (var graphicsHandle = Graphics.FromImage(bitmap))
                        {
                            graphicsHandle.Clear(Color.White);
                            graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;

                            var deltaWidth = resized.Width - size;
                            var deltaHeight = resized.Height - size;

                            var x = 0;
                            if (deltaWidth != 0)
                            {
                                x = Math.Abs(deltaWidth/2);
                            }

                            var y = 0;
                            if (deltaHeight != 0)
                            {
                                y = Math.Abs(deltaHeight/2);
                            }

                            graphicsHandle.DrawImage(resized, x, y);
                        }
                        return ImageToByteArray(bitmap);
                    }
                }
            }
        }

        public static byte[] ResizeBuildingImage(byte[] image)
        {
            return ResizeToMaxSize(800, 800, image);
        }

        /// <summary>
        ///     Will create a square of size x size and proportionally center the image in that square
        /// </summary>
        /// <param name="jpg">The JPG.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static byte[] CenterResizeImage(byte[] jpg, int size)
        {
            using (var img = LoadFromByteArray(jpg))
            {
                var delta = img.Height > img.Width ? img.Height/size : img.Width/size;
                var height = img.Height/delta;
                var width = img.Width/delta;
                using (var resized = ResizeToMaxSize(width, height, img))
                {
                    return ImageToByteArray(resized);
                }
            }
        }

        public static byte[] CorrectOrientation(byte[] imageData)
        {
            try
            {
                using (var ms = new MemoryStream(imageData))
                {
                    using (var originalImage = Image.FromStream(ms))
                    {
                        if (originalImage.PropertyIdList.Contains(0x0112))
                        {
                            int rotationValue = originalImage.GetPropertyItem(0x0112).Value[0];
                            switch (rotationValue)
                            {
                                case 1: // landscape, do nothing
                                    break;

                                case 8: // rotated 90 right
                                    // de-rotate:
                                    originalImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                    break;

                                case 3: // bottoms up
                                    originalImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                    break;

                                case 6: // rotated 90 left
                                    originalImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                    break;
                            }
                        }
                        return ImageToByteArray(originalImage);
                    }
                }
            }
            catch //this is not a jpeg image
            {
                return imageData;
            }
        }
    }
}