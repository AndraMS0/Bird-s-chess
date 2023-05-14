using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sah_Ai
{
    public class Pawn : Piece
    {
        
        public Pawn(Image image, PieceColor color, ChessSquare square) : base(image, color, square)
        {
            this.Type = PieceType.Pawn;
        }

        public override int[] getOffsets(ChessSquare position, Game board, Button[,] buttons)
        {
            List<int> rowOffsets = new List<int>();
            List<int> colOffsets = new List<int>();

            // Check if pawn is white or black
            int direction = (this.color == PieceColor.White) ? -1 : 1;

            // Check if pawn can move one square forward
            ChessSquare square = new ChessSquare(position.Row + direction, position.Column);
            if (board.isValidSquare(square) && board.getPiece(square) == null)
            {
                // Add offset for one square forward
                rowOffsets.Add(direction);
                colOffsets.Add(0);

                // Color the button of the potential move
                Button[,] copyButtons = board.MyBoard.MyButtons;
                Button button = copyButtons[square.Row, square.Column];
                button.BackColor = Color.Green;

            }

            // Check if pawn can move two squares forward
            if ((position.Row == 1 && direction == 1) || (position.Row == 6 && direction == -1))
            {
                square = new ChessSquare(position.Row + 2 * direction, position.Column);
                if (board.isValidSquare(square) && board.getPiece(square) == null)
                {
                    // Add offset for two squares forward
                    rowOffsets.Add(2 * direction);

                    colOffsets.Add(0);

                    // Color the button of the potential move
                    Button[,] copyButtons = board.MyBoard.MyButtons;
                    Button button = copyButtons[square.Row, square.Column];
                    button.BackColor = Color.Green;
                }
            }

            // Check if pawn can capture diagonally
            int[] diagonalOffsets = new int[] { -1, 1 };
            foreach (int offset in diagonalOffsets)
            {
                square = new ChessSquare(position.Row + direction, position.Column + offset);
                if (board.isValidSquare(square) && board.getPiece(square) != null && board.getPiece(square).color != this.color)
                {
                    // Add offset for diagonal capture
                    rowOffsets.Add(direction);
                    colOffsets.Add(offset);

                    // Color the button of the potential move
                    Button[,] copyButtons = board.MyBoard.MyButtons;
                    Button button = copyButtons[square.Row, square.Column];
                    button.BackColor = Color.Red;
                }
            }

            // Combine the row and column offsets into a single array
            int[] offsets = new int[rowOffsets.Count * 2];
            for (int i = 0; i < rowOffsets.Count; i++)
            {
                offsets[i * 2] = rowOffsets[i];
                offsets[i * 2 + 1] = colOffsets[i];
            }
            int position_offset = 0;
            int row;
            int col;
            foreach (int offset in offsets)
            {
                if (position.Row + offset % 10 > position.Row && this.color == PieceColor.White)
                    continue;
                if (position.Row + offset % 10 < position.Row && this.color == PieceColor.Black)
                    continue;

                row = position.Row + offset % 10;
                col = position.Column + offset / 10;
                if (board.isValidSquare(new ChessSquare(row, col)) && board.getPiece(new ChessSquare(row, col)) == null)
                {
                    // Set color to green for potential move
                    buttons[row, col].BackColor = Color.Green;
                }
                position_offset++;
            }
            return offsets;
        }

    }
}
