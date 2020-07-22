using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageFilters.Thresholding
{
    public class UnmanagedImage : IDisposable
    {
        private IntPtr imageData;
        // image size
        private int width, height;
        // image stride (line size)
        private int stride;
        // image pixel format
        private PixelFormat pixelFormat;  
        // flag which indicates if the image should be disposed or not
        private bool mustBeDisposed = false;

        public IntPtr ImageData
        {
            get { return imageData; }
        }
        public int Width
        {
            get { return width; }
        }

      
        public int Height
        {
            get { return height; }
        }

       
        public int Stride
        {
            get { return stride; }
        }

        public PixelFormat PixelFormat
        {
            get { return pixelFormat; }
        }
        public UnmanagedImage(WriteableBitmap bitmapData)
        {
            this.imageData = bitmapData.BackBuffer;
            this.width = bitmapData.PixelWidth;
            this.height = bitmapData.PixelHeight;
            this.stride = bitmapData.BackBufferStride;
            this.pixelFormat = bitmapData.Format;
        }

        ~UnmanagedImage()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            // remove me from the Finalization queue 
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
            }
            // free image memory if the image was allocated using this class
            if ((mustBeDisposed) && (imageData != IntPtr.Zero))
            {
                System.Runtime.InteropServices.Marshal.FreeHGlobal(imageData);
                System.GC.RemoveMemoryPressure(stride * height);
                imageData = IntPtr.Zero;
            }
        }
    }
}
