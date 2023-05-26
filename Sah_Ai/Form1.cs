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
		private Button depth3;
		private Button depth2;
		private Button depth1;
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
			depth1 = new Button();
			depth1.Location = new Point(400, 200);
			depth1.Size = new Size(100, 30);
			depth1.Text = "Select depth 1";
			depth1.Visible = true;
			depth1.Click += new EventHandler(Depth1_Click);
			this.Controls.Add(depth1);



			depth2 = new Button();
			depth2.Location = new Point(400, 250);
			depth2.Size = new Size(100, 30);
			depth2.Text = "Select depth 2";
			depth2.Visible = true;
			depth2.Click += new EventHandler(Depth2_Click);
			this.Controls.Add(depth2);


			depth3 = new Button();
			depth3.Location = new Point(400, 300);
			depth3.Size = new Size(100, 30);
			depth3.Text = "Select depth 3";
			depth3.Visible = true;
			depth3.Click += new EventHandler(Depth3_Click);
			this.Controls.Add(depth3);

			Button button = (Button)sender;
			button.Enabled = false;
			button.Visible = false;
			_network.Visible = false;
			_network.Enabled = false;

		}

		private void Depth1_Click(object sender, EventArgs e) {
			AIGame AiGame = new AIGame(this);
			AiGame.SetDepth(1);
			Button button = (Button)sender;
			button.Enabled = false;
			button.Visible = false;
			depth2.Enabled = false;
			depth2.Visible = false;
			depth3.Enabled = false;
			depth3.Visible = false;
			



		}

		private void Depth2_Click(object sender, EventArgs e)
		{
			AIGame AiGame = new AIGame(this);
			AiGame.SetDepth(2);
			Button button = (Button)sender;
			button.Enabled = false;
			button.Visible = false;
			depth1.Enabled = false;
			depth1.Visible = false;
			depth3.Enabled = false;
			depth3.Visible = false;



		}

		private void Depth3_Click(object sender, EventArgs e)
		{
			AIGame AiGame = new AIGame(this);
			AiGame.SetDepth(3);
			Button button = (Button)sender;
			button.Enabled = false;
			button.Visible = false;
			depth1.Enabled = false;
			depth1.Visible = false;
			depth2.Enabled = false;
			depth2.Visible = false;



		}




	}
}
