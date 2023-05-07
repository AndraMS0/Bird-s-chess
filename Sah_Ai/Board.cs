using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Sah_Ai
{
    public class Board
    {
        private Button[,] _buttons = new Button[8, 10];
        private Piece[,] _pieces = new Piece[8, 10];
        private Piece currectPieceToMove;
        private int[] offsets;
        private Dictionary<Button, int[]> buttonOfssetsCorrelation = new Dictionary<Button, int[]>();
        private int last_x;
        private int last_y;
        private int new_x;
        private int new_y;
        private Queue<ChessSquare> yellow_buttons = new Queue<ChessSquare>();
        private Player whitePlayer;
        private Player blackPlayer;
        private Player currentPlayer;
        private TcpListener server;
        private TcpClient client;
        private bool isConnected = false;
        private string IP;
        private Player lockedPlayer;
        private Button host;
        private Button connection;
        private Form currentForm;
        public Button[,] MyButtons
        {
            get { return _buttons; }
        }
        public void CreatePiece(Piece.PieceType type, Piece.PieceColor color, int row, int col, string imagePath)
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
        public Board(Form form)
        {
            currentForm = form;
            whitePlayer = new Player("White Player", Piece.PieceColor.White);
            blackPlayer = new Player("Black Player", Piece.PieceColor.Black);
            currentPlayer = whitePlayer;
            lockedPlayer = whitePlayer;

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
                    _buttons[i, j] = button;

                    form.Controls.Add(_buttons[i, j]);
                }

            }
            host = new Button();
            host.Location = new Point(1136, 102);
            host.Size = new Size(100, 30);
            host.Text = "Host";
            host.Visible = true;
            host.Click += new EventHandler(Host_Click);
            form.Controls.Add(host);
            

            connection = new Button();
            connection.Location = new Point(1136, 205);
            connection.Size = new Size(100, 30);
            connection.Text = "Connection";
            connection.Visible = true;
            connection.Click += new EventHandler(Connection_Click);
            form.Controls.Add(connection);

            String folderpath = @"C:\Users\andra\OneDrive\Desktop\an3\sah\Bird-s-chess\Sah_Ai\Images\";
        
            for (int i = 0; i < 10; i++)
            {
                CreatePiece(Piece.PieceType.Pawn, Piece.PieceColor.White, 6, i, folderpath + "pion.png");
            }
            //BISHOP
            CreatePiece(Piece.PieceType.Bishop, Piece.PieceColor.White, 7, 2, folderpath + "nebun.png");
            CreatePiece(Piece.PieceType.Bishop, Piece.PieceColor.White, 7, 7, folderpath + "nebun.png");
            //ROOK
            CreatePiece(Piece.PieceType.Rook, Piece.PieceColor.White, 7, 0, folderpath + "tura.png");
            CreatePiece(Piece.PieceType.Rook, Piece.PieceColor.White, 7, 9, folderpath + "tura.png");
            //KNIGHT
            CreatePiece(Piece.PieceType.Knight, Piece.PieceColor.White, 7, 1, folderpath + "cal.png");
            CreatePiece(Piece.PieceType.Knight, Piece.PieceColor.White, 7, 8, folderpath + "cal.png");
            //QUEEN
            CreatePiece(Piece.PieceType.Queen, Piece.PieceColor.White, 7, 4, folderpath + "regina.png");
            //KING
            CreatePiece(Piece.PieceType.King, Piece.PieceColor.White, 7, 5, folderpath + "rege.png");
            //GUARD
            CreatePiece(Piece.PieceType.Guard, Piece.PieceColor.White, 7, 3, folderpath + "guard.png");
            //EQUERY
            CreatePiece(Piece.PieceType.Equery, Piece.PieceColor.White, 7, 6, folderpath + "equery.png");
            // INITIALIZARE PIESE NEGRE
            //PAWN
            for (int i = 0; i < 10; i++)
            {
                CreatePiece(Piece.PieceType.Pawn, Piece.PieceColor.Black, 1, i, folderpath + "pion2.png");
            }
            //BISHOP
            CreatePiece(Piece.PieceType.Bishop, Piece.PieceColor.Black, 0, 2, folderpath + "nebun2.png");
            CreatePiece(Piece.PieceType.Bishop, Piece.PieceColor.Black, 0, 7, folderpath + "nebun2.png");
            //ROOK
            CreatePiece(Piece.PieceType.Rook, Piece.PieceColor.Black, 0, 0, folderpath + "tura2.png");
            CreatePiece(Piece.PieceType.Rook, Piece.PieceColor.Black, 0, 9, folderpath + "tura2.png");
            //KNIGHT
            CreatePiece(Piece.PieceType.Knight, Piece.PieceColor.Black, 0, 1, folderpath + "cal2.png");
            CreatePiece(Piece.PieceType.Knight, Piece.PieceColor.Black, 0, 8, folderpath + "cal2.png");
            //QUEEN
            CreatePiece(Piece.PieceType.Queen, Piece.PieceColor.Black, 0, 4, folderpath + "regina2.png");
            //KING
            CreatePiece(Piece.PieceType.King, Piece.PieceColor.Black, 0, 5, folderpath + "rege2.png");
            //GUARD
            CreatePiece(Piece.PieceType.Guard, Piece.PieceColor.Black, 0, 3, folderpath + "guard2.png");
            //EQUERY
            CreatePiece(Piece.PieceType.Equery, Piece.PieceColor.Black, 0, 6, folderpath + "equery2.png");
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
                    //if (data != null) currentForm.Dispatcher.Invoke(() => DoReceive(data));
                    if (data != null)
                    {
                        currentForm.Invoke((MethodInvoker)delegate
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
            DoMove(new_poz, old_poz, offsetsReceived);
        }
        public void refreshBoard(int row, int col, int[] offset_array)
        {
            if ((row + col) % 2 == 0)
            {
                _buttons[row, col].BackColor = Color.White;
                _buttons[row, col].BackgroundImage = null;
            }
            else
            {
                _buttons[row, col].BackColor = Color.FromArgb(160, 160, 160);
                _buttons[row, col].BackgroundImage = null;
            }

            for (int i = 0; i < offset_array.Length - 1; i += 2)
            {
                int last_row = row + offset_array[i];
                int last_col = col + offset_array[i + 1];
                if ((last_row + last_col) % 2 == 0)
                {
                    _buttons[last_row, last_col].BackColor = Color.White;

                }
                else
                {
                    _buttons[last_row, last_col].BackColor = Color.FromArgb(160, 160, 160);

                }

            }

        }
        public bool isValidSquare(ChessSquare square)
        {
           
            return (square.Row >= 0 && square.Row < 8 && square.Column >= 0 && square.Column < 10);
        }
        public Piece getPiece(ChessSquare square)
        {
 
            if (!isValidSquare(square))
            {
                return null;
            }
            int row = square.Row;
            int column = square.Column;
            return _pieces[row, column];

        }

        public Image imageToButton(string filePath, int row, int col)
        {
            Button button = _buttons[row, col];
            Image image = Image.FromFile(filePath);

            image = new Bitmap(image, button.Size);

            button.BackgroundImageLayout = ImageLayout.Center;

            button.BackgroundImage = image;
            return image;
        }
        public ChessSquare getSquare(Button button)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    if (_buttons[row, col] == button)
                    {
                        return new ChessSquare(row, col);
                    }
                }
            }

            return null;
        }
        private void clear_old_selection()
        {
            while (yellow_buttons.Count > 1)
            {
                ChessSquare chessSq = yellow_buttons.Peek();

                if ((chessSq.Row + chessSq.Column) % 2 == 0)
                {
                    _buttons[chessSq.Row, chessSq.Column].BackColor = Color.White;

                }
                else
                {
                    _buttons[chessSq.Row, chessSq.Column].BackColor = Color.FromArgb(160, 160, 160);

                }
                int[] current_offsets;
                if (buttonOfssetsCorrelation.TryGetValue(_buttons[chessSq.Row, chessSq.Column], out current_offsets))
                {

                    for (int i = 0; i < current_offsets.Length - 1; i += 2)
                    {
                        int last_r = chessSq.Row + current_offsets[i];
                        int last_c = chessSq.Column + current_offsets[i + 1];
                        if ((last_r + last_c) % 2 == 0)
                        {
                            _buttons[last_r, last_c].BackColor = Color.White;

                        }
                        else
                        {
                            _buttons[last_r, last_c].BackColor = Color.FromArgb(160, 160, 160);

                        }

                    }
                }
                yellow_buttons.Dequeue();


            }
        }
        public void switch_control(ChessSquare current_type)
        {
            Piece currentPiece = getPiece(current_type);
            if (currentPiece.color == Piece.PieceColor.White)
            {
                currentPlayer = blackPlayer;
            }
            else
            {
                currentPlayer = whitePlayer;
            }
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

            ChessSquare square = getSquare(clickedButton);

            if (square != null)
            {
                Piece piece = getPiece(square);

                if (piece != null && piece.color == currentPlayer.Color && currentPlayer==lockedPlayer)
                {

                    clickedButton.BackColor = Color.Yellow;
                    yellow_buttons.Enqueue(square);
                    if (yellow_buttons.Count > 1)
                        clear_old_selection();
                    offsets = piece.getOffsets(square, this, _buttons);
                    if (!buttonOfssetsCorrelation.ContainsKey(_buttons[square.Row, square.Column]))
                        buttonOfssetsCorrelation.Add(_buttons[square.Row, square.Column], offsets);
                    else
                        buttonOfssetsCorrelation[_buttons[square.Row, square.Column]] = offsets;
                    int row = square.Row;
                    int col = square.Column;
                    currectPieceToMove = _pieces[row, col];
                    last_x = row;
                    last_y = col;

                }
            }

            if (clickedButton.BackColor == Color.Green)
            {

                messageToSend = square.Row.ToString() + " " + square.Column.ToString() + " " +
                currectPieceToMove.Position.Row.ToString() + " " + currectPieceToMove.Position.Column.ToString() + " " +
                string.Join(" ", offsets);
      
                DoMove(square, currectPieceToMove.Position, offsets);


            }
            if (clickedButton.BackColor == Color.Red)
            {
                ChessSquare old_sq = new ChessSquare(last_x, last_y);  
                messageToSend = square.Row.ToString() + " " + square.Column.ToString() + " " +
                 currectPieceToMove.Position.Row.ToString() + " " + currectPieceToMove.Position.Column.ToString() + " " +
                 string.Join(" ", offsets);
                DoMove(square, old_sq, offsets);
                

            }
            SendData(messageToSend);

        }
        private void DoMove(ChessSquare new_Position, ChessSquare old_position, int[] offsets_toMove)
        {
            Piece currentPiece = getPiece(old_position);
            _pieces[new_Position.Row, new_Position.Column] = _pieces[old_position.Row, old_position.Column];
            _pieces[old_position.Row, old_position.Column] = null;
            _pieces[new_Position.Row, new_Position.Column].Position = new_Position;

            Image image = currentPiece.Pieceimage;
            image = new Bitmap(image, _buttons[new_Position.Row, new_Position.Column].Size);
            _buttons[new_Position.Row, new_Position.Column].BackgroundImageLayout = ImageLayout.Center;
            _buttons[new_Position.Row, new_Position.Column].BackgroundImage = image;
            switch_control(new_Position);
            refreshBoard(old_position.Row, old_position.Column, offsets_toMove);
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
            lockedPlayer = blackPlayer;
        }
        private void Host_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            server = null;
            Int32 port = 1234;
            IPAddress localAddr = IPAddress.Any;
            server = new TcpListener(localAddr, port);
            server.Start();
            button.Visible = false;
            connection.Visible = false;
            
            Thread hostThread = new Thread(WaitForConnection);
            hostThread.Start();
        }

        public void play()
        {

        }
    }
}

