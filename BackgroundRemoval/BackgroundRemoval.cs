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

            SobelEdgeDetector sobelEdgeDetector = new();
            CannyEdgeDetector cannyEdgeDetector = new();
            Bitmap imageToProcess = edgeDetectionType switch
            {
                EdgeDetectionTypes.Sobel => sobelEdgeDetector.Apply(grayscale),
                EdgeDetectionTypes.Canny => cannyEdgeDetector.Apply(grayscale),
                EdgeDetectionTypes.SobelThenCanny => cannyEdgeDetector.Apply(sobelEdgeDetector.Apply(grayscale)),
                _ => throw new ArgumentOutOfRangeException(nameof(edgeDetectionType), "Unknown edge detection type"),
            };
            BlobCounter blobCounter = new()
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
            List<IntPoint> corners = new();
            
            // Create convex hull searching algorithm
            GrahamConvexHull hullFinder = new();
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
                Bitmap clippedImage = new((int)mainObjectBounds.Width, (int)mainObjectBounds.Height);//, PixelFormat.Format32bppArgb);
                using var g = Graphics.FromImage(clippedImage);
                g.Clip = clipRegion; // Restrict drawing region
                g.DrawImage(image, -mainObjectBounds.X, -mainObjectBounds.Y, image.Width, image.Height); // Draw clipped
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
            Rectangle destRect = new(0, 0, width, height);
            Bitmap destImage = new(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using var graphics = Graphics.FromImage(destImage);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using ImageAttributes wrapMode = new();
            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

            return destImage;
        }

        public static Bitmap ChangePixelFormat(Bitmap image, PixelFormat pixelFormat)
        {
            Bitmap clone = new(image.Width, image.Height, pixelFormat);

            using var gr = Graphics.FromImage(clone);
            gr.DrawImage(image, new Rectangle(0, 0, clone.Width, clone.Height));

            return clone;
        }
    }
}
