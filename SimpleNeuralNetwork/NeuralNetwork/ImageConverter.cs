using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace NeuralNetwork
{
    /// <summary>
    /// Color to Black and White Conversion Class
    /// </summary>
    public class ImageConverter
    {
        /// <summary>
        /// Threshold value
        /// </summary>
        public int thresholdValue { get; set; } = 128;

        /// <summary>
        /// Image width
        /// </summary>
        public int imageWidth { get; set; }

        /// <summary>
        /// Image height
        /// </summary>
        public int imageHeight { get; set; }

        /// <summary>
        /// Convert image from file to pixel array
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public double[] Convert(string path)
        {
            List<double> result = new List<double>();

            Bitmap image = new Bitmap(path);

            Bitmap resizeImage = new Bitmap(image, new Size(50, 50));

            imageWidth = resizeImage.Width;
            imageHeight = resizeImage.Height;

            for(int y = 0; y < imageHeight; y++)
            {
                for(int x = 0; x < imageWidth; x++)
                {
                    var pixel = resizeImage.GetPixel(x, y);
                    int brightness = Brightness(pixel);
                    result.Add(brightness);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Convert color pixel to black and white
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        private int Brightness(Color pixel)
        {
            double brightness = 0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B;
            return brightness < thresholdValue ? 0 : 1;
        }

        /// <summary>
        /// Save image to file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixels"></param>
        public void SaveConvertPicture(string path, double[] pixels)
        {
            Bitmap newImage = new Bitmap(imageWidth, imageHeight);
            for (int y = 0; y < newImage.Height; y++)
            {
                for (int x = 0; x < newImage.Width; x++)
                {
                    Color color = pixels[y * imageWidth + x] == 1 ? Color.White : Color.Black;
                    newImage.SetPixel(x, y, color);
                }
            }
            newImage.Save(path);
        }
    }
}
