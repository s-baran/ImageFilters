using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageFilters
{
    public class Helpers
    {
        public static BitmapImage ConvertWriteableBitmapToBitmapImage(WriteableBitmap wbm)
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

        public static int[,] ReadMatrixFromFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Text files (*.txt)|*.txt";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;

                List<string> lines = new List<string>();
                if (!File.Exists(selectedFileName))
                    return null;
                using (var reader = new StreamReader(selectedFileName))
                {
                    while (!reader.EndOfStream)
                        lines.Add(reader.ReadLine());
                }

                var columns = lines[0].Split('\t').Count() - 1;
                var rows = lines.Count;
                int[,] matrix = new int[rows, columns];
                var lineNumber = 0;

                for (var i = 0; i < rows; i++)
                {
                    var values = lines[i].Split('\t');
                    for (var j = 0; j < columns; j++)
                    {
                        int.TryParse(values[j], out matrix[lineNumber, j]);
                    }
                    lineNumber++;
                }

                return matrix;
            }
            return null;
        }

        public static void SaveMatrixToFile(int[,] matrix)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Txt(*.txt)|*.txt";

            if (saveDialog.ShowDialog() == true)
            {

                using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                {
                    for (var y = 0; y < matrix.GetLength(0); y++)
                    {
                        for (var x = 0; x < matrix.GetLength(1); x++)
                        {
                            writer.Write(matrix[y, x].ToString() + '\t');
                        }
                        writer.Write(Environment.NewLine);
                    }
                }
            }
        }

        public static StringBuilder MatrixToString(int[,] matrix)
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    sb.Append(matrix[y, x].ToString() + '\t');
                }
                sb.AppendLine();
            }
            return sb;
        }

        public static int[,] StringToMatrix(string str)
        {
            int[,] arr;
            List<List<int>> arrList = new List<List<int>>();
            string [] rowArray = str.Split('\n');
            int size = rowArray.Length - 1;
            arr = new int[size,size];
            for (int i = 0; i < size; i++)
            {
                string[] elements = rowArray[i].Split('\t');
                for (int j = 0; j < size; j++)
                {
                    arr[j, i] = int.Parse(elements[j]);
                }
            }
            return arr;
        }



    }
}
