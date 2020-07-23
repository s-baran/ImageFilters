using ImageFilters.BlurFilters;
using ImageFilters.Thresholding;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Markup.Localizer;
using System.Windows.Media.Imaging;

namespace ImageFilters
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ControlsDisabling();
        }
        private bool isImageLoaded = false;
        private BitmapImage originalImage;
        private BitmapImage processedImage;
        private KernelWindow kernelWindow;

        public bool IsImageLoaded { get => isImageLoaded; set => isImageLoaded = value; }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                FileName.Content = selectedFileName;
                originalImage = new BitmapImage();
                originalImage.BeginInit();
                originalImage.UriSource = new Uri(selectedFileName);
                originalImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreImageCache;
                originalImage.EndInit();

                processedImage = originalImage.Clone();
                ImageViewer.Source = originalImage;
                IsImageLoaded = true;
                ControlsEnabling();
            }
        }


        private void BradleyButton_Click(object sender, RoutedEventArgs e)
        {
            BradleyThresholding filter = new BradleyThresholding(ThresholdSlider.Value, SizeSlider.Value);
            processedImage = new BitmapImage();
            processedImage = originalImage.Clone();
            filter.ApplyInPlace(ref processedImage);
            ImageViewer.Source = processedImage;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PNG (*.png)|*.png|JPEG (*.jpg;*.jpeg;*jpe;*jfif)|*.jpg;*.jpeg;*jpe;*jfif|Bitmap (*.bmp)|*.bmp|TIFF (*.tiff;*.tif)|*.tiff;*.tif";

            if (saveDialog.ShowDialog() == true)
            {
                using (FileStream stream = new FileStream(saveDialog.FileName, FileMode.Create))
                {
                    switch (saveDialog.FilterIndex)
                    {
                        case 1:
                            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                            pngEncoder.Frames.Add(BitmapFrame.Create(processedImage));
                            pngEncoder.Save(stream);
                            break;
                        case 2:
                            JpegBitmapEncoder jpegEncoder = new JpegBitmapEncoder();
                            jpegEncoder.Frames.Add(BitmapFrame.Create(processedImage));
                            jpegEncoder.Save(stream);
                            break;
                        case 3:
                            BmpBitmapEncoder bitmapEncoder = new BmpBitmapEncoder();
                            bitmapEncoder.Frames.Add(BitmapFrame.Create(processedImage));
                            bitmapEncoder.Save(stream);
                            break;
                        case 4:
                            TiffBitmapEncoder tiffEncoder = new TiffBitmapEncoder();
                            tiffEncoder.Frames.Add(BitmapFrame.Create(processedImage));
                            tiffEncoder.Save(stream);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ImageViewer.Source = originalImage;
        }

        private void BlurButton_Click(object sender, RoutedEventArgs e)
        {
            Blur blurFilter = new Blur((int)BlurSizeSlider.Value);

            processedImage = blurFilter.Apply(originalImage);
            ImageViewer.Source = processedImage;
        }

        private void GaussBlurButton_Click(object sender, RoutedEventArgs e)
        {
            Blur gaussianBlurFilter = new Blur(SigmaSlider.Value, (int)BlurSizeSlider.Value);

            processedImage = gaussianBlurFilter.Apply(originalImage);
            ImageViewer.Source = processedImage;
        }

        public void ControlsEnabling()
        {
            if (IsImageLoaded)
            {
                BradleyButton.IsEnabled = true;
                SaveButton.IsEnabled = true;
                GaussBlurButton.IsEnabled = true;
                BlurButton.IsEnabled = true;
                if (IsLoadedLabel.Visibility == Visibility.Visible) ApplyKernelButton.IsEnabled = true;
            }
        }
        private void ControlsDisabling()
        {
            BradleyButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
            GaussBlurButton.IsEnabled = false;
            BlurButton.IsEnabled = false;
        }

        private void KernelCreator_Click(object sender, RoutedEventArgs e)
        {
            kernelWindow = new KernelWindow((int)BlurSizeSlider.Value, SigmaSlider.Value);
            kernelWindow.Show();
        }

        private void ApplyKernel_Click(object sender, RoutedEventArgs e)
        {
            Blur customKernelFilter = new Blur(kernelWindow.CKernel);
            processedImage = customKernelFilter.Apply(originalImage);
            ImageViewer.Source = processedImage;
        }
    }
}
