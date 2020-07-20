using System;
using System.Drawing; 
using System.Windows.Media.Imaging;

namespace ImageFilters
{
    public class IntegralImage
    {
        protected uint[,] integralImage = null;
        private int width;
        private int height;

        public IntegralImage(int width, int height)
        {
            this.width = width;
            this.height = height;
            integralImage = new uint[height + 1, width + 1];
        }
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }
         
        public static IntegralImage FromBitmap(WriteableBitmap image)
        {
            image.Lock();

            // get source image size
            int width = image.PixelWidth;
            int height = image.PixelHeight;
            int offset = image.BackBufferStride - width;

            // create integral image
            IntegralImage im = new IntegralImage(width, height);
            uint[,] integralImage = im.integralImage;

            // do the job
            unsafe
            {
                byte* src = (byte*)image.BackBuffer;

                // for each line
                for (int y = 1; y <= height; y++)
                {
                    uint rowSum = 0;

                    // for each pixel
                    for (int x = 1; x <= width; x++, src++)
                    {
                        rowSum += *src;

                        integralImage[y, x] = rowSum + integralImage[y - 1, x];
                    }
                    src += offset;
                }
            }

            image.Unlock();
            return im;
        }

        public float GetRectangleMeanUnsafe(int x1, int y1, int x2, int y2)
        {
            x2++;
            y2++;

            // return sum divided by actual rectangles size
            return (float)((double)(integralImage[y2, x2] + integralImage[y1, x1] - integralImage[y2, x1] - integralImage[y1, x2]) /
                (double)((x2 - x1) * (y2 - y1)));
        }
    }
}

