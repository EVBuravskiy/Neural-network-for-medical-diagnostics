using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualInterface
{
    public partial class DiagnosticResult : Form
    {
        public DiagnosticResult(bool result)
        {
            InitializeComponent();
            if(result)
            {
                labelResult.Text += ": имеется заболевание. Необходимо более тщательное исследование";
            }
            else
            {
                labelResult.Text += ": заболевания не выявлены.";
            }
        }

        private void buttonExitClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
