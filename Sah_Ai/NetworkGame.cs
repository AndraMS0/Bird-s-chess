using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Sah_Ai
{
    public class NetworkGame
    {

        private Piece currectPieceToMove;
        private int[] offsets;
        private int last_x;
        private int last_y;
        private TcpListener server;
        private TcpClient client;
        private bool isConnected = false;
        private string IP;
        private Player lockedPlayer;
        private Button host;
        private Button _connection;
        private Form _currentForm;
        private Game _game;

        public NetworkGame(Form form)
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

            _currentForm = form;
            _game.MyWhitePlayer = new Player("White Player", Piece.PieceColor.White);
            _game.MyBlackPlayer = new Player("Black Player", Piece.PieceColor.Black);
            _game.MyCurrentPlayer = _game.MyWhitePlayer;
            lockedPlayer = _game.MyWhitePlayer;

            host = new Button();
            host.Location = new Point(850, 102);
            host.Size = new Size(100, 30);
            host.Text = "Host";
            host.Visible = true;
            host.Click += new EventHandler(Host_Click);
            form.Controls.Add(host);
            
            _connection = new Button();
            _connection.Location = new Point(850, 205);
            _connection.Size = new Size(100, 30);
            _connection.Text = "Connection";
            _connection.Visible = true;
            _connection.Click += new EventHandler(Connection_Click);
            form.Controls.Add(_connection);

            Thread receiveThread = new Thread(ReceiveData);
            receiveThread.Start();
        }
        private void ReceiveData()
        {
            while (true)
            {
                while (client == null || client.Connected == false) ;
                Byte[] bytes = new Byte[256];
                String data = null;
                NetworkStream stream = client.GetStream();
                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    if (data != null)
                    {
                        _currentForm.Invoke((MethodInvoker)delegate
                        {
                            DoReceive(data);
                        });
                    }
                    if (client is null) Application.Exit(); 
                }

            }
        }
        private void DoReceive(string result)
        {
            var tokens = result.Split(" ".ToCharArray()).Select(x => int.Parse(x)).ToArray();
            ChessSquare new_poz = new ChessSquare(tokens[0], tokens[1]);
            ChessSquare old_poz = new ChessSquare(tokens[2], tokens[3]);
            int[] offsetsReceived = new int[tokens.Length-4];
            for(int i=4; i<tokens.Length; i++)
            {
                offsetsReceived[i-4] = tokens[i];
            }
            _game.DoMove(new_poz, old_poz, offsetsReceived);
        }
 
        private void SendData(string textToSend)
        {
            NetworkStream stream = client.GetStream();
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(textToSend);

            stream.Write(data, 0, data.Length);
        }
        private void Button_Click(object sender, EventArgs e)
        {
            string messageToSend = String.Empty;
            if (!isConnected) return;
            Button clickedButton = (Button)sender;

            ChessSquare square = _game.getSquare(clickedButton);

            if (square != null)
            {
                Piece piece = _game.getPiece(square);

                if (piece != null && piece.color == _game.MyCurrentPlayer.Color && _game.MyCurrentPlayer==lockedPlayer)
                {

                    clickedButton.BackColor = Color.Yellow;
                    _game.MyYellow_buttons.Enqueue(square);
                    if (_game.MyYellow_buttons.Count > 1)
                        _game.clear_old_selection();
                    offsets = piece.getOffsets(square, _game, _game.MyBoard.MyButtons);
                    if (!_game.MyButtonOfssetsCorrelation.ContainsKey(_game.MyBoard.MyButtons[square.Row, square.Column]))
                        _game.MyButtonOfssetsCorrelation.Add(_game.MyBoard.MyButtons[square.Row, square.Column], offsets);
                    else
                        _game.MyButtonOfssetsCorrelation[_game.MyBoard.MyButtons[square.Row, square.Column]] = offsets;
                    int row = square.Row;
                    int col = square.Column;
                    currectPieceToMove = _game.MyBoard.MyPieces[row, col];
                    last_x = row;
                    last_y = col;

                }
            }

            if (clickedButton.BackColor == Color.Green)
            {

                messageToSend = square.Row.ToString() + " " + square.Column.ToString() + " " +
                currectPieceToMove.Position.Row.ToString() + " " + currectPieceToMove.Position.Column.ToString() + " " +
                string.Join(" ", offsets);
      
                _game.DoMove(square, currectPieceToMove.Position, offsets);


            }
            if (clickedButton.BackColor == Color.Red)
            {
                ChessSquare old_sq = new ChessSquare(last_x, last_y);  
                messageToSend = square.Row.ToString() + " " + square.Column.ToString() + " " +
                 currectPieceToMove.Position.Row.ToString() + " " + currectPieceToMove.Position.Column.ToString() + " " +
                 string.Join(" ", offsets);
                _game.DoMove(square, old_sq, offsets);
                

            }
            SendData(messageToSend);

        }

        private void WaitForConnection()
        {
            while (client is null)
            {
                client = server.AcceptTcpClient();
            }
            isConnected = true;

        }
        private void Connection_Click(object sender, EventArgs args)
        {
            _currentForm.Text = "Client";
            Button button = (Button)sender;
            var window = new IPInput();
            if (window.ShowDialog() == DialogResult.OK)
            {
                IP = window.IP;
            }
            Int32 port = 1234;
            client = new TcpClient();
            client.Connect(IPAddress.Parse(IP), port);
            isConnected = true;
            button.Visible = false;
            host.Visible = false;
            lockedPlayer = _game.MyBlackPlayer;
        }
        private void Host_Click(object sender, EventArgs e)
        {
            _currentForm.Text = "Host";
            Button button = (Button)sender;
            server = null;
            Int32 port = 1234;
            IPAddress localAddr = IPAddress.Any;
            server = new TcpListener(localAddr, port);
            server.Start();
            button.Visible = false;
            _connection.Visible = false;
            
            Thread hostThread = new Thread(WaitForConnection);
            hostThread.Start();
        }

       
    }
}

