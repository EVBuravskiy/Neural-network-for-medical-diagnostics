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
    public class NeuralNetworkTests
    {
        [TestMethod()]
        public void FeedForwardNeuralNetworkTest()
        {
            double[,] inputsSignals = new double[,]
            {
                { 0, 0, 0, 0},
                { 0, 0, 0, 1},
                { 0, 0, 1, 0},
                { 0, 0, 1, 1},
                { 0, 1, 0, 0},
                { 0, 1, 0, 1},
                { 0, 1, 1, 0},
                { 0, 1, 1, 1},
                { 1, 0, 0, 0},
                { 1, 0, 0, 1},
                { 1, 0, 1, 0},
                { 1, 0, 1, 1},
                { 1, 1, 0, 0},
                { 1, 1, 0, 1},
                { 1, 1, 1, 0},
                { 1, 1, 1, 1},
            };
            double[] expectedResults = new double[] { 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1};
            TopologyOfNeuralNetwork topology = new TopologyOfNeuralNetwork(4, 1, 0.1, 2);
            NeuralNetwork neuralNetwork = new NeuralNetwork(topology);
            double difference = neuralNetwork.Learn(expectedResults, inputsSignals, 100000);
            List<double> results = new List<double>();
            for (int i = 0; i < expectedResults.Length; i++)
            {
                double[] input = NeuralNetwork.GetRow(inputsSignals, i);
                double result = neuralNetwork.PredictResult(input).Output;
                results.Add(result);
            }
            for(int i = 0; i < results.Count; i++)
            {
                double expectedResult = Math.Round(expectedResults[i], 2);
                double actualResult = Math.Round(results[i], 2);
                Assert.AreEqual(expectedResult, actualResult);
            }
        }

        [TestMethod()]
        public void DatasetTest()
        {
            List<double> expectedResults = new List<double>();
            List<double[]> inputSignals = new List<double[]>();
            using (StreamReader sr = new StreamReader("heart.csv"))
            {
                var header = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    var row = sr.ReadLine();
                    var valuesStr = row.Split(",");
                    List<double> values = valuesStr.Select(v => Convert.ToDouble(v.Replace(".", ","))).ToList();
                    double result = values.Last();
                    double[] inputs = values.Take(values.Count - 1).ToArray();
                    expectedResults.Add(result);
                    inputSignals.Add(inputs);
                }
            }

            TopologyOfNeuralNetwork topology = new TopologyOfNeuralNetwork(inputSignals[0].Length, 1, 0.1, inputSignals[0].Length / 2);
            NeuralNetwork neuralNetwork = new NeuralNetwork(topology);

            double[,] inputsSignals = new double[inputSignals.Count, inputSignals[0].Length];
            for (int i = 0; i < inputsSignals.GetLength(0); i++)
            {
                for (int j = 0; j < inputsSignals.GetLength(1); j++)
                {
                    inputsSignals[i, j] = inputSignals[i][j];
                }
            }

            double difference = neuralNetwork.Learn(expectedResults.ToArray(), inputsSignals, 100);

            List<double> results = new List<double>();
            for (int i = 0; i < expectedResults.Count; i++)
            {
                double result = neuralNetwork.PredictResult(inputSignals[i]).Output;
                results.Add(result);
            }

            for (int i = 0; i < results.Count; i++)
            {
                double expectedResult = Math.Round(expectedResults[i], 0);
                double actualResult = Math.Round(results[i], 0);
                Assert.AreEqual(expectedResult, actualResult);
            }
        }

        [TestMethod()]
        public void RecognizeImages()
        {
            string infectedPath = @"F:\Programming\С#\MedicalDataBase\malaria\Infected\";
            string unInfectedPath = @"F:\Programming\С#\MedicalDataBase\malaria\UnInfected\";

            ImageConverter imageConverter = new ImageConverter();
            double[] testImageInputInfected = imageConverter.Convert(infectedPath + "C33P1thinF_IMG_20150619_115808a_cell_206.png");
            double[] testImageInputUnInfected = imageConverter.Convert(unInfectedPath + "C1_thinF_IMG_20150604_104722_cell_79.png");


            TopologyOfNeuralNetwork topology = new TopologyOfNeuralNetwork(testImageInputInfected.Length, 1, 0.1, testImageInputInfected.Length / 2);
            NeuralNetwork neuralNetwork = new NeuralNetwork(topology);

            int size = 100;

            double[,] infectedInputsDoubleArray = GetData(infectedPath, imageConverter, testImageInputInfected, size);
            neuralNetwork.Learn(new double[] { 1.0 }, infectedInputsDoubleArray, 1);

            var unInfectedInputsDoubleArray = GetData(unInfectedPath, imageConverter, testImageInputInfected, size);
            neuralNetwork.Learn(new double[] { 0.0 }, unInfectedInputsDoubleArray, 1);


            double resultInfected = neuralNetwork.PredictResult(testImageInputInfected.Select(x => (double)x).ToArray()).Output;
            double resultUnInfected = neuralNetwork.PredictResult(testImageInputUnInfected.Select(x => (double)x).ToArray()).Output;

            Assert.AreEqual(1, Math.Round(resultInfected, 2));
            Assert.AreEqual(0, Math.Round(resultUnInfected, 2));
        }

        /// <summary>
        /// Get collection of data
        /// </summary>
        /// <param name = "path" ></ param >
        /// < param name="imageConverter"></param>
        /// <param name = "testImageInputInfected" ></ param >
        /// < param name="size"></param>
        /// <returns></returns>
        private static double[,] GetData(string path, ImageConverter imageConverter, double[] testImageInputInfected, int size)
        {
            var images = Directory.GetFiles(path);
            double[,] data = new double[size, testImageInputInfected.Length];
            for (int i = 0; i < size; i++)
            {
                double[] inputImage = imageConverter.Convert(images[i]);
                for (int j = 0; j < inputImage.Length; j++)
                {
                    data[i, j] = inputImage[j];
                }
            }
            return data;
        }
    }
}