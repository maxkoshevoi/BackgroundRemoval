using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using IntPoint = AForge.IntPoint;

namespace BackgroundRemoval.Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        FilterInfoCollection _device;
        VideoCaptureDevice _captureDevice;
        Bitmap sourceImage;
        
        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                _device = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                for (var i = 0; i < _device.Count; i++)
                {
                    cbCamera.Items.Add(_device[i].Name);
                }
                cbCamera.Items.Add("Select file...");
                cbCamera.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopCameras();
        }

        private void cbCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            StopCameras();
            if (cbCamera.SelectedIndex == cbCamera.Items.Count - 1)
            {
                ImportFile();
            }
            else
            {
                SrartCameras(cbCamera.SelectedIndex);
            }
        }

        private void ImportFile()
        {
            try
            {
                _captureDevice = null;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    sourceImage = new Bitmap(openFileDialog.FileName);
                    
                    sourceImage = BackgroundRemoval.Resize(sourceImage, 1000);

                    FrameProcessor(null, new NewFrameEventArgs(sourceImage));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void Filter_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCamera.SelectedIndex == cbCamera.Items.Count - 1)
            {
                FrameProcessor(null, new NewFrameEventArgs(sourceImage));
            }
        }

        private void SrartCameras(int deviceindex)
        {
            try
            {
                _captureDevice = new VideoCaptureDevice(_device[deviceindex].MonikerString);
                _captureDevice.NewFrame += FrameProcessor;
                //Start the Capture Device
                _captureDevice.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void StopCameras()
        {
            _captureDevice?.Stop();
        }

        private void FrameProcessor(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();
            DetectObjectInImage(frame);
        }
        
        private void DetectObjectInImage(Bitmap image)
        {
            GaussianBlur gaussianBlur = new(4, 11);
            SobelEdgeDetector sobelEdgeDetector = new();
            CannyEdgeDetector cannyEdgeDetector = new();

            Bitmap grayscale = Grayscale.CommonAlgorithms.BT709.Apply(image);

            //if (chGaussianBlur.Checked)
            //{
            //    Bitmap blur = gaussianBlur.Apply(grayscale);
            //    pbOriginal.Image = (Bitmap)grayscale.Clone();
            //    grayscale = blur;
            //}

            Bitmap sobel = sobelEdgeDetector.Apply(grayscale);
            Bitmap canny = cannyEdgeDetector.Apply(grayscale);
            Bitmap sc = cannyEdgeDetector.Apply(sobel);
            
            pbSobel.Image = (Bitmap)sobel.Clone();
            pbCanny.Image = (Bitmap)canny.Clone();
            pbSC.Image = (Bitmap)sc.Clone();

            Bitmap imageToProcess;
            if (rbSobel.Checked)
            {
                imageToProcess = sobel;
            }
            else if (rbCanny.Checked)
            {
                imageToProcess = canny;
            }
            else
            {
                imageToProcess = sc;
            }

            PaintObjects((Bitmap)imageToProcess.Clone(), image, out Bitmap allObjects, out Bitmap mainObject);
            pbOriginal.Image = image;
            pbAllObjects.Image = allObjects;
            pbMainObject.Image = mainObject;
        }

        private static void PaintObjects(Bitmap grayImage, Bitmap imageToDraw, out Bitmap allObjects, out Bitmap mainObject)
        {
            mainObject = (Bitmap)imageToDraw.Clone();
            allObjects = (Bitmap)imageToDraw.Clone();

            BlobCounter blobCounter = GetObjectsRectangles(grayImage);

            DrawAllObjects(ref allObjects, blobCounter.GetObjectsRectangles());
            //DrawMainObjectRectangle(ref mainObject, blobCounter);
            DrawMainObjectEdges(ref mainObject, blobCounter); // Same as calling BackgroundRemoval.RemoveBackground() 
        }

        private static BlobCounter GetObjectsRectangles(Bitmap grayImage)
        {
            BlobCounter blobCounter = new()
            {
                BlobsFilter = new GradientSizeBlobFilter(grayImage.Size, new Size(5, 5), new Size(100, 100)),
                FilterBlobs = true,
                ObjectsOrder = ObjectsOrder.Size
            };
            blobCounter.ProcessImage(grayImage);
            return blobCounter;
        }

        private static void DrawAllObjects(ref Bitmap allObjects, Rectangle[] rectangles)
        {
            using var g = Graphics.FromImage(allObjects);
            using Pen pen = new(Color.FromArgb(160, 255, 160), 3);
            foreach (Rectangle rect in rectangles)
            {
                g.DrawRectangle(pen, rect);
            }
        }

        private static void DrawMainObjectRectangle(ref Bitmap mainObject, BlobCounter blobCounter)
        {
            List<IntPoint> corners = new();
            GrahamConvexHull hullFinder = new();
            foreach (Rectangle rect in blobCounter.GetObjectsRectangles())
            {
                corners.Add(new(rect.X, rect.Y));
                corners.Add(new(rect.X, rect.Bottom));
                corners.Add(new(rect.Right, rect.Bottom));
                corners.Add(new(rect.Right, rect.Y));
                corners = hullFinder.FindHull(corners);
            }

            if (corners.Any())
            {
                using var g = Graphics.FromImage(mainObject);
                using Pen pen = new(Color.FromArgb(160, 255, 160), 3);
                g.DrawPolygon(pen, corners.Select(c => new Point(c.X, c.Y)).ToArray());
            }
        }

        private static void DrawMainObjectEdges(ref Bitmap mainObject, BlobCounter blobCounter)
        {
            List<IntPoint> corners = new();
            // create convex hull searching algorithm
            GrahamConvexHull hullFinder = new();
            foreach (Blob blob in blobCounter.GetObjectsInformation())
            {
                // get blob's edge points
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blob);
                corners.AddRange(edgePoints);
                // blob's convex hull
                corners = hullFinder.FindHull(corners);
            }

            if (corners.Any())
            {
                //using (Graphics g = Graphics.FromImage(mainObject))
                //using (Pen pen = new Pen(Color.FromArgb(160, 255, 160), 3))
                //{
                //    g.DrawPolygon(pen, corners.Select(c => new Point(c.X, c.Y)).ToArray());
                //}

                GraphicsPath graphicsPath = new();
                graphicsPath.AddPolygon(corners.Select(c => new Point(c.X, c.Y)).ToArray());
                RectangleF mainObjectBounds = graphicsPath.GetBounds();

                Region clipRegion = new(graphicsPath);
                clipRegion.Translate(-mainObjectBounds.X, -mainObjectBounds.Y);

                Bitmap clippedImage = new((int)mainObjectBounds.Width, (int)mainObjectBounds.Height);//, PixelFormat.Format32bppArgb);
                using var g = Graphics.FromImage(clippedImage);
                g.Clip = clipRegion;   // restrict drawing region
                g.DrawImage(mainObject, -mainObjectBounds.X, -mainObjectBounds.Y, mainObject.Width, mainObject.Height); // draw clipped
                
                mainObject = clippedImage;
            }
        }
    }
}
