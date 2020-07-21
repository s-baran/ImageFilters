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
        }

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
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreImageCache;
                bitmap.EndInit();

                
                ImageViewer.Source = bitmap;
                BradleyButton.IsEnabled = true;
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
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(FileNameLabel.Content.ToString());
            
            image.EndInit();
            filter.ApplyInPlace(ref image);
            ImageViewer.Source = image;
        }
    }
}
