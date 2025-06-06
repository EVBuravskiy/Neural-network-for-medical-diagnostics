namespace NeuralNetwork
{
    /// <summary>
    /// Neuron layer class
    /// </summary>
    public class LayerOfNeurons
    {
        /// <summary>
        /// List of neurons
        /// </summary>
        public List<Neuron> Neurons { get; set; }

        /// <summary>
        /// Number of neurons in a layer
        /// </summary>
        public int NeuronsCount => Neurons?.Count ?? 0;

        /// <summary>
        /// Type of neurons in a layer
        /// </summary>
        public NeuronType LayerType { get; set; }

        /// <summary>
        /// Neuron Layer Constructor
        /// </summary>
        /// <param name="neurons"></param>
        /// <param name="neuronType"></param>
        public LayerOfNeurons(List<Neuron> neurons, NeuronType neuronType = NeuronType.Hidden) 
        {
            Neurons = new List<Neuron>();
            Neurons.AddRange(neurons);
            LayerType = neuronType;
        }

        /// <summary>
        /// Receive signals from neurons
        /// </summary>
        /// <returns></returns>
        public List<double> GetSignals()
        {
            List<double> signals = new List<double>();
            foreach(Neuron neuron in Neurons)
            {
                signals.Add(neuron.Output);
            }
            return signals;
        }

        public override string ToString()
        {
            return LayerType.ToString();
        }
    }
}
