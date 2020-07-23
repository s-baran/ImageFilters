
using ImageFilters.BlurFilters;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageFilters
{
    public partial class KernelWindow : Window
    {
        public KernelWindow(int size, double sigma)
        {
            InitializeComponent();
            Kernel kernel = new Kernel(size, sigma);
            cKernel = kernel.IntKernel;
            kernelAsText.Text = Helpers.MatrixToString(cKernel).ToString();
        }

        private int[,] cKernel;

        private int ckSize;

        public int[,] CKernel { get => cKernel; set => cKernel = value; }

        public int CkSize
        {
            get => ckSize;
            set
            {
                ckSize = Math.Max(3, Math.Min(31, value | 1));
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            cKernel = Helpers.ReadMatrixFromFile();
            if (cKernel != null)
            {
                StringBuilder sb = Helpers.MatrixToString(cKernel);
                kernelAsText.Text = sb.ToString();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            cKernel = Helpers.StringToMatrix(kernelAsText.Text);
            if (cKernel != null)
            {
                Helpers.SaveMatrixToFile(cKernel);
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            string value = SizeTextBox.Text;
            if (value != "")
            {
                int intvalue;
                if (int.TryParse(value, out intvalue))
                {
                    CkSize = intvalue;

                    cKernel = new int[ckSize, ckSize];

                    kernelAsText.Text = Helpers.MatrixToString(cKernel).ToString();
                    ValidationLabel.Content = null;
                    SizeTextBox.Text = ckSize.ToString();
                }
                else ValidationLabel.Content = "Podaj cyfrę!";
            }
            else { ValidationLabel.Content = "Podaj wartość!"; };
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mwnd = (MainWindow)Application.Current.MainWindow;
            mwnd.IsLoadedLabel.Visibility = Visibility.Visible;
            try
            {
                cKernel = Helpers.StringToMatrix(kernelAsText.Text);
            }
            catch (Exception ex)
            {
                mwnd.IsLoadedLabel.Content = ex.Message;
                return;
            }
            if (cKernel.GetLength(0) == cKernel.GetLength(1)&&cKernel.GetLength(0)>3)
            {
                mwnd.IsLoadedLabel.Content = "Custom Kernel loaded!";
                mwnd.ControlsEnabling();
            }
            else
            {
                mwnd.IsLoadedLabel.Content = "Wrong size of kernel!";
            }
            
        }

        protected override void OnClosed(EventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).IsLoadedLabel.Visibility = Visibility.Collapsed;
            base.OnClosed(e);
        }
    }
}
