namespace NeuralNetwork
{
    /// <summary>
    /// Topology for neural network
    /// </summary>
    public class TopologyOfNeuralNetwork
    {
        /// <summary>
        /// Number of inputs to a neural network
        /// </summary>
        public int InputCount { get; set; }

        /// <summary>
        /// Number of outputs from a neural network
        /// </summary>
        public int OutputCount { get; set; }

        /// <summary>
        /// Learning rate factor
        /// </summary>
        public double LearningRate { get; set; }

        /// <summary>
        /// List of hidden neural layers
        /// </summary>
        public List<int> HiddenLayers { get; set; }

        /// <summary>
        /// Neural Network Topology Constructor
        /// </summary>
        /// <param name="inputCount"></param>
        /// <param name="outputCout"></param>
        /// <param name="layers"></param>
        public TopologyOfNeuralNetwork(int inputCount, int outputCout, double learningRate, params int[] layers)
        {
            InputCount = inputCount;
            OutputCount = outputCout;
            HiddenLayers = new List<int>();
            HiddenLayers.AddRange(layers);
            LearningRate = learningRate;
        }
    }
}
