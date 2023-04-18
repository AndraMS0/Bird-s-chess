using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sah_Ai
{
    public abstract class Piece
    {
        public enum PieceType { King, Queen, Bishop, Knight, Rook, Pawn };
        public enum PieceColor {White, Black};
        public PieceType Type { get; protected set; }
        public PieceColor color { get; private set; }
        public ChessSquare Position;
        public Image Pieceimage { get; set; }
        public ChessSquare MyPosition
        {
          get
          {
            return Position;
          }
          set
            {
            Position  = value;
           }
        }

        public Piece(Image image, PieceColor color, ChessSquare square)
        {
            this.color = color;
            this.Pieceimage = image;
            this.Position = square;
        }
        public abstract int[] getOffsets(ChessSquare position, Board board, Button[,] buttons);
        
    }
}
