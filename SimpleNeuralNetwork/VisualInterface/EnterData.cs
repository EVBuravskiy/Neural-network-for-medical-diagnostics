using System.Reflection;

namespace VisualInterface
{
    public partial class EnterData : Form
    {
        /// <summary>
        /// TextBox Collection
        /// </summary>
        private List<TextBox> inputTextBoxes = new List<TextBox>();
        public EnterData()
        {
            InitializeComponent();
            var propertyInfo = typeof(Patient).GetProperties();
            for(int i = 0; i < propertyInfo.Length; i++)
            {
                TextBox textBox = CreateTextBox(i, propertyInfo[i]);
                Controls.Add(textBox);
                Label label = CreateLabel(i, propertyInfo[i]);
                Controls.Add(label);
                inputTextBoxes.Add(textBox);
            }
        }

        private void EnterDataLoad(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Create and display form
        /// </summary>
        /// <returns></returns>
        public bool? ShowForm()
        {
            EnterData newForm = new EnterData();
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                List<double> values = new List<double>();
                foreach (TextBox textBox in newForm.inputTextBoxes)
                {
                    if (double.TryParse(textBox.Text, out double value))
                    {
                        values.Add(value);
                    }
                }

                if (Program.SystemController.DataNetwork.IsLearned == false)
                {
                    Program.SystemController.LernNeuralNetwork();
                }

                var result = Program.SystemController.DataNetwork.PredictResult(values.ToArray()).Output;
                return result == 1.0 ? true : false;
            };
            return null;
        }

        /// <summary>
        /// Create a TextBox for the form
        /// </summary>
        /// <param name="number"></param>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private TextBox CreateTextBox(int number, PropertyInfo propertyInfo)
        {
            int y = number * 25;

            TextBox textBox = new TextBox();

            textBox.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            textBox.Location = new System.Drawing.Point(202, y);
            textBox.Name = "textBox" + propertyInfo.Name + number;
            textBox.Size = new System.Drawing.Size(586, y);
            textBox.TabIndex = number;
            textBox.Font = new Font("Times New Roman", 9.75F, FontStyle.Italic, GraphicsUnit.Point, ((byte)(204)));
            textBox.ForeColor = Color.Gray;
            textBox.Text = "Enter " + propertyInfo.Name;
            textBox.Tag = "Enter " + propertyInfo.Name;

            textBox.GotFocus += TextBoxGotFocus;
            textBox.LostFocus += TextBoxLostFocus;

            return textBox;
        }

        /// <summary>
        /// Event handler for when an element receives focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxGotFocus(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == textBox.Tag.ToString())
            {
                textBox.Text = "";
                textBox.Font = new Font("Times New Roman", 9.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
                textBox.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// Event handler for when an element loses focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxLostFocus(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if(textBox.Text == "")
            {
                textBox.Text = textBox.Tag.ToString();
                textBox.Font = new Font("Times New Roman", 9.75F, FontStyle.Italic, GraphicsUnit.Point, ((byte)(204)));
                textBox.ForeColor = Color.Gray;
            }
        }

        /// <summary>
        /// Method for creating a Label element for a form
        /// </summary>
        /// <param name="number"></param>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private Label CreateLabel(int number, PropertyInfo propertyInfo)
        {
            int y = number * 25 + 5;

            Label label = new Label();

            label.AutoSize = true;
            label.Location = new System.Drawing.Point(12, y);
            label.Name = "label" + number;
            label.Size = new System.Drawing.Size(38, 15);
            label.TabIndex = 1;
            label.Text = propertyInfo.Name;
            return label;
        }

        private void buttonOKClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
