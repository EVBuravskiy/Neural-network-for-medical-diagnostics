using System.Data;

namespace NeuralNetwork
{
    /// <summary>
    /// Neural Network Class
    /// </summary>
    public class NeuralNetwork
    {
        /// <summary>
        /// Neural network topology
        /// </summary>
        public TopologyOfNeuralNetwork TopologyOfNeuralNetwork { get; private set; }

        /// <summary>
        /// List of neural network layers
        /// </summary>
        public List<LayerOfNeurons> ListOfLayers { get; set; }

        /// <summary>
        /// Neural network learning indicator
        /// </summary>
        public bool IsLearned { get; set; } = false;

        /// <summary>
        /// Neural Network Constructor
        /// </summary>
        /// <param name="topologyOfNeuralNetwork"></param>
        public NeuralNetwork(TopologyOfNeuralNetwork topologyOfNeuralNetwork)
        {
            TopologyOfNeuralNetwork = topologyOfNeuralNetwork;

            ListOfLayers = new List<LayerOfNeurons>();

            CreateInputLayer();
            CreateHiddenLayers();
            CreateOutputLayer();
        }

        //METHODS FOR CREATING NEURAL NETWORK LAYERS
        /// <summary>
        /// Create an input neural layer and add it to the neural network
        /// </summary>
        private void CreateInputLayer()
        {
            List<Neuron> inputNeurons = new List<Neuron>();
            for(int i = 0; i < TopologyOfNeuralNetwork.InputCount; i++)
            {
                Neuron newNeuron = new Neuron(1, NeuronType.Input);
                inputNeurons.Add(newNeuron);
            }
            LayerOfNeurons inputLayer = new LayerOfNeurons(inputNeurons, NeuronType.Input);
            ListOfLayers.Add(inputLayer);
        }

        /// <summary>
        /// Create hidden (internal) neural layers and add them to the neural network
        /// </summary>
        private void CreateHiddenLayers()
        {
            for (int j = 0; j < TopologyOfNeuralNetwork.HiddenLayers.Count; j++)
            {
                List<Neuron> neuronsInLayer = new List<Neuron>();
                LayerOfNeurons lastLayer = ListOfLayers.Last();
                for (int i = 0; i < TopologyOfNeuralNetwork.HiddenLayers[j]; i++)
                {
                    Neuron newNeuron = new Neuron(lastLayer.NeuronsCount, NeuronType.Hidden);
                    neuronsInLayer.Add(newNeuron);
                }
                LayerOfNeurons hiddenLayer = new LayerOfNeurons(neuronsInLayer, NeuronType.Hidden);
                ListOfLayers.Add(hiddenLayer);
            }
        }

        /// <summary>
        /// Create an output neural layer and add it to the neural network
        /// </summary>
        private void CreateOutputLayer()
        {
            List<Neuron> outputNeurons = new List<Neuron>();
            LayerOfNeurons lastLayer = ListOfLayers.Last();
            for (int i = 0; i < TopologyOfNeuralNetwork.OutputCount; i++)
            {
                Neuron newNeuron = new Neuron(lastLayer.NeuronsCount, NeuronType.Output);
                outputNeurons.Add(newNeuron);
            }
            LayerOfNeurons outputLayer = new LayerOfNeurons(outputNeurons, NeuronType.Output);
            ListOfLayers.Add(outputLayer);
        }


        //METHODS FOR PASSING DATA THROUGH NEURAL NETWORK LAYERS
        /// <summary>
        /// Pass the input data through the layer
        /// </summary>
        /// <param name="inputSignals"></param>
        /// <returns></returns>
        public Neuron PredictResult(params double[] inputSignals)
        {
            if (inputSignals.Length != ListOfLayers[0].NeuronsCount)
            {
                throw new ArgumentException("The number of input signals does not match the number of neuronsв", nameof(inputSignals));
            }
            SendSignalsToInputNeurons(inputSignals);
            FeedForwardAllLayersAfterInput();
            if (TopologyOfNeuralNetwork.OutputCount == 1)
            {
                return ListOfLayers.Last().Neurons[0];
            }
            else
            {
                return ListOfLayers.Last().Neurons.OrderByDescending(n => n.Output).First();
            }
        }

        /// <summary>
        /// Send data to the input layer of the neural network
        /// </summary>
        /// <param name="inputSignals"></param>
        private void SendSignalsToInputNeurons(params double[] inputSignals)
        {
            for (int i = 0; i < inputSignals.Length; i++)
            {
                List<double> signal = new List<double>() { inputSignals[i] };
                Neuron neuron = ListOfLayers[0].Neurons[i];
                neuron.FeedForward(signal);
            }
        }

