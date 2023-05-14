using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sah_Ai
{
    public class Board
    {
        private Button[,] _buttons = new Button[8, 10];
        private Piece[,] _pieces = new Piece[8, 10];

        public Button[,] MyButtons
        {
            get
            {
                return _buttons;
            }
            set
            {
                _buttons = value;
            }
        }

        public Piece[,] MyPieces
        {
            get
            {
                return _pieces;
            }
            set
            {
                _pieces = value;
            }
        }
        public Board(Form form)
        {
         
        }
        public void CreateAllPieces()
        {
            String folderpath = @"..\..\Images\";

            for (int i = 0; i < 10; i++)
            {
                CreatePiece(Piece.PieceType.Pawn, Piece.PieceColor.White, 6, i, folderpath + "pion.png");
            }
        
            CreatePiece(Piece.PieceType.Bishop, Piece.PieceColor.White, 7, 2, folderpath + "nebun.png");
            CreatePiece(Piece.PieceType.Bishop, Piece.PieceColor.White, 7, 7, folderpath + "nebun.png");
            
            CreatePiece(Piece.PieceType.Rook, Piece.PieceColor.White, 7, 0, folderpath + "tura.png");
            CreatePiece(Piece.PieceType.Rook, Piece.PieceColor.White, 7, 9, folderpath + "tura.png");
            
            CreatePiece(Piece.PieceType.Knight, Piece.PieceColor.White, 7, 1, folderpath + "cal.png");
            CreatePiece(Piece.PieceType.Knight, Piece.PieceColor.White, 7, 8, folderpath + "cal.png");
           
            CreatePiece(Piece.PieceType.Queen, Piece.PieceColor.White, 7, 4, folderpath + "regina.png");
            
            CreatePiece(Piece.PieceType.King, Piece.PieceColor.White, 7, 5, folderpath + "rege.png");
            
            CreatePiece(Piece.PieceType.Guard, Piece.PieceColor.White, 7, 3, folderpath + "guard.png");
            
            CreatePiece(Piece.PieceType.Equery, Piece.PieceColor.White, 7, 6, folderpath + "equery.png");
            
            for (int i = 0; i < 10; i++)
            {
                CreatePiece(Piece.PieceType.Pawn, Piece.PieceColor.Black, 1, i, folderpath + "pion2.png");
            }
           
            CreatePiece(Piece.PieceType.Bishop, Piece.PieceColor.Black, 0, 2, folderpath + "nebun2.png");
            CreatePiece(Piece.PieceType.Bishop, Piece.PieceColor.Black, 0, 7, folderpath + "nebun2.png");
            
            CreatePiece(Piece.PieceType.Rook, Piece.PieceColor.Black, 0, 0, folderpath + "tura2.png");
            CreatePiece(Piece.PieceType.Rook, Piece.PieceColor.Black, 0, 9, folderpath + "tura2.png");
           
            CreatePiece(Piece.PieceType.Knight, Piece.PieceColor.Black, 0, 1, folderpath + "cal2.png");
            CreatePiece(Piece.PieceType.Knight, Piece.PieceColor.Black, 0, 8, folderpath + "cal2.png");
            
            CreatePiece(Piece.PieceType.Queen, Piece.PieceColor.Black, 0, 4, folderpath + "regina2.png");
            
            CreatePiece(Piece.PieceType.King, Piece.PieceColor.Black, 0, 5, folderpath + "rege2.png");
           
            CreatePiece(Piece.PieceType.Guard, Piece.PieceColor.Black, 0, 3, folderpath + "guard2.png");
            
            CreatePiece(Piece.PieceType.Equery, Piece.PieceColor.Black, 0, 6, folderpath + "equery2.png");
        }
        private void CreatePiece(Piece.PieceType type, Piece.PieceColor color, int row, int col, string imagePath)
        {

            Image image = imageToButton(imagePath, row, col);
            ChessSquare chessSquare = new ChessSquare(row, col);

            Piece piece = null;

            switch (type)
            {
                case Piece.PieceType.Pawn:
                    piece = new Pawn(image, color, chessSquare);
                    break;
                case Piece.PieceType.Knight:
                    piece = new Knight(image, color, chessSquare);
                    break;
                case Piece.PieceType.Bishop:
                    piece = new Bishop(image, color, chessSquare);
                    break;
                case Piece.PieceType.Rook:
                    piece = new Rook(image, color, chessSquare);
                    break;
                case Piece.PieceType.Queen:
                    piece = new Queen(image, color, chessSquare);
                    break;
                case Piece.PieceType.King:
                    piece = new King(image, color, chessSquare);
                    break;
                case Piece.PieceType.Guard:
                    piece = new Guard(image, color, chessSquare);
                    break;
                case Piece.PieceType.Equery:
                    piece = new Equery(image, color, chessSquare);
                    break;
            }

            if (piece != null)
            {
                _pieces[row, col] = piece;
            }
        }

        private Image imageToButton(string filePath, int row, int col)
        {
            Button button = _buttons[row, col];
            Image image = Image.FromFile(filePath);

            image = new Bitmap(image, button.Size);

            button.BackgroundImageLayout = ImageLayout.Center;

            button.BackgroundImage = image;
            return image;
        }

    }
}
