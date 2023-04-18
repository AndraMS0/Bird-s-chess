﻿using System;
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
        public Piece currectPieceToMove;
        public int[] offsets;
        public int last_x;
        public int last_y;
        public int new_x;
        public int new_y;

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
            Image image = imageToButton(whitePawn, 6, 2);
            ChessSquare chessSquare = new ChessSquare(6, 2);
            Piece pawn = new Pawn(image, Piece.PieceColor.White,chessSquare);
            pieces[6, 2] = pawn;

            string blackPawn = @"C:\Users\andra\OneDrive\Desktop\an3\sah\Bird-s-chess\Sah_Ai\Images\pion2.png";
            Image image2 = imageToButton(blackPawn, 1, 1);
            ChessSquare chessSquare2 = new ChessSquare(1, 1);
            Piece pawn2 = new Pawn(image2, Piece.PieceColor.Black,chessSquare2);
            pieces[1, 1] = pawn2;

            
            Image image3 = imageToButton(blackPawn, 1, 3);
            ChessSquare chessSquare3 = new ChessSquare(1, 3);
            Piece pawn3 = new Pawn(image3, Piece.PieceColor.Black,chessSquare3);
            pieces[1, 3] = pawn3;
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
                    offsets = piece.getOffsets(square, this,buttons);
                    int row = square.Row;
                    int col = square.Column;
                    currectPieceToMove = pieces[row,col];
                    last_x = row;
                    last_y = col;
                    
                    
                }
            }
            if(clickedButton.BackColor == Color.Green)
            {
                int row = square.Row;
                int col = square.Column;
                new_x = row;
                new_y = col;
                Image image  = currectPieceToMove.Pieceimage;
                ChessSquare selectedPiecePosition = currectPieceToMove.Position;// aici e ciudat
                int r = selectedPiecePosition.Row;
                int c = selectedPiecePosition.Column;
                image = new Bitmap(image, clickedButton.Size);
                clickedButton.BackgroundImageLayout = ImageLayout.Center;
                clickedButton.BackgroundImage = image;
                ChessSquare sq = new ChessSquare(new_x,new_y);
                pieces[new_x,new_y] = pieces[r,c];
                pieces[new_x, new_y].Position = sq;
                //pieces[r,c].MyPosition = sq; // si mai ciudat, se schimba automat si currectPieceToMove
                pieces[r,c] = null;
                if ((r + c) % 2 == 0)
                {
                    buttons[r,c].BackColor = Color.White;
                    buttons[r,c].BackgroundImage = null;
                }
                else
                {
                  buttons[r,c].BackColor = Color.FromArgb(160, 160, 160);
                  buttons[r,c].BackgroundImage = null;
                }
                
               for (int i=0; i<offsets.Length-1;i+=2)
               {
                  int rc = last_x + offsets[i];
                  int cc = last_y + offsets[i+1];
                    if ((rc + cc) % 2 == 0)
                    {
                    buttons[rc,cc].BackColor = Color.White;
                    
                    }
                else
                   {
                    buttons[rc,cc].BackColor = Color.FromArgb(160, 160, 160);
                  
                   }

               }

            }
            if(clickedButton.BackColor == Color.Red)
            {
                int row = square.Row;
                int col = square.Column;
                pieces[row,col] = null;
                pieces[row,col] = pieces[last_x, last_y];
                pieces[last_x, last_y] = null;
                ChessSquare sq = new ChessSquare(row,col);
                pieces[row,col].Position = sq;

                buttons[row,col].BackgroundImage = null;
                Image image  = currectPieceToMove.Pieceimage;
                image = new Bitmap(image, clickedButton.Size);
                clickedButton.BackgroundImageLayout = ImageLayout.Center;
                clickedButton.BackgroundImage = image;
                //ChessSquare sq = new ChessSquare(row,col);
                if ((last_x + last_y) % 2 == 0)
                {
                    buttons[last_x,last_y].BackColor = Color.White;
                    buttons[last_x,last_y].BackgroundImage = null;
                }
                else
                {
                  buttons[last_x,last_y].BackColor = Color.FromArgb(160, 160, 160);
                  buttons[last_x,last_y].BackgroundImage = null;
                }
                for (int i=0; i<offsets.Length-1;i+=2)
                {
                  int rc = last_x + offsets[i];
                  int cc = last_y + offsets[i+1];
                    if ((rc + cc) % 2 == 0)
                    {
                    buttons[rc,cc].BackColor = Color.White;
                    
                    }
                else
                   {
                    buttons[rc,cc].BackColor = Color.FromArgb(160, 160, 160);
                  
                   }

                }


            }

        }
        
        public void play()
        {

        }
    }
}