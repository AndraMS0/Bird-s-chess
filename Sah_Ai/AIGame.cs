using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sah_Ai
{
    public class AIGame
    {
        private Game _game;
        private Piece _currectPieceToMove;
        private int[] _offsets;
        private int _last_x;
        private int _last_y;
        private int _depth;
        public void SetDepth(int depth) {
           this._depth = depth;
        }
        public AIGame(Form form)
        {

            _game = new Game(form);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 10; j++)
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
                    button.Location = new Point(j * 70, i * 70);
                    _game.MyBoard.MyButtons[i, j] = button;

                    form.Controls.Add(_game.MyBoard.MyButtons[i, j]);
                }
            }
            _game.MyBoard.CreateAllPieces();

            //_currentForm = form;
            _game.MyWhitePlayer = new Player("White Player", Piece.PieceColor.White);
            _game.MyBlackPlayer = new Player("Black Player", Piece.PieceColor.Black);
            _game.MyCurrentPlayer = _game.MyWhitePlayer;

        }
        private void Button_Click(object sender, EventArgs e)
        {
           
            Button clickedButton = (Button)sender;

            ChessSquare square = _game.getSquare(clickedButton);

            if (square != null)
            {
                Piece piece = _game.getPiece(square);

                if (piece != null && piece.color == Piece.PieceColor.White)
                {

                    clickedButton.BackColor = Color.Yellow;
                    _game.MyYellow_buttons.Enqueue(square);
                    if (_game.MyYellow_buttons.Count > 1)
                        _game.clear_old_selection();
                    _offsets = piece.getOffsets(square, _game, _game.MyBoard.MyButtons);
                    _game.VerifyKingState();
                    if (!_game.MyButtonOfssetsCorrelation.ContainsKey(_game.MyBoard.MyButtons[square.Row, square.Column]))
                        _game.MyButtonOfssetsCorrelation.Add(_game.MyBoard.MyButtons[square.Row, square.Column], _offsets);
                    else
                        _game.MyButtonOfssetsCorrelation[_game.MyBoard.MyButtons[square.Row, square.Column]] = _offsets;
                    int row = square.Row;
                    int col = square.Column;
                    _currectPieceToMove = _game.MyBoard.MyPieces[row, col];
                    _last_x = row;
                    _last_y = col;

                }
            }

            if (clickedButton.BackColor == Color.Green)
            {

                _game.DoMove(square, _currectPieceToMove.Position, _offsets);
                CalculateBestMoveForEnemy();

            }
            if (clickedButton.BackColor == Color.Red)
            {
                Piece.PieceColor kingColor = Piece.PieceColor.White;
                if (_game.IsCheck(square, ref kingColor))
                {
                    MessageBox.Show($"The {kingColor} King is in Check");
                }
                else
                {
                    ChessSquare old_sq = new ChessSquare(_last_x, _last_y);
                    _game.DoMove(square, old_sq, _offsets);
                }
                
                CalculateBestMoveForEnemy();

            }

           

        }

  
        public void CalculateBestMoveForEnemy()
        {
            var bestMove = GetBestMoveForEnemy();
            Piece.PieceColor kingColor = Piece.PieceColor.White;
            if (_game.IsCheck(bestMove.Item2, ref kingColor))
            {
                MessageBox.Show($"The {kingColor} King is in Check");
            }
            else
            {
                _game.DoMove(bestMove.Item2, bestMove.Item1.MyPosition, bestMove.Item3);
            }
            
        }
        public Tuple<Piece, ChessSquare, int[]> GetBestMoveForEnemy()
        {
            var allEnemyMoves = GetAllMovesForPlayerWithPieces(Piece.PieceColor.Black);
            var scores = new List<int>();
            foreach(var enemy in allEnemyMoves)
            {
                if (enemy.Item2 == null)
                    continue;
                var initialPosition = enemy.Item1.MyPosition;
                ChessSquare nextPosition = enemy.Item2;
                var initialPieceOnNext = _game.getPiece(nextPosition);
                    var piece = enemy.Item1;
                    _game.MyBoard.MyPieces[nextPosition.Row, nextPosition.Column] = null;
                    
                _game.MyBoard.MyPieces[nextPosition.Row, nextPosition.Column] = _game.MyBoard.MyPieces[piece.MyPosition.Row, piece.MyPosition.Column]; //piece;
                _game.MyBoard.MyPieces[piece.MyPosition.Row, piece.MyPosition.Column] = null;
                piece.MyPosition = nextPosition;
                enemy.Item1.Position = nextPosition;

                scores.Add(Minimax(_depth, _game.MyBlackPlayer));

                    _game.MyBoard.MyPieces[initialPosition.Row, initialPosition.Column] = _game.MyBoard.MyPieces[nextPosition.Row, nextPosition.Column];
                _game.MyBoard.MyPieces[nextPosition.Row, nextPosition.Column] = initialPieceOnNext;
                piece.MyPosition = initialPosition;
                enemy.Item1.Position = initialPosition;


            }
            _game.MyCurrentPlayer = _game.MyBlackPlayer;
            return allEnemyMoves[scores.IndexOf(scores.Max())];
        }
        public int Minimax(int depth, Player player)
        {
            _game.MyCurrentPlayer = player;
            int value = 0;
            if (depth == 0)
                return value;
            var allMoves = GetAllMovesForPlayerWithPieces(player.Color);
            List<Tuple<Piece, ChessSquare, int[]>> newAllMoves = new List<Tuple<Piece, ChessSquare, int[]>>();
            
            foreach(var move in allMoves)
            {
                ChessSquare sq = new ChessSquare(move.Item2.Row, move.Item2.Column);
                if(_game.getPiece(sq) != null)
                {
                    newAllMoves.Add(move);
                }
            }
            

            if (player == _game.MyBlackPlayer)
            {
                value = int.MinValue;
                foreach(var enemy in newAllMoves)
                {
                    
                        var positionToMove = enemy.Item2;
                        var pieceOnNextPosition = _game.getPiece(positionToMove);
                    if (pieceOnNextPosition != null)
                        if (pieceOnNextPosition.Type == Piece.PieceType.King && pieceOnNextPosition.color == Piece.PieceColor.White)
                            //continue;
                            return value;
                        if (enemy.Item1 == null)
                            continue;
                        if(pieceOnNextPosition != null && pieceOnNextPosition.color == Piece.PieceColor.White)
                        {
                            value += pieceOnNextPosition.score;
                        }
                        var initialPosition = enemy.Item1.Position;
                        var initialPieceOnNextMovePosition = _game.MyBoard.MyPieces[positionToMove.Row, positionToMove.Column];
                        var piece = enemy.Item1;
                        _game.MyBoard.MyPieces[positionToMove.Row, positionToMove.Column] = null;
                    //doesMove
                    _game.MyBoard.MyPieces[positionToMove.Row, positionToMove.Column] = piece;
                    _game.MyBoard.MyPieces[piece.MyPosition.Row, piece.MyPosition.Column] = null;
                   piece.MyPosition = positionToMove;
                    enemy.Item1.Position = positionToMove;
                    value += Minimax(depth - 1, _game.MyWhitePlayer);
                        _game.MyBoard.MyPieces[initialPosition.Row, initialPosition.Column] = _game.MyBoard.MyPieces[positionToMove.Row, positionToMove.Column];
                    _game.MyBoard.MyPieces[positionToMove.Row, positionToMove.Column] = pieceOnNextPosition;
                    piece.MyPosition = initialPosition;
                    enemy.Item1.Position = initialPosition;


                }

            }

            if(player == _game.MyWhitePlayer)
            {
                value = int.MaxValue;
                foreach (var enemy in newAllMoves)
                {
                    
                    var positionToMove = enemy.Item2;
                        var pieceOnNextPosition = _game.getPiece(positionToMove);
                    if (pieceOnNextPosition != null && pieceOnNextPosition.Type == Piece.PieceType.King && pieceOnNextPosition.color == Piece.PieceColor.Black)
                        return -value;

                        if (pieceOnNextPosition != null && pieceOnNextPosition.color == Piece.PieceColor.Black)
                        {
                            value -= pieceOnNextPosition.score;
                        }

                        var initialPosition = enemy.Item1.Position;
                        var initialPieceOnNextMovePosition = _game.MyBoard.MyPieces[positionToMove.Row, positionToMove.Column];
                        var piece = enemy.Item1;
                        _game.MyBoard.MyPieces[positionToMove.Row, positionToMove.Column] = null;

                        
                        _game.MyBoard.MyPieces[positionToMove.Row, positionToMove.Column] = piece;
                    _game.MyBoard.MyPieces[piece.MyPosition.Row, piece.MyPosition.Column] = null;
                    _game.MyBoard.MyPieces[positionToMove.Row, positionToMove.Column].MyPosition = positionToMove;
                        piece.MyPosition = positionToMove;
                    enemy.Item1.Position = positionToMove;
                        value -= Minimax(depth - 1, _game.MyBlackPlayer);
                  
                    _game.MyBoard.MyPieces[initialPosition.Row, initialPosition.Column] = _game.MyBoard.MyPieces[positionToMove.Row, positionToMove.Column]; //piece;
                        _game.MyBoard.MyPieces[positionToMove.Row, positionToMove.Column] = initialPieceOnNextMovePosition;
                        piece.MyPosition = initialPosition;
                    enemy.Item1.Position = initialPosition;

                }
            }

            return value;

        }
        public void RefreshBoardAi(int row, int col, int[] offset_array)
        {
         
            for (int i = 0; i < offset_array.Length - 1; i += 2)
            {
                int last_row = row + offset_array[i];
                int last_col = col + offset_array[i + 1];
                if ((last_row + last_col) % 2 == 0)
                {
                    _game.MyBoard.MyButtons[last_row, last_col].BackColor = Color.White;

                }
                else
                {
                    _game.MyBoard.MyButtons[last_row, last_col].BackColor = Color.FromArgb(160, 160, 160);

                }

            }

        }
        public List<Tuple<Piece, ChessSquare, int[]>> GetAllMovesForPlayerWithPieces(Piece.PieceColor player)
        {
            var allMoves = new List<Tuple<Piece, ChessSquare, int[]>>();
            for(int i=0; i< 8; i++)
            {
                for(int j=0; j < 10; j++)
                {
                    ChessSquare square = new ChessSquare(i, j);
                    if (_game.getPiece(square) != null && _game.MyBoard.MyPieces[i, j].color == player)
                    {
                        int[] move = _game.MyBoard.MyPieces[i, j].getOffsets(square, _game, _game.MyBoard.MyButtons);
                        if (move.Length == 0)
                            continue;
                        for (int k = 0; k < 8; k++)
                        {
                            for (int n = 0; n < 10; n++)
                            {
                                if (_game.MyBoard.MyButtons[k, n].BackColor == Color.Green || _game.MyBoard.MyButtons[k, n].BackColor == Color.Red)
                                {
                                    ChessSquare potentialMove = new ChessSquare(k, n);
                                    allMoves.Add(new Tuple<Piece, ChessSquare, int[]>(_game.MyBoard.MyPieces[i, j], potentialMove, move));
                                }
                            }
                        }
                        RefreshBoardAi(square.Row, square.Column, move);
                      
                    }
                }
            }
            return allMoves;
        }

    }
}
