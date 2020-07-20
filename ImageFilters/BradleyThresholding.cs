using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageFilters
{
    public class BradleyThresholding
    {
        private Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

        private int windowSize = 41;
        private float pixelBrightnessDifferenceLimit = 0.15f;

        private void CheckSourceFormat(PixelFormat pixelFormat)
        {
            if (!formatTranslations.ContainsKey(pixelFormat))
                throw new UnsupportedImageFormatException("Source pixel format is not supported by the filter.");
        }

        public int WindowSize
        {
            get { return windowSize; }
            set { windowSize = Math.Max(3, value | 1); }
        }

        public void ApplyInPlace(ref BitmapImage image)
        {
            //lock source bitmap data
            WriteableBitmap data = new WriteableBitmap(image);

            FormatConvertedBitmap converter = new FormatConvertedBitmap();

            converter.BeginInit();
            converter.Source = data;
            converter.DestinationFormat = PixelFormats.Indexed8;
            converter.DestinationPalette = BitmapPalettes.Halftone256;
            converter.EndInit();

             
            ProcessFilter(ref data);
            image = ConvertWriteableBitmapToBitmapImage(data);

        }
        public BitmapImage ConvertWriteableBitmapToBitmapImage(WriteableBitmap wbm)
        {
            BitmapImage bmImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(wbm));
                encoder.Save(stream);
                bmImage.BeginInit();
                bmImage.CacheOption = BitmapCacheOption.OnLoad;
                bmImage.StreamSource = stream;
                bmImage.EndInit();
                bmImage.Freeze();
            }
            return bmImage;
        }

        protected unsafe void ProcessFilter(ref WriteableBitmap image)
        {
            image.Lock();
            // create integral image
            IntegralImage im = IntegralImage.FromBitmap(image);

            int width = image.PixelWidth;
            int height = image.PixelHeight;
            int widthM1 = width - 1;
            int heightM1 = height - 1;

            int offset = image.BackBufferStride - width;
            int radius = windowSize / 2;

            float avgBrightnessPart = 1.0f - pixelBrightnessDifferenceLimit;

            byte* ptr = (byte*)image.BackBuffer;

            for (int y = 0; y < height; y++)
            {
                // rectangle's Y coordinates
                int y1 = y - radius;
                int y2 = y + radius;

                if (y1 < 0)
                    y1 = 0;
                if (y2 > heightM1)
                    y2 = heightM1;

                for (int x = 0; x < width; x++, ptr++)
                {
                    // rectangle's X coordinates
                    int x1 = x - radius;
                    int x2 = x + radius;

                    if (x1 < 0)
                        x1 = 0;
                    if (x2 > widthM1)
                        x2 = widthM1;

                    *ptr = (byte)((*ptr < (int)(im.GetRectangleMeanUnsafe(x1, y1, x2, y2) * avgBrightnessPart)) ? 0 : 255);
                }

                ptr += offset;
            }
            image.Unlock();
        }
            public BradleyThresholding()
        {
            
        }

    }
}
