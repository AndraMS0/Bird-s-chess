using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sah_Ai
{
    public class ChessSquare
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        public ChessSquare(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
