using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ImageFilters.BlurFilters
{
    public class Kernel
    {
        private int[,] intKernel;
        private int size = 5;
        private double sigma = 1.4;
        private double sqrSigma = 1.0;
        private int divisor;

        public int Size
        {
            get => size;
            set
            {
                size = Math.Max(3, Math.Min(101, value | 1));
            }
        }

        public double Sigma
        {
            get => sigma;
            set
            {
                sigma = Math.Max(0.5, Math.Min(5.0, value));
                sqrSigma = sigma * sigma;
            }
        }

        public int Divisor { get => divisor; }
        public int[,] IntKernel { get => intKernel; }

       

       
        private double GausianFunction(double x, double y)
        {
            return Math.Exp((x * x + y * y) / (-2 * sqrSigma)) / (2 * Math.PI * sqrSigma);
        }

        private void CreateGaussianKernel()
        {
            // check for evem size and for out of range
            if (((size % 2) == 0) || (size < 3) || (size > 101))
            {
                throw new ArgumentException("Wrong kernal size.");
            }

            intKernel = new int[size, size];
            // raduis
            int r = size / 2;
            // kernel
            double[,] kernel = new double[size, size];

            divisor = 1;
            // compute kernel
            for (int y = -r, i = 0; i < size; y++, i++)
            {
                for (int x = -r, j = 0; j < size; x++, j++)
                {
                    kernel[i, j] = GausianFunction(x, y);
                }
            }

            double min = kernel[0, 0];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    double v = kernel[i, j] / min;

                    if (v > ushort.MaxValue)
                    {
                        v = ushort.MaxValue;
                    }
                    intKernel[i, j] = (int)v;

                    // collect divisor
                    divisor += intKernel[i, j];
                }
            }
        }
        public static int ComputeDivisor(int[,] kernel)
        {
            int divisor = 1;
            int size = kernel.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {   
                    // collect divisor
                    divisor += kernel[i, j];
                }
            }
            return divisor;
        }
        private void CreateBlurKernel()
        {
            if (((size % 2) == 0) || (size < 3) || (size > 101))
            {
                throw new ArgumentException("Wrong kernal size.");
            }
            intKernel = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    intKernel[i, j] = 1;
                    divisor++;
                }
            }

        }

        public Kernel(int size, double sigma)
        {
            Size = Math.Min(21, size);
            Sigma = sigma;
            CreateGaussianKernel();
        }
        public Kernel(int size)
        {
            Size = size;
            CreateBlurKernel();
        }

    }
}
