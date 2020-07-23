using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageFilters.BlurFilters
{

    public class Blur
    {
        // Index of alpha component for ARGB images. 
        public const short A = 3;
        // Index of red component.
        public const short R = 2;
        // Index of green component.
        public const short G = 1;
        // Index of blue component.    
        public const short B = 0;


        private bool processAlpha = true;
        private int size;
        private int[,] kernel;
        private int divisor = 1;
        private int threshold = 0;
        private bool dynamicDivisorForEdges = true;

        private unsafe void ProcessFilter(WriteableBitmap source, WriteableBitmap destination, Int32Rect rect)
        {
            int pixelSize = source.Format.BitsPerPixel / 8;
             

            int startX = rect.X;
            int startY = rect.Y;
            int stopX = startX + rect.Width;
            int stopY = startY + rect.Height;                  

            // check pixel size to find if we deal with 8 or 16 bpp channels
            if ((pixelSize <= 4) && (pixelSize != 2))
            {
                int srcStride = source.BackBufferStride;
                int dstStride = destination.BackBufferStride;

                int srcOffset = srcStride - rect.Width * pixelSize;
                int dstOffset = dstStride - rect.Width * pixelSize;

                byte* src = (byte*)source.BackBuffer;
                byte* dst = (byte*)destination.BackBuffer;

                // allign pointers to the first pixel to process
                src += (startY * srcStride + startX * pixelSize);
                dst += (startY * dstStride + startX * pixelSize);

                // do the processing job
                if (destination.Format == PixelFormats.Gray8)
                {
                    // grayscale image
                    Process8bppImage(src, dst, srcStride, dstStride, srcOffset, dstOffset, startX, startY, stopX, stopY);
                }
                else
                {
                    // RGB image
                    if ((pixelSize == 3) || (!processAlpha))
                    {
                        Process24bppImage(src, dst, srcStride, dstStride, srcOffset, dstOffset, startX, startY, stopX, stopY, pixelSize);
                    }
                    else
                    {
                        Process32bppImage(src, dst, srcStride, dstStride, srcOffset, dstOffset, startX, startY, stopX, stopY);
                    }
                }
            }
            else
            {
                pixelSize /= 2;

                int dstStride = destination.BackBufferStride / 2;
                int srcStride = source.BackBufferStride / 2;

                // base pointers
                ushort* baseSrc = (ushort*)source.BackBuffer;
                ushort* baseDst = (ushort*)destination.BackBuffer;

                // allign pointers by X
                baseSrc += (startX * pixelSize);
                baseDst += (startX * pixelSize);

                if (source.Format == PixelFormats.Gray16)
                {
                    // 16 bpp grayscale image
                    Process16bppImage(baseSrc, baseDst, srcStride, dstStride, startX, startY, stopX, stopY);
                }
                else
                {
                    // RGB image
                    if ((pixelSize == 3) || (!processAlpha))
                    {
                        Process48bppImage(baseSrc, baseDst, srcStride, dstStride, startX, startY, stopX, stopY, pixelSize);
                    }
                    else
                    {
                        Process64bppImage(baseSrc, baseDst, srcStride, dstStride, startX, startY, stopX, stopY);
                    }
                }
            }
        }

        private unsafe void Process64bppImage(ushort* baseSrc, ushort* baseDst, int srcStride, int dstStride, int startX, int startY, int stopX, int stopY)
        {
            throw new NotImplementedException();
        }

        private unsafe void Process48bppImage(ushort* baseSrc, ushort* baseDst, int srcStride, int dstStride, int startX, int startY, int stopX, int stopY, int pixelSize)
        {
            throw new NotImplementedException();
        }

        private unsafe void Process32bppImage(byte* src, byte* dst, int srcStride, int dstStride, int srcOffset, int dstOffset, int startX, int startY, int stopX, int stopY)
        {
            // loop and array indexes
            int i, j, t, k, ir, jr;
            // kernel's radius
            int radius = size >> 1;
            // color sums
            long r, g, b, a, div;

            // kernel size
            int kernelSize = size * size;
            // number of kernel elements taken into account
            int processedKernelSize;

            byte* p;

            // for each line
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, src += 4, dst += 4)
                {
                    r = g = b = a = div = processedKernelSize = 0;

                    // for each kernel row
                    for (i = 0; i < size; i++)
                    {
                        ir = i - radius;
                        t = y + ir;

                        // skip row
                        if (t < startY)
                            continue;
                        // break
                        if (t >= stopY)
                            break;

                        // for each kernel column
                        for (j = 0; j < size; j++)
                        {
                            jr = j - radius;
                            t = x + jr;

                            // skip column
                            if (t < startX)
                                continue;

                            if (t < stopX)
                            {
                                k = kernel[i, j];
                                p = &src[ir * srcStride + jr * 4];

                                div += k;

                                b += k * p[B];
                                g += k * p[G];
                                r += k * p[R];
                                a += k * p[A];

                                processedKernelSize++;
                            }
                        }
                    }

                    // check if all kernel elements were processed
                    if (processedKernelSize == kernelSize)
                    {
                        // all kernel elements are processed - we are not on the edge
                        div = divisor;
                    }
                    else
                    {
                        // we are on edge. do we need to use dynamic divisor or not?
                        if (!dynamicDivisorForEdges)
                        {
                            // do
                            div = divisor;
                        }
                    }

                    // check divider
                    if (div != 0)
                    {
                        r /= div;
                        g /= div;
                        b /= div;
                        a /= div;
                    }
                    r += threshold;
                    g += threshold;
                    b += threshold;
                    a += threshold;

                    dst[R] = (byte)((r > 255) ? 255 : ((r < 0) ? 0 : r));
                    dst[G] = (byte)((g > 255) ? 255 : ((g < 0) ? 0 : g));
                    dst[B] = (byte)((b > 255) ? 255 : ((b < 0) ? 0 : b));
                    dst[A] = (byte)((a > 255) ? 255 : ((a < 0) ? 0 : a));
                }
                src += srcOffset;
                dst += dstOffset;
            }
        }

        private unsafe void Process24bppImage(byte* src, byte* dst, int srcStride, int dstStride, int srcOffset, int dstOffset, int startX, int startY, int stopX, int stopY, int pixelSize)
        {
            throw new NotImplementedException();
        }

        private unsafe void Process16bppImage(ushort* baseSrc, ushort* baseDst, int srcStride, int dstStride, int startX, int startY, int stopX, int stopY)
        {
            throw new NotImplementedException();
        }

        private unsafe void Process8bppImage(byte* src, byte* dst, int srcStride, int dstStride, int srcOffset, int dstOffset, int startX, int startY, int stopX, int stopY)
        {
            // loop and array indexes
            int i, j, t, k, ir, jr;
            // kernel's radius
            int radius = size >> 1;
            // color sums
            long g, div;

            // kernel size
            int kernelSize = size * size;
            // number of kernel elements taken into account
            int processedKernelSize;

            // for each line
            for (int y = startY; y < stopY; y++)
            {
                // for each pixel
                for (int x = startX; x < stopX; x++, src++, dst++)
                {
                    g = div = processedKernelSize = 0;

                    // for each kernel row
                    for (i = 0; i < size; i++)
                    {
                        ir = i - radius;
                        t = y + ir;

                        // skip row
                        if (t < startY)
                            continue;
                        // break
                        if (t >= stopY)
                            break;

                        // for each kernel column
                        for (j = 0; j < size; j++)
                        {
                            jr = j - radius;
                            t = x + jr;

                            // skip column
                            if (t < startX)
                                continue;

                            if (t < stopX)
                            {
                                k = kernel[i, j];

                                div += k;
                                g += k * src[ir * srcStride + jr];
                                processedKernelSize++;
                            }
                        }
                    }

                    // check if all kernel elements were processed
                    if (processedKernelSize == kernelSize)
                    {
                        // all kernel elements are processed - we are not on the edge
                        div = divisor;
                    }
                    else
                    {
                        // we are on edge. do we need to use dynamic divisor or not?
                        if (!dynamicDivisorForEdges)
                        {
                            // do
                            div = divisor;
                        }
                    }

                    // check divider
                    if (div != 0)
                    {
                        g /= div;
                    }
                    g += threshold;
                    *dst = (byte)((g > 255) ? 255 : ((g < 0) ? 0 : g));
                }
                src += srcOffset;
                dst += dstOffset;
            }
        }
       
        public BitmapImage Apply(BitmapImage image)
        {
            WriteableBitmap sourceWbmp = new WriteableBitmap(image);
            WriteableBitmap outputWbmp = new WriteableBitmap(image.PixelWidth, image.PixelHeight, image.DpiX, image.DpiY, image.Format, image.Palette);

            ProcessFilter(sourceWbmp, outputWbmp, new Int32Rect(0, 0,image.PixelWidth,image.PixelHeight));
            return Helpers.ConvertWriteableBitmapToBitmapImage(outputWbmp);               
        }

        public Blur(double sigma, int size)
        {
            this.size = Math.Min(21,size);
            Kernel kernel = new Kernel(this.size,sigma);
            this.kernel = kernel.IntKernel;
            this.divisor = kernel.Divisor;
        }

        public Blur(int[,] customKernel)
        {
            this.size = customKernel.GetLength(0);
            this.kernel = customKernel;
            this.divisor = Kernel.ComputeDivisor(customKernel);            
        }
        public Blur(int size)
        {
            this.size = size;
            Kernel kernel = new Kernel(size);
            this.kernel = kernel.IntKernel;
            this.divisor = kernel.Divisor;
        }
    }
}

