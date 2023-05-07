using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sah_Ai
{
    public partial class IPInput : Form
    {
        public string IP;
        public IPInput()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {

            IP = textBox1.Text;
            DialogResult = DialogResult.OK;
            this.Close();

        }
    }
}
