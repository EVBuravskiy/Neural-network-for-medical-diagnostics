namespace VisualInterface
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void inputImageToolStripMenuItemClick(object sender, EventArgs e)
        {
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                NeuralNetwork.ImageConverter imageConverter = new NeuralNetwork.ImageConverter();
                double[] inputSignals = imageConverter.Convert(fileName);
                double result = Program.SystemController.ImageNetwork.PredictResult(inputSignals).Output;
            }
        }

        private void intputDataToolStripMenuItemClick(object sender, EventArgs e)
        {
            EnterData enterData = new EnterData();
            bool? result = enterData.ShowForm();
            if(result.HasValue)
            {
                DiagnosticResult diagnosticResult = new DiagnosticResult(result.Value);
                diagnosticResult.ShowDialog();
            }
            result = null;
        }
    }
}