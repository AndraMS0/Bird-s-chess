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
            chessBoard.Anchor = AnchorStyles.None;
            chessBoard.Padding = chessBoard.Padding = new Padding(form.ClientSize.Width / 10, form.ClientSize.Width / 20, 0, 0);
            chessBoard.Size = new Size((int)(form.ClientSize.Width * 0.9), (int)(form.ClientSize.Height * 0.9));


            for (int i = 0; i < 8; i++)
            {
                chessBoard.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5f));
            }
            for (int j = 0; j < 10; j++)
            {
                chessBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));
            }


            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Button square = new Button();
                    square.Dock = DockStyle.Fill;
                    square.Size = new Size(70, 70);
                    square.Margin = new Padding(0);
                    square.Padding = new Padding(0);
                    square.Visible = true;
                    square.Click += Square_Click;
                    if ((i + j) % 2 == 0)
                    {
                        square.BackColor = Color.White;
                    }
                    else
                    {
                        square.BackColor = Color.FromArgb(160, 160, 160);
                    }
                    chessBoard.Controls.Add(square, j, i);
                }
            }


            form.Controls.Add(chessBoard);
        }

        private void Square_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int currentRow = chessBoard.GetRow(clickedButton);
            int currentCol = chessBoard.GetColumn(clickedButton);
            int nextRow = currentRow + 1;

            if (nextRow < 8)
            {
                Button nextButton = (Button)chessBoard.GetControlFromPosition(currentCol, nextRow);

                if (nextButton.BackgroundImage != null)
                {
                    return;
                }
                else
                {
                    nextButton.BackgroundImage = clickedButton.BackgroundImage;
                    clickedButton.BackgroundImage = null;
                }
            }
        }

        public void AddPieceToButton(string filePath, int row, int col)
        {
            Button button = (Button)chessBoard.GetControlFromPosition(col, row);
            Image image = Image.FromFile(filePath);

            // resize the image to fit the button
            image = new Bitmap(image, button.Size);

            // center the image in the button
            button.BackgroundImageLayout = ImageLayout.Center;

            // set the image as the button's background image
            button.BackgroundImage = image;
        }
    }
}
