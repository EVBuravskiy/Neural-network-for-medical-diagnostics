using NeuralNetwork;

namespace VisualInterface
{
    /// <summary>
    /// Controller for working with a neural network
    /// </summary>
    public class SystemController
    {
        /// <summary>
        /// Neural network for working with data for diagnosing heart disease
        /// </summary>
        public NeuralNetwork.NeuralNetwork DataNetwork { get; }

        /// <summary>
        /// Neural network for working with image data
        /// </summary>
        public NeuralNetwork.NeuralNetwork ImageNetwork { get; }

        /// <summary>
        /// Constructor that initializes neural networks
        /// </summary>
        public SystemController()
        {
            NeuralNetwork.TopologyOfNeuralNetwork dataTopology = new NeuralNetwork.TopologyOfNeuralNetwork(13, 1, 0.1, 6);
            DataNetwork = new NeuralNetwork.NeuralNetwork(dataTopology);
            
            NeuralNetwork.TopologyOfNeuralNetwork imageTopology = new NeuralNetwork.TopologyOfNeuralNetwork(150, 1, 0.1, 75);
            ImageNetwork = new NeuralNetwork.NeuralNetwork(imageTopology);
        }

        /// <summary>
        /// Neural network training
        /// </summary>
        public void LernNeuralNetwork ()
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

            double[,] inputsSignals = new double[inputSignals.Count, inputSignals[0].Length];
            for (int i = 0; i < inputsSignals.GetLength(0); i++)
            {
                for (int j = 0; j < inputsSignals.GetLength(1); j++)
                {
                    inputsSignals[i, j] = inputSignals[i][j];
                }
            }

            double difference = DataNetwork.Learn(expectedResults.ToArray(), inputsSignals, 1000);

            List<double> results = new List<double>();
            for (int i = 0; i < expectedResults.Count; i++)
            {
                double result = DataNetwork.PredictResult(inputSignals[i]).Output;
                results.Add(result);
            }
        }
    }
}
