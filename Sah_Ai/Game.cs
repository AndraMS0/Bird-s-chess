using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sah_Ai
{
    public class Game
    {
        private Board board;
        private Queue<ChessSquare> yellow_buttons = new Queue<ChessSquare>();
        private Dictionary<Button, int[]> buttonOfssetsCorrelation = new Dictionary<Button, int[]>();
        private Player whitePlayer;
        private Player blackPlayer;
        private Player currentPlayer;

        public Board MyBoard
        {
            get { return board; }
            set { board = value; }
        }

        public Queue<ChessSquare> MyYellow_buttons
        {
            get { return yellow_buttons; }
            set { yellow_buttons = value; }
        }

        public Dictionary<Button, int[]> MyButtonOfssetsCorrelation
        {
            get { return buttonOfssetsCorrelation; }
            set { buttonOfssetsCorrelation = value; }
        }
        public Player MyWhitePlayer
        {
            get { return whitePlayer; }
            set { whitePlayer = value; }
        }
         public Player MyBlackPlayer
         {
            get { return blackPlayer; }
            set { blackPlayer = value; }
         }
        public Player MyCurrentPlayer
        {
            get { return currentPlayer; }
            set
            {
                currentPlayer = value;
            }
        }


        public Game(Form form)
        {
            board = new Board(form);
        }
        public bool IsCheck(ChessSquare square, ref Piece.PieceColor kingColor)
        {
            var piece = getPiece(square);
            if(piece!= null && piece.Type == Piece.PieceType.King)
            {
                kingColor = piece.color;
                return true;
            }
            return false;
        }
        public void refreshBoard(int row, int col, int[] offset_array)
        {
            if ((row + col) % 2 == 0)
            {
                board.MyButtons[row, col].BackColor = Color.White;
                board.MyButtons[row, col].BackgroundImage = null;
            }
            else
            {
                board.MyButtons[row, col].BackColor = Color.FromArgb(160, 160, 160);
                board.MyButtons[row, col].BackgroundImage = null;
            }

            for (int i = 0; i < offset_array.Length - 1; i += 2)
            {
                int last_row = row + offset_array[i];
                int last_col = col + offset_array[i + 1];
                if ((last_row + last_col) % 2 == 0)
                {
                    board.MyButtons[last_row, last_col].BackColor = Color.White;

                }
                else
                {
                    board.MyButtons[last_row, last_col].BackColor = Color.FromArgb(160, 160, 160);

                }

            }

        }

        public bool isValidSquare(ChessSquare square)
        {

            return (square.Row >= 0 && square.Row < 8 && square.Column >= 0 && square.Column < 10);
        }

        public Piece getPiece(ChessSquare square)
        {

            if (!isValidSquare(square))
            {
                return null;
            }
            int row = square.Row;
            int column = square.Column;
            return board.MyPieces[row, column];

        }

        public ChessSquare getSquare(Button button)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    if (board.MyButtons[row, col] == button)
                    {
                        return new ChessSquare(row, col);
                    }
                }
            }

            return null;
        }
        public void VerifyKingState()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 10; j++)
                {
                    ChessSquare sq = new ChessSquare(i, j);
                    Piece currentPiece = getPiece(sq);
                    if (MyBoard.MyButtons[i, j].BackColor == Color.Red && currentPiece.Type == Piece.PieceType.King)
                    {
                        Piece.PieceColor pieceColor = currentPiece.color;
                        MessageBox.Show($"The {pieceColor} King is in Check");
                        
                     
                    }
                }
        }

        public void clear_old_selection()
        {
            while (yellow_buttons.Count > 1)
            {
                ChessSquare chessSq = yellow_buttons.Peek();

                if ((chessSq.Row + chessSq.Column) % 2 == 0)
                {
                    board.MyButtons[chessSq.Row, chessSq.Column].BackColor = Color.White;

                }
                else
                {
                    board.MyButtons[chessSq.Row, chessSq.Column].BackColor = Color.FromArgb(160, 160, 160);

                }
                int[] current_offsets;
                if (buttonOfssetsCorrelation.TryGetValue(board.MyButtons[chessSq.Row, chessSq.Column], out current_offsets))
                {

                    for (int i = 0; i < current_offsets.Length - 1; i += 2)
                    {
                        int last_r = chessSq.Row + current_offsets[i];
                        int last_c = chessSq.Column + current_offsets[i + 1];
                        if ((last_r + last_c) % 2 == 0)
                        {
                            board.MyButtons[last_r, last_c].BackColor = Color.White;

                        }
                        else
                        {
                            board.MyButtons[last_r, last_c].BackColor = Color.FromArgb(160, 160, 160);

                        }

                    }
                }
                yellow_buttons.Dequeue();


            }
        }
        public void switch_control(ChessSquare current_type)
        {
            Piece currentPiece = getPiece(current_type);
            if (currentPiece.color == Piece.PieceColor.White)
            {
                currentPlayer = blackPlayer;
            }
            else
            {
                currentPlayer = whitePlayer;
            }
        }

        public void DoMove(ChessSquare new_Position, ChessSquare old_position, int[] offsets_toMove)
        {
            Piece currentPiece = getPiece(old_position);
            board.MyPieces[new_Position.Row, new_Position.Column] = board.MyPieces[old_position.Row, old_position.Column];
            board.MyPieces[old_position.Row, old_position.Column] = null;
            board.MyPieces[new_Position.Row, new_Position.Column].Position = new_Position;

            Image image = currentPiece.Pieceimage;
            image = new Bitmap(image, board.MyButtons[new_Position.Row, new_Position.Column].Size);
            board.MyButtons[new_Position.Row, new_Position.Column].BackgroundImageLayout = ImageLayout.Center;
            board.MyButtons[new_Position.Row, new_Position.Column].BackgroundImage = image;
            switch_control(new_Position);
            refreshBoard(old_position.Row, old_position.Column, offsets_toMove);
        }
    }
}
