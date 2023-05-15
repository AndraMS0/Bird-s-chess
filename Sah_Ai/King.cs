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
    class King : Piece
    {
        public override int score => 255;
        public King(Image image, PieceColor color, ChessSquare square) : base(image, color, square)
        {
            this.Type = PieceType.King;
        }
        public override int[] getOffsets(ChessSquare position, Game board, Button[,] buttons)
        {
            List<int> rowOffsets = new List<int>();
            List<int> colOffsets = new List<int>();

   
            int[,] kingOffsets = new int[,] { { -1, -1 }, { -1, 0 }, { -1, 1 },
                                      { 0, -1 },             { 0, 1 },
                                      { 1, -1 },  { 1, 0 },  { 1, 1 } };

           
            for (int i = 0; i < kingOffsets.GetLength(0); i++)
            {
                int row = position.Row + kingOffsets[i, 0];
                int col = position.Column + kingOffsets[i, 1];

                // Check if the new square is a valid one
                if (board.isValidSquare(new ChessSquare(row, col)))
                {
                    Piece piece = board.getPiece(new ChessSquare(row, col));

                    // Check if the square is empty or occupied by an opponent's piece
                    if (piece == null || piece.color != this.color)
                    {
                        rowOffsets.Add(row - position.Row);
                        colOffsets.Add(col - position.Column);

                        // Highlight the possible move on the UI
                        buttons[row, col].BackColor = (piece == null) ? Color.Green : Color.Red;
                    }
                }
            }

            // Combine the row and column offsets into a single array
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
