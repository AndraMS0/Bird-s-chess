using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sah_Ai.Piece;

namespace Sah_Ai
{
    class Queen : Piece
    {
        public Queen(Image image, PieceColor color, ChessSquare square) : base(image, color, square)
        {
            this.Type = PieceType.Queen;
        }
        public override int[] getOffsets(ChessSquare position, Board board, Button[,] buttons)
        {
            List<int> rowOffsets = new List<int>();
            List<int> colOffsets = new List<int>();

            // Rook-like moves
            int[,] horizontalVerticalOffsets = new int[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
            for (int i = 0; i < horizontalVerticalOffsets.GetLength(0); i++)
            {
                int row = position.Row + horizontalVerticalOffsets[i, 0];
                int col = position.Column + horizontalVerticalOffsets[i, 1];
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

                    row += horizontalVerticalOffsets[i, 0];
                    col += horizontalVerticalOffsets[i, 1];
                }
            }

            // Bishop-like moves
            int[,] diagonalOffsets = new int[,] { { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };
            for (int i = 0; i < diagonalOffsets.GetLength(0); i++)
            {
                int row = position.Row + diagonalOffsets[i, 0];
                int col = position.Column + diagonalOffsets[i, 1];
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

                    row += diagonalOffsets[i, 0];
                    col += diagonalOffsets[i, 1];
                }
            }

            int[] offsets = new int[rowOffsets.Count * 2];
            for (int i = 0; i < rowOffsets.Count; i++)
            {
                offsets[i * 2] = rowOffsets[i];
                offsets[i * 2 + 1] = colOffsets[i];
            }

            return offsets;
        }
    }
}
