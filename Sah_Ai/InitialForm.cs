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
    public partial class InitialForm : Form
    {
        public InitialForm()
        {
            InitializeComponent();
            Network.Click += Button_click;
        }
        private void Button_click(object sender, EventArgs args)
        {
            var pressedButton = (Button)sender;
            //Form formToClose = Application.OpenForms["InitialForm"];
            //Form1 window = null;
             if (pressedButton.Name =="Network")
             {
                var form1 = new Form1();
                form1.Show();
                //form1.Show();


             }

            this.Close();



        }

        private void InitialForm_Load(object sender, EventArgs e)
        {

        }
    }
}
