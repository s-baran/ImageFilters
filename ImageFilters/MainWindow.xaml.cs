using ImageFilters.BlurFilters;
using ImageFilters.Thresholding;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
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
        private BitmapImage originalImage;
        private BitmapImage processedImage;
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


                ImageViewer.Source = originalImage;
                ControlsEnabling();
            }
        }

        private void GetRealOrientation(BitmapImage image)
        {
            using (FileStream fileStream = new FileStream(image.UriSource.ToString(), FileMode.Open, FileAccess.Read))
            {
                BitmapFrame bitmapFrame = BitmapFrame.Create(fileStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                BitmapMetadata bitmapMetadata = bitmapFrame.Metadata as BitmapMetadata;

                if ((bitmapMetadata != null) && (bitmapMetadata.ContainsQuery("System.Photo.Orientation")))
                {
                    object o = bitmapMetadata.GetQuery("System.Photo.Orientation");
                    //TODO: make orientation 
                }
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
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(processedImage));
                    encoder.Save(stream);
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
            Blur gaussianBlurFilter = new Blur((int)SigmaSlider.Value,(int)BlurSizeSlider.Value);

            gaussianBlurFilter.Apply(originalImage);
        }

        private void ControlsEnabling()
        {
            BradleyButton.IsEnabled = true;
            SaveButton.IsEnabled = true;
            GaussBlurButton.IsEnabled = true;
            BlurButton.IsEnabled = true;
        }
        private void ControlsDisabling()
        {
            BradleyButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
            GaussBlurButton.IsEnabled = false;
            BlurButton.IsEnabled = false;
        }
    }
}
