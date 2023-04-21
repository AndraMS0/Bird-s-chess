using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sah_Ai
{
    public class Player
    {
        public string Name { get; set; }
        public Piece.PieceColor Color { get; set; }
        public Player(string name, Piece.PieceColor color)
        {
            Name = name;
            Color = color;
        }


    }
}
