using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            BradleyButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
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
                FileNameLabel.Content = selectedFileName;
                originalImage = new BitmapImage();
                originalImage.BeginInit();
                originalImage.UriSource = new Uri(selectedFileName);
                originalImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreImageCache;
                originalImage.EndInit();


                ImageViewer.Source = originalImage;
                BradleyButton.IsEnabled = true;
                SaveButton.IsEnabled = true;
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
    }
}
