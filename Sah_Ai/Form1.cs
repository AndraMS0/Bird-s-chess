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
	public partial class Form1 : Form
	{
	
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			
			Button network = new Button();
			network.Location = new Point(500, 102);
			network.Size = new Size(100, 30);
			network.Text = "Network";
			network.Visible = true;
			network.Click += new EventHandler(Network_Click);
			this.Controls.Add(network);
           
			
		}
		private void Network_Click(object sender, EventArgs e)
        {
			Button button = (Button)sender;
			NetworkGame tabla = new NetworkGame(this);
			button.Enabled = false;
			button.Visible = false;
			
		}




	}
}
