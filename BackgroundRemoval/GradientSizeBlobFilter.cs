using AForge.Imaging;
using System;
using System.Drawing;

namespace BackgroundRemoval
{
    public class GradientSizeBlobFilter : IBlobsFilter
    {
        readonly PointF imageCenter;
        readonly Size minBlobSize, maxBlobSize;

        public GradientSizeBlobFilter(Size ImageSize, Size MinBlobSize, Size MaxBlobSize)
        {
            imageCenter = new PointF(ImageSize.Width / 2f, ImageSize.Height / 2f);
            minBlobSize = MinBlobSize;
            maxBlobSize = MaxBlobSize;
        }

        public bool Check(Blob blob)
        {
            float widthMult = Math.Abs(imageCenter.X - blob.CenterOfGravity.X) / imageCenter.X;
            float heightMult = Math.Abs(imageCenter.Y - blob.CenterOfGravity.Y) / imageCenter.Y;
            
            int neededWidth = minBlobSize.Width + (int)Math.Round((maxBlobSize.Width - minBlobSize.Width) * widthMult);
            int neededHeigth = minBlobSize.Height + (int)Math.Round((maxBlobSize.Height - minBlobSize.Height) * heightMult);

            return blob.Rectangle.Width >= neededWidth && blob.Rectangle.Height >= neededHeigth;
        }
    }
}
