using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sah_Ai
{
    public class Board
    {
        //private TableLayoutPanel chessBoard;
        public Button[,] buttons = new Button[8, 10];
        public Piece[,] pieces = new Piece[8, 10];

        public static int SIZE { get; internal set; }

        public Board(Form form)
        {
            
            for(int i=0; i < 8; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    Button button = new Button();
                    button.Click += new EventHandler(Button_Click);
                    button.Size = new Size(70, 70);
                    button.Visible = true;
                    if ((i + j) % 2 == 0)
                    {
                        button.BackColor = Color.White;
                    }
                    else
                    {
                        button.BackColor = Color.FromArgb(160, 160, 160);
                    }
                    button.Location = new Point(j*70, i*70);
                    buttons[i, j] = button;
                  
                    form.Controls.Add(buttons[i, j]);
                }
                
            }
            string whitePawn = @"C:\Users\andra\OneDrive\Desktop\an3\sah\Bird-s-chess\Sah_Ai\Images\pion.png";
            Image image = imageToButton(whitePawn, 6, 0);
            ChessSquare chessSquare = new ChessSquare(6, 0);
            Piece pawn = new Pawn(image, Piece.PieceColor.White,chessSquare);
            pieces[6, 0] = pawn;

        }
        public bool isValidSquare(ChessSquare square)
        {
            // Check if row and column are within the bounds of the board
            return (square.Row >= 0 && square.Row < 8 && square.Column >= 0 && square.Column < 10);
        }
        public Piece getPiece(ChessSquare square)
        {
            // Check if square is valid
            if (!isValidSquare(square))
            {
                return null;
            }
            int row = square.Row;
            int column = square.Column;
            return pieces[row, column];
       
        }

        public Image imageToButton(string filePath, int row, int col)
        {
            Button button = buttons[row, col];
            Image image = Image.FromFile(filePath);

            // resize the image to fit the button
             image = new Bitmap(image, button.Size);

           // center the image in the button
            button.BackgroundImageLayout = ImageLayout.Center;

            // set the image as the button's background image
            button.BackgroundImage = image;
            return image;
        }
        public ChessSquare getSquare(Button button)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    if (buttons[row, col] == button)
                    {
                        return new ChessSquare(row, col);
                    }
                }
            }

            return null;
        }
        

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            
            ChessSquare square = getSquare(clickedButton);
            if (square != null)
            {
                Piece piece = getPiece(square);
             
                if (piece != null && piece.color == Piece.PieceColor.White)
                {
                   
                    clickedButton.BackColor = Color.Yellow;
                    int[] offsets = piece.getOffsets(square, this,buttons);
                    
                    
                }
            }

        }
        

        public void play()
        {

        }
    }
}
