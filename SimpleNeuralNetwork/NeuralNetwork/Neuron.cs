namespace NeuralNetwork
{
    /// <summary>
    /// Neuron class
    /// </summary>
    public class Neuron
    {
        /// <summary>
        /// List of weights for input signals
        /// </summary>
        public List<double> Weights { get; set; }

        /// <summary>
        /// List of input signals
        /// </summary>
        public List<double> InputsSignals { get; set; }

        /// <summary>
        /// Neuron type (enum) 
        /// </summary>
        public NeuronType NeuronType { get; set; }

        /// <summary>
        /// Output signal
        /// </summary>
        public double Output { get; set; }

        /// <summary>
        /// Delta
        /// </summary>
        public double Delta { get; set; }

        /// <summary>
        /// Neuron class constructor
        /// </summary>
        public Neuron()
        {
            Weights = new List<double>();
            InputsSignals = new List<double>();
            NeuronType = NeuronType.Hidden;
            Output = 0;
        }

        /// <summary>
        /// Neuron class constructor with incoming signals and neuron's type
        /// </summary>

        public Neuron(int incomingSignals, NeuronType neuronType) : this()
        {
            NeuronType = neuronType;
            InitWeightsRandomValues(incomingSignals);
        }

        /// <summary>
        /// Initialize the weight
        /// </summary>
        /// <param name="incomingSignals"></param>
        private void InitWeightsRandomValues(int incomingSignals)
        {
            Random rnd = new Random();
            for (int i = 0; i < incomingSignals; i++)
            {
                if (NeuronType == NeuronType.Input)
                {
                    Weights.Add(1);
                }
                else
                {
                    Weights.Add(rnd.NextDouble());
                }
                InputsSignals.Add(0);
            }
        }

        /// <summary>
        /// Promotion signal
        /// </summary>
        /// <param name="incomingSignals"></param>
        /// <returns>double</returns>
        public double FeedForward(List<double> incomingSignals)
        {
            for(int i = 0; i < InputsSignals.Count; i++)
            {
                InputsSignals[i] = incomingSignals[i];
            }

            double sum = 0.0;
            if (incomingSignals.Count == Weights.Count) 
            {
                for(int i = 0; i < incomingSignals.Count; i++)
                {
                    sum += incomingSignals[i] * Weights[i];
                }
            }
            if (NeuronType != NeuronType.Input)
            {
                Output = Sigmoid(sum);
            }
            else
            {
                Output = sum;
            }
            return Output;
        }

        /// <summary>
        /// Sigmoid
        /// </summary>
        /// <param name="x"></param>
        /// <returns>double</returns>
        private double Sigmoid(double x)
        {
            return 1.0/(1.0 + Math.Pow(Math.E, -x));
        }

        /// <summary>
        /// Get the derivative of the sigmoid
        /// </summary>
        /// <param name="x"></param>
        /// <returns>double</returns>
        private double SigmoidDx(double x)
        {
            double sigmoid = Sigmoid(x);
            double result = sigmoid / (1 - sigmoid);
            return result;
        }

        /// <summary>
        /// Balance the signal's weight
        /// </summary>
        /// <param name="error"></param>
        /// <param name="learningRate"></param>
        public void BalanceeWeight(double error, double learningRate)
        {
            if(NeuronType == NeuronType.Input)
            {
                return;
            }
            Delta = error * SigmoidDx(Output);
            for(int i = 0; i < Weights.Count; i++)
            {
                double currentWeight = Weights[i];
                double inputSignal = InputsSignals[i];
                double newWeight = currentWeight - inputSignal * Delta * learningRate;
                Weights[i] = newWeight;
            }
        }

        public override string ToString()
        {
            return Output.ToString();
        }
    }
}