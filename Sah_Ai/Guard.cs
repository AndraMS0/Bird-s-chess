using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sah_Ai
{
	class Guard : Piece
	{
		public Guard(Image image, PieceColor color, ChessSquare square) : base(image, color, square)
		{
			this.Type = PieceType.Guard;
		}
        public override int[] getOffsets(ChessSquare position, Game board, Button[,] buttons)
        {
            List<int> rowOffsets = new List<int>();
            List<int> colOffsets = new List<int>();

            // Rook offsets
            int[,] rookOffsets = new int[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
            for (int i = 0; i < rookOffsets.GetLength(0); i++)
            {
                int row = position.Row + rookOffsets[i, 0];
                int col = position.Column + rookOffsets[i, 1];
                while (board.isValidSquare(new ChessSquare(row, col)))
                {
                    if (board.getPiece(new ChessSquare(row, col)) == null)
                    {
                        rowOffsets.Add(row - position.Row);
                        colOffsets.Add(col - position.Column);
                        buttons[row, col].BackColor = Color.Green;
                    }
                    else if (board.getPiece(new ChessSquare(row, col)).color != this.color)
                    {
                        rowOffsets.Add(row - position.Row);
                        colOffsets.Add(col - position.Column);
                        buttons[row, col].BackColor = Color.Red;
                        break;
                    }
                    else
                    {
                        break;
                    }
                    row += rookOffsets[i, 0];
                    col += rookOffsets[i, 1];
                }
            }

            // Knight offsets
            int[,] knightOffsets = new int[,] { { -2, -1 }, { -2, 1 }, { -1, -2 }, { -1, 2 }, { 1, -2 }, { 1, 2 }, { 2, -1 }, { 2, 1 } };
            for (int i = 0; i < knightOffsets.GetLength(0); i++)
            {
                int row = position.Row + knightOffsets[i, 0];
                int col = position.Column + knightOffsets[i, 1];
                if (board.isValidSquare(new ChessSquare(row, col)))
                {
                    if (board.getPiece(new ChessSquare(row, col)) == null || board.getPiece(new ChessSquare(row, col)).color != this.color)
                    {
                        rowOffsets.Add(row - position.Row);
                        colOffsets.Add(col - position.Column);

                        if (board.getPiece(new ChessSquare(row, col)) == null)
                        {
                            buttons[row, col].BackColor = Color.Green;
                        }
                        else
                        {
                            buttons[row, col].BackColor = Color.Red;
                        }
                    }
                }
            }

            int[] offsets = new int[rowOffsets.Count * 2];
            for (int i = 0; i < rowOffsets.Count; i++)
            {
                offsets[2 * i] = rowOffsets[i];
                offsets[2 * i + 1] = colOffsets[i];
            }

            return offsets;
        }
    }
}