        /// <summary>
        /// Send data to the other layers of the neural network
        /// </summary>
        private void FeedForwardAllLayersAfterInput()
        {
            for (int i = 1; i < ListOfLayers.Count; i++)
            {
                List<double> previousLayerSignals = ListOfLayers[i - 1].GetSignals();
                LayerOfNeurons layer = ListOfLayers[i];
                foreach (Neuron neuron in layer.Neurons)
                {
                    neuron.FeedForward(previousLayerSignals);
                }
            }
        }

        /// <summary>
        /// Learn from a dataset
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="numberOfEpochs"></param>
        /// <returns></returns>
        public double Learn(double[] expectedResults, double[,] inputsSignals, int numberOfEpochs)
        {
            var normalizationSignals = SignalNormalization(inputsSignals);
            var signalsScalling = SignalScaling(normalizationSignals);
            double error = 0.0;
            for (int i = 0; i < numberOfEpochs; i++)
            {
                for (int j = 0; j < expectedResults.Length; j++)
                {
                    double expectedResult = expectedResults[j];
                    double[] signals = GetRow(signalsScalling, j);
                    error += BackpropagationOfError(expectedResult, signals);
                }
            }
            IsLearned = true;
            return error / numberOfEpochs;
        }

        /// <summary>
        /// Get row from signals table
        /// </summary>
        /// <param name="inputsSignal"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static double[] GetRow(double[,] inputsSignal, int row)
        {
            int columns = inputsSignal.GetLength(1);
            double[] result = new double[columns];
            for(int i = 0; i < columns; i++)
            {
                result[i] = inputsSignal[row, i];
            }
            return result;
        }

        /// <summary>
        /// Backpropagation of error
        /// </summary>
        /// <param name="expectedResult"></param>
        /// <param name="inputSignals"></param>
        /// <returns></returns>
        private double BackpropagationOfError(double expectedResult, params double[] inputSignals)
        {
            Neuron actualNeuron = PredictResult(inputSignals);
            double actualResult = actualNeuron.Output;
            double lastDifference = actualResult - expectedResult;
            foreach(Neuron neuron in ListOfLayers.Last().Neurons)
            {
                neuron.BalanceeWeight(lastDifference, TopologyOfNeuralNetwork.LearningRate);
            }
            for(int j = ListOfLayers.Count - 2; j >= 0; j--)
            {
                LayerOfNeurons currentLayer = ListOfLayers[j];
                LayerOfNeurons previousLayer = ListOfLayers[j + 1];
                for(int i = 0; i < currentLayer.NeuronsCount; i++)
                {
                    Neuron currentNeuron = currentLayer.Neurons[i];
                    for (int k = 0; k < previousLayer.NeuronsCount; k++)
                    {
                        Neuron previousNeuron = previousLayer.Neurons[k];
                        double internalDifference = previousNeuron.Weights[i] * previousNeuron.Delta;
                        currentNeuron.BalanceeWeight(internalDifference, TopologyOfNeuralNetwork.LearningRate);
                    }
                }
            }
            return Math.Pow(lastDifference, 2);
        }

        /// <summary>
        /// Scale the signal
        /// </summary>
        /// <param name="inputsSignal"></param>
        /// <returns></returns>
        private double[,] SignalScaling(double[,] inputsSignal)
        {
            double[,] result = new double[inputsSignal.GetLength(0), inputsSignal.GetLength(1)];
            for(int columns = 0; columns < inputsSignal.GetLength(1); columns++)
            {
                double min = inputsSignal[0, columns];
                double max = inputsSignal[0, columns];
                for(int row = 1; row < inputsSignal.GetLength(0); row++)
                {
                    double element = inputsSignal[row, columns];
                    if (min > element)
                    {
                        min = element;
                    }
                    if (max < element)
                    {
                        max = element;
                    }
                }
                double divider = max - min;
                for (int row = 1; row < inputsSignal.GetLength(0); row++)
                {
                    result[row, columns] = (inputsSignal[row, columns] - min) / divider;
                }
            }
            return result;
        }

        /// <summary>
        /// Normalize the signal
        /// </summary>
        /// <param name="inputsSignal"></param>
        /// <returns></returns>
        private double[,] SignalNormalization(double[,] inputsSignal)
        {
            double[,] result = new double[inputsSignal.GetLength(0), inputsSignal.GetLength(1)];
            for(int column = 0; column < inputsSignal.GetLength(1); column++)
            {
                double sum = 0.0;
                for(int row = 1; row < inputsSignal.GetLength(0); row++)
                {
                    sum += inputsSignal[row, column];
                }
                double average = sum / inputsSignal.GetLength(0);
                double error = 0.0;
                for (int row = 1; row < inputsSignal.GetLength(0); row++)
                {
                    error += Math.Pow(inputsSignal[row, column] - average, 2);
                }
                double standardDeviation = Math.Sqrt(error / inputsSignal.GetLength(0));
                for (int row = 1; row < inputsSignal.GetLength(0); row++)
                {
                    result[row, column] = (inputsSignal[row, column] - average) / standardDeviation;
                }
            }
            return result;
        }
    }
}
