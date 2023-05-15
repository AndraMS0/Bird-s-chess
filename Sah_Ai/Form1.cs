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
		private Button _network;
		private Button _artificialIntelligence;
		private void Form1_Load(object sender, EventArgs e)
		{
			
		    _network = new Button();
			_network.Location = new Point(400, 102);
			_network.Size = new Size(100, 30);
			_network.Text = "Network";
			_network.Visible = true;
			_network.Click += new EventHandler(Network_Click);
			this.Controls.Add(_network);

			_artificialIntelligence = new Button();
			_artificialIntelligence.Location = new Point(400, 150);
			_artificialIntelligence.Size = new Size(100, 30);
			_artificialIntelligence.Text = "AI Game";
			_artificialIntelligence.Visible = true;
			_artificialIntelligence.Click += new EventHandler(AI_Click);
			this.Controls.Add(_artificialIntelligence);


		}
		private void Network_Click(object sender, EventArgs e)
        {
			Button button = (Button)sender;
			NetworkGame networkGame = new NetworkGame(this);
			button.Enabled = false;
			button.Visible = false;
			_artificialIntelligence.Enabled = false;
			_artificialIntelligence.Visible = false;
			
		}

		private void AI_Click(object sender, EventArgs e)
        {
			Button button = (Button)sender;
			AIGame aiGAme = new AIGame(this);
			button.Enabled = false;
			button.Visible = false;
			_network.Visible = false;
			_network.Enabled = false;

		}






	}
}
