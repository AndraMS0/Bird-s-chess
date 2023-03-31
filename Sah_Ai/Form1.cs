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
			Tabla tabla = new Tabla(this);
			string path = @"C:\Facultate_an_3\INTELIGENTA ARTIFICIALA\Proiect_sah_bun\Bird-s-chess\Sah_Ai\Images\tura.png";
			for (int i = 0; i <= 5; i++)
			{
				tabla.AddPieceToButton(path, 0, i);
			}
		}
	}
}
