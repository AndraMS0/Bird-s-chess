using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sah_Ai
{
	public class Tabla
	{
		private TableLayoutPanel chessBoard;

		public Tabla(Form form)
		{
			chessBoard = new TableLayoutPanel();
			chessBoard.RowCount = 8;
			chessBoard.ColumnCount = 10;
			chessBoard.Dock = DockStyle.Fill;
			chessBoard.Margin = new Padding(2);

			// Set the row and column styles
			for (int i = 0; i < 8; i++)
			{
				chessBoard.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5f));
			}
			for (int j = 0; j < 10; j++)
			{
				chessBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));
			}

			// Add the chess board squares to the table layout panel
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					Panel square = new Panel();

					square.Dock = DockStyle.Fill;
					square.Size = new Size(60, 60);
					square.Padding = new Padding(5);
					square.BackColor = Color.Green;
					square.Visible = true;
					if ((i + j) % 2 == 0)
					{
						square.BackColor = Color.FromArgb(238, 238, 238);

					}
					else
					{
						square.BackColor = Color.FromArgb(119, 136, 153);

					}
					chessBoard.Controls.Add(square, j, i);

				}
			}

			// Add the table layout panel to the form
			form.Controls.Add(chessBoard);
		}
		public void AdaugaPiesa(int row, int col, string imageFileName)
		{
			// Get the panel that represents the square
			Panel square = (Panel)chessBoard.GetControlFromPosition(row, col);

			// Create a new PictureBox control and set its image
			PictureBox piece = new PictureBox();
			piece.Image = Image.FromFile(imageFileName);
			piece.SizeMode = PictureBoxSizeMode.Zoom;

			// Add the PictureBox control to the square
			square.Controls.Add(piece);
		}
	}
}
