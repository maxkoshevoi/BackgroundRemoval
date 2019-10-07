using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using IntPoint = AForge.IntPoint;

namespace BackgroundRemoval
{
    public static class BackgroundRemoval
    {
        /// <summary>
        /// Removes backgroung from the image using object detection
        /// </summary>
        /// <param name="image">Image to remove backgroun from</param>
        /// <param name="edgeDetectionType">Filter type </param>
        /// <returns>Clipped image with transparent background</returns>
        public static Bitmap RemoveBackground(Bitmap image, EdgeDetectionTypes edgeDetectionType = EdgeDetectionTypes.Canny)
        {
            Bitmap grayscale = Grayscale.CommonAlgorithms.BT709.Apply(image);

            SobelEdgeDetector sobelEdgeDetector = new SobelEdgeDetector();
            CannyEdgeDetector cannyEdgeDetector = new CannyEdgeDetector();
            
            Bitmap imageToProcess;
            switch (edgeDetectionType)
            {
                case EdgeDetectionTypes.Sobel:
                    imageToProcess = sobelEdgeDetector.Apply(grayscale);
                    break;
                case EdgeDetectionTypes.Canny:
                    imageToProcess = cannyEdgeDetector.Apply(grayscale);
                    break;
                case EdgeDetectionTypes.SobelThenCanny:
                    Bitmap sobel = sobelEdgeDetector.Apply(grayscale);
                    imageToProcess = cannyEdgeDetector.Apply(sobel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(edgeDetectionType), "Unknown edge detection type");
            }

            BlobCounter blobCounter = new BlobCounter
            {
                BlobsFilter = new GradientSizeBlobFilter(imageToProcess.Size, new Size(5, 5), new Size(100, 100)),
                FilterBlobs = true
            };
            blobCounter.ProcessImage(imageToProcess);

            Bitmap objectWithoutBackground = ClipMainObject(image, blobCounter);
            return objectWithoutBackground;
        }

        private static Bitmap ClipMainObject(Bitmap image, BlobCounter blobCounter)
        {
            List<IntPoint> corners = new List<IntPoint>();
            // Create convex hull searching algorithm
            GrahamConvexHull hullFinder = new GrahamConvexHull();
            foreach (Blob blob in blobCounter.GetObjectsInformation())
            {
                // Get blob's edge points
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blob);
                corners.AddRange(edgePoints);
                // Blob's convex hull
                corners = hullFinder.FindHull(corners);
            }

            if (corners.Any())
            {
                GraphicsPath graphicsPath = new GraphicsPath();
                graphicsPath.AddPolygon(corners.Select(c => new Point(c.X, c.Y)).ToArray());
                RectangleF mainObjectBounds = graphicsPath.GetBounds();

                // Move region to top left corner
                Region clipRegion = new Region(graphicsPath);
                clipRegion.Translate(-mainObjectBounds.X, -mainObjectBounds.Y);

                // Draw selected region
                Bitmap clippedImage = new Bitmap((int)mainObjectBounds.Width, (int)mainObjectBounds.Height);//, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(clippedImage))
                {
                    g.Clip = clipRegion; // Restrict drawing region
                    g.DrawImage(image, -mainObjectBounds.X, -mainObjectBounds.Y, image.Width, image.Height); // Draw clipped
                }
                return clippedImage;
            }

            return image;
        }

        public static Bitmap Resize(Bitmap image, int maxBorder)
        {
            if (image.Width <= maxBorder && image.Height <= maxBorder)
            {
                return image;
            }

            int newWidth, newHeight;
            if (image.Width > maxBorder)
            {
                newWidth = maxBorder;
                newHeight = (int)(image.Height * (maxBorder / (double)image.Width));
            }
            else
            {
                newHeight = maxBorder;
                newWidth = (int)(image.Width * (maxBorder / (double)image.Height));
            }
            // Bitmap resizedImage = new Bitmap(image, newWidth, newHeight);
            Bitmap resizedImage = Resize(image, newWidth, newHeight);
            if (resizedImage.PixelFormat != image.PixelFormat)
            {
                resizedImage = ChangePixelFormat(resizedImage, image.PixelFormat);
            }
            return resizedImage;
        }

        public static Bitmap Resize(Bitmap image, int width, int height)
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
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Bitmap ChangePixelFormat(Bitmap image, PixelFormat pixelFormat)
        {
            Bitmap clone = new Bitmap(image.Width, image.Height, pixelFormat);

            using (Graphics gr = Graphics.FromImage(clone))
            {
                gr.DrawImage(image, new Rectangle(0, 0, clone.Width, clone.Height));
            }

            return clone;
        }
    }
}
