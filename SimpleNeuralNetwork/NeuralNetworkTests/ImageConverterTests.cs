using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Tests
{
    [TestClass()]
    public class ImageConverterTests
    {
        [TestMethod()]
        public void ConvertTest()
        {
            string infectedPath = @"F:\Programming\С#\MedicalDataBase\malaria\Infected\";
            string unInfectedPath = @"F:\Programming\С#\MedicalDataBase\malaria\UnInfected\";
            string testPath = @"F:\Programming\С#\MedicalDataBase\test\";

            ImageConverter imageConverter = new ImageConverter();
            double[] testImageInputInfected = imageConverter.Convert(infectedPath + "C33P1thinF_IMG_20150619_115740a_cell_162.png");
            imageConverter.SaveConvertPicture(testPath + "infectedTest.png", testImageInputInfected);
            double[] testImageInputUnInfected = imageConverter.Convert(unInfectedPath + "C1_thinF_IMG_20150604_104722_cell_79.png");
            imageConverter.SaveConvertPicture(testPath  + "unInfectedTest.png", testImageInputUnInfected);
        }
    }
}