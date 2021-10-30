using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sakk_Alkalmazás_2._0
{
    public partial class InGameForm : Form
    {
        #region Pieces
        BlackPawn blackPawn = new BlackPawn();
        BlackRook1 blackRook2 = new BlackRook1();
        BlackRook2 blackRook1 = new BlackRook2();
        BlackKnight blackKnight = new BlackKnight();
        BlackKnight2 blackKnight2 = new BlackKnight2();
        BlackBishop blackBishop = new BlackBishop();
        BlackBishop2 blackBishop2 = new BlackBishop2();
        BlackQueen blackQueen = new BlackQueen();
        BlackKing blackKing = new BlackKing();
        WhitePawn whitePawn = new WhitePawn();
        WhiteRook1 whiteRook2 = new WhiteRook1();
        WhiteRook2 whiteRook1 = new WhiteRook2();
        WhiteKnight whiteKnight = new WhiteKnight();
        WhiteKnight2 whiteKnight2 = new WhiteKnight2();
        WhiteBishop whiteBishop = new WhiteBishop();
        WhiteBishop2 whiteBishop2 = new WhiteBishop2();
        WhiteQueen whiteQueen = new WhiteQueen();
        WhiteKing whiteKing = new WhiteKing();
        #endregion
        #region bools
        public bool BlackRookMoved1 = true;
        public bool BlackRookMoved2 = true;
        public bool BlackKingMoved = true;
        public bool WhiteRookMoved1=true;
        public bool WhiteRookMoved2=true;
        public bool WhiteKingMoved=true;
        public bool singleGame = false;
        public bool WhiteTurn=true;
        public bool NotAllowedMove = false;
        public bool OtherPlayerTurn = false;
        public bool GameOver = false;
        public bool enablesocket = true;
        #endregion
        #region Socket
        private Socket sock;
        private BackgroundWorker MessageReceiver = new BackgroundWorker();
        private TcpListener server = null;
        private TcpClient client;
        #endregion
        #region integers
        public int BeforeMove_I;
        public int BeforeMove_J;
        public int LastMovedPiece = 0;
        public int Moves = 0;
        public int Castling = 0;
        public int Promotionvalue = 0;
        public int PromotedPiece { get; set; }
        #endregion
        ClickUserClass[,] TableBackground;
        TableClass tableClass = new TableClass();
        public int[,] WhiteStaleArray = new int[8, 8];
        public int[,] BlackStaleArray = new int[8, 8];

        public InGameForm(bool SingleGame, bool isHost, string ip = null)
        {
            InitializeComponent();
            singleGame = SingleGame;
            //its need for the Lan games
            if (!SingleGame)
            {
                MessageReceiver.DoWork += MessageReceiver_DoWork;

                if (isHost)
                {
                    server = new TcpListener(System.Net.IPAddress.Any, 5732);
                    server.Start();
                    sock = server.AcceptSocket();
                }
                else
                {
                    try
                    {
                        client = new TcpClient(ip, 5732);
                        sock = client.Client;
                        MessageReceiver.RunWorkerAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Close();
                    }
                }
            }

            //this is how every game will start, every pieces have an own number
            tableClass.Table = new int[8, 8]
            {
                { 02, 03, 04, 05, 06, 09, 08, 07},
                { 01, 01, 01, 01, 01, 01, 01, 01},
                { 00, 00, 00, 00, 00, 00, 00, 00},
                { 00, 00, 00, 00, 00, 00, 00 ,00},
                { 00, 00, 00, 00, 00, 00, 00 ,00},
                { 00, 00, 00, 00, 00, 00, 00 ,00},
                { 11, 11, 11, 11, 11, 11, 11, 11},
                { 12, 13, 14, 15, 16, 19, 18, 17},
            };
            TableBackground = new ClickUserClass[8, 8];
            tableClass.PossibleMoves = new int[8, 8];
            tableClass.AllPossibleMoves = new int[8, 8];

            //here we create the board and make it clickable
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    TableBackground[i, j] = new ClickUserClass();
                    TableBackground[i, j].Parent = this;
                    TableBackground[i, j].Location = new Point(j * 50 + 50, i * 50 + 50);
                    TableBackground[i, j].pozX = j;
                    TableBackground[i, j].pozY = i;
                    TableBackground[i, j].Size = new Size(50, 50);
                    TableBackground[i, j].Click += new EventHandler(ClickUserClass_Click);
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 1)
                        {
                            TableBackground[i, j].BackColor = Color.Green;
                        }
                        else
                        {
                            TableBackground[i, j].BackColor = Color.White;
                        }
                    }
                    else
                    {
                        if (j % 2 == 1)
                        {
                            TableBackground[i, j].BackColor = Color.White;
                        }
                        else
                        {
                            TableBackground[i, j].BackColor = Color.Green;
                        }
                    }
                    TableBackground[i, j].BackgroundImageLayout = ImageLayout.Center;
                }
            }
            GetPiecesOnBoard();
            Pieces();
        }
        //in this method we are going to save every pieces in an array as number 1
        //and the empty cells will be 0
        public void GetPiecesOnBoard()
        {
            int i, j;
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (tableClass.Table[i, j] != 0)
                    {
                        tableClass.PossibleMoves[i, j] = 1;
                    }
                    else
                    {
                        tableClass.PossibleMoves[i, j] = 0;
                    }
                }
            }
        }
        //in this method we check our table int array, and draw the right pieces to the right numbers
        //like 05 is equals to black queen
        //we will call this method after every moves
        public void Pieces()
        {
            int i, j;
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    switch (tableClass.Table[i, j])
                    {
                        case 00: TableBackground[i, j].BackgroundImage = null; break;
                        case 01: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\SotetParaszt.png"); break;
                        case 02: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\SotetBastya.png"); break;
                        case 03: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\SotetHuszar.png"); break;
                        case 04: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\SotetFuto.png"); break;
                        case 05: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\SotetKiralyno.png"); break;
                        case 06: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\SotetKiraly.png"); break;
                        case 07: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\SotetBastya.png"); break;
                        case 08: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\SotetHuszar.png"); break;
                        case 09: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\SotetFuto.png"); break;
                        case 11: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\VilagosParaszt.png"); break;
                        case 12: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\VilagosBastya.png"); break;
                        case 13: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\VilagosHuszar.png"); break;
                        case 14: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\VilagosFuto.png"); break;
                        case 15: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\VilagosKiralyno.png"); break;
                        case 16: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\VilagosKiraly.png"); break;
                        case 17: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\VilagosBastya.png"); break;
                        case 18: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\VilagosHuszar.png"); break;
                        case 19: TableBackground[i, j].BackgroundImage = System.Drawing.Image.FromFile("Kepek\\VilagosFuto.png"); break;
                    }
                }
            }
            StaleArrays();
            tableClass.MarkStale(TableBackground, tableClass.Table, WhiteStaleArray, BlackStaleArray);
        }
        //in this event we will give the position of the selected piece to our method
        void ClickUserClass_Click(object sender, EventArgs e)
        {
            AfterClickOnTable((sender as ClickUserClass).pozY, (sender as ClickUserClass).pozX);
        }
        /*in the PossibleMoves array 0 is equals to empty cells, 1 = selectable pieces, 2 is the cells where u can move with your selected piece
         And 3 is the piece that u already selected*/
        public void AfterClickOnTable(int i, int j)
        {
            switch (tableClass.PossibleMoves[i, j])
            {
                //Here we selected a piece so it will call the methods that calculate the avalaible moves
                //And we save the location of the selected piece, its will be very important in the future
                case 1:
                    PossibleMovesByPieces(tableClass.Table[i, j], i, j);
                    BeforeMove_I = i;
                    BeforeMove_J = j;
                    break;
                //this case will be true if the player moved somewhere with his piece
                //we reset our "Moves" integers, then call our very fancy method
                case 2:
                    Moves = 0;
                    SuccesfulMove(i,j);
                    break;
                //this is that situation when the player reclick his piece and the board will be clean
                case 3:
                    EndMove();
                    break;
            }
            tableClass.MarkStale(TableBackground, tableClass.Table, WhiteStaleArray, BlackStaleArray);
        }
        //this method will calculate the avalaible moves of the selected piece
        //x is equals to the value of the selected piece, the i and the j is the position of it
        public void PossibleMovesByPieces(int x, int i, int j)
        {
            //to call this method is important because we have to clear the previously selections, if we choose another pieces
            EndMove();
            //here with the switch we will add the right move positions to our PossibleMoves array as value 2, with use of our classes
            switch (x)
            {
                case 1:
                    tableClass.PossibleMoves = blackPawn.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                    break;
                case 2:
                    tableClass.PossibleMoves = blackRook1.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j, WhiteTurn, OtherPlayerTurn);
                    break;
                case 3:
                    tableClass.PossibleMoves = blackKnight.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j,WhiteTurn,OtherPlayerTurn);
                    break;
                case 4:
                    tableClass.PossibleMoves = blackBishop.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j,WhiteTurn,OtherPlayerTurn);
                    break;
                case 5:
                    tableClass.PossibleMoves = blackQueen.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j,WhiteTurn,OtherPlayerTurn);
                    break;
                case 6:
                    tableClass.PossibleMoves = blackKing.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j,WhiteTurn,BlackKingMoved,BlackRookMoved1,BlackRookMoved2,OtherPlayerTurn);
                    break;
                case 7:
                    tableClass.PossibleMoves = blackRook2.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j,WhiteTurn,OtherPlayerTurn);
                    break;
                case 8:
                    tableClass.PossibleMoves = blackKnight.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j,WhiteTurn,OtherPlayerTurn);
                    break;
                case 9:
                    tableClass.PossibleMoves = blackBishop.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j,WhiteTurn,OtherPlayerTurn);
                    break;
                case 11:
                    tableClass.PossibleMoves = whitePawn.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                    break;
                case 12:
                    tableClass.PossibleMoves = whiteRook1.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j, WhiteTurn, OtherPlayerTurn);
                    break;
                case 13:
                    tableClass.PossibleMoves = whiteKnight.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                    break;
                case 14:
                    tableClass.PossibleMoves = whiteBishop.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                    break;
                case 15:
                    tableClass.PossibleMoves = whiteQueen.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                    break;
                case 16:
                    tableClass.PossibleMoves = whiteKing.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j, WhiteTurn, WhiteKingMoved, WhiteRookMoved1, WhiteRookMoved2,OtherPlayerTurn);
                    break;
                case 17:
                    tableClass.PossibleMoves = whiteRook2.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                    break;
                case 18:
                    tableClass.PossibleMoves = whiteKnight2.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                    break;
                case 19:
                    tableClass.PossibleMoves = whiteBishop.GetPossibleMoves(tableClass.Table, tableClass.PossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                    break;
            }
            //the position of the selected piece will get value 3
            tableClass.PossibleMoves[i, j] = 3;
            //we should not enable impossible moves, like let our king in danger, so we have to validate moves
            RemoveMoveThatNotPossible(x, i, j);
            //now everything is fine, lets draw the possible moves
            ShowPossibleMoves();
        }
        //in this method we will delete that moves that not enable in chess
        //x is the value of the piece, and now the a and the b is the position, because i was in autopilot and i use "i,j" in for cycles
        public void RemoveMoveThatNotPossible(int x,int a,int b)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //we will check every available moves, so every time when the array is 2
                    if (tableClass.PossibleMoves[i, j] == 2)
                    {
                        /*we have to save the original position, because we will simulate that what will happen in every available move position and then
                         we need to restore it*/
                        int SelectedPiece = tableClass.Table[i, j];
                        //here we simulate the new position
                        tableClass.Table[i, j] = x;
                        tableClass.Table[a, b] = 0;
                        //we have 2 arrays with every possible stale situation, and with this method calling we refresh it with our new simulation positions
                        StaleArrays();
                        //this two condition will check that the opponents stale array contains the positon of the king
                        //if theres invalid moves then one of them statement will be true, and then it delete that moves
                        if (tableClass.NotValidMoveChecker(tableClass.Table, WhiteStaleArray, BlackStaleArray) == 1 && WhiteTurn)
                        {
                            tableClass.PossibleMoves[i, j] = 0;
                            if (i == 7 && j == 3&&x==16)
                            {
                                tableClass.PossibleMoves[7, 2] = 0;
                            }
                        }
                        if (tableClass.NotValidMoveChecker(tableClass.Table, WhiteStaleArray, BlackStaleArray) == 2 && !WhiteTurn)
                        {
                            tableClass.PossibleMoves[i, j] = 0;
                            if (i == 0 && j == 3 && x == 6)
                            {
                                tableClass.PossibleMoves[0, 2] = 0;
                            }
                        }
                        //at here we restore everything
                        tableClass.Table[i, j] = SelectedPiece;
                        tableClass.Table[a, b] = x;
                        StaleArrays();
                    }
                }
            }
        }
        //lets color cells
        public void ShowPossibleMoves()
        {
            int i, j;
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    //we color the avalaibles move cells to yellow *array value 2*
                    if (tableClass.PossibleMoves[i, j] == 2)
                    {
                        TableBackground[i, j].BackColor = Color.Yellow;
                    }
                    //the background of the selected cell will be blue at here
                    if (tableClass.PossibleMoves[i, j] == 3)
                    {
                        TableBackground[i, j].BackColor = Color.Blue;
                    }
                }
            }
        }
        //lets recolor the board after every click
        public void EndMove()
        {
            int i, j;
            //we check cells and if there are pieces that position will get value 1 in the PossibleMoves array
            //its important because you can only click that cells where the value is 1
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (tableClass.Table[i, j] != 0)
                    {
                        tableClass.PossibleMoves[i, j] = 1;
                    }
                    else
                    {
                        tableClass.PossibleMoves[i, j] = 0;
                    }
                }
            }
            //this is recolor the board after the player reclick his piece or choose another one
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 1)
                        {
                            TableBackground[i, j].BackColor = Color.Green;
                        }
                        else
                        {
                            TableBackground[i, j].BackColor = Color.White;
                        }
                    }
                    else
                    {
                        if (j % 2 == 1)
                        {
                            TableBackground[i, j].BackColor = Color.White;
                        }
                        else
                        {
                            TableBackground[i, j].BackColor = Color.Green;
                        }
                    }
                }
            }
            tableClass.MarkStale(TableBackground, tableClass.Table, WhiteStaleArray, BlackStaleArray);
        }
        //we should reset the stale possiblities often
        public void StaleArrays()
        {
            //its very simple, we save every possible moves by players
            WhiteStaleArray = new int[8, 8];
            WhiteStaleArray = blackPawn.IsStale(tableClass.Table, WhiteStaleArray);
            WhiteStaleArray = blackRook1.IsStale(tableClass.Table, WhiteStaleArray);
            WhiteStaleArray = blackRook2.IsStale(tableClass.Table, WhiteStaleArray);
            WhiteStaleArray = blackKnight.IsStale(tableClass.Table, WhiteStaleArray);
            WhiteStaleArray = blackKnight2.IsStale(tableClass.Table, WhiteStaleArray);
            WhiteStaleArray = blackBishop.IsStale(tableClass.Table, WhiteStaleArray);
            WhiteStaleArray = blackBishop2.IsStale(tableClass.Table, WhiteStaleArray);
            WhiteStaleArray = blackQueen.IsStale(tableClass.Table, WhiteStaleArray);

            BlackStaleArray = new int[8, 8];
            BlackStaleArray = whitePawn.IsStale(tableClass.Table, BlackStaleArray);
            BlackStaleArray = whiteRook1.IsStale(tableClass.Table, BlackStaleArray);
            BlackStaleArray = whiteRook2.IsStale(tableClass.Table, BlackStaleArray);
            BlackStaleArray = whiteKnight.IsStale(tableClass.Table, BlackStaleArray);
            BlackStaleArray = whiteKnight2.IsStale(tableClass.Table, BlackStaleArray);
            BlackStaleArray = whiteBishop.IsStale(tableClass.Table, BlackStaleArray);
            BlackStaleArray = whiteBishop2.IsStale(tableClass.Table, BlackStaleArray);
            BlackStaleArray = whiteQueen.IsStale(tableClass.Table, BlackStaleArray);

        }
        //this method will happening when one of the players did a valid move, it get the position of the piece after the move
        public void SuccesfulMove(int i, int j)
        {
            enablesocket = true;
            if (!singleGame)
            {
                //first of all if we play Lan game, we must save that which piece was moved
                //this is very important because we have to send this data as byte to the enemy with socket
                for (int x = 0; x < 20; x++)
                {
                    if(tableClass.Table[BeforeMove_I, BeforeMove_J] == x)
                    {
                        LastMovedPiece = x;
                    }
                }
            }
            //in our next method we will check that castling (king rook swap) is possible
            //and if one of the pawns went throught the board(promotion) we should do something as well
            CastlingAndPawnPromotionChecker(i,j);
            //and this is the point where we give the moved piece to the new position
            tableClass.Table[i, j] = tableClass.Table[BeforeMove_I, BeforeMove_J];
            //these if-s is for the castling, and we will send the "Castling" integer to the enemy, if we play on Lan
            //if Castling is 1 the black player did castling, if its 2, then the white one did it
            if (tableClass.Table[BeforeMove_I, BeforeMove_J] == 06)
            {
                if (i == 0 && j == 2)
                {
                    tableClass.Table[0, 3] = 02;
                    tableClass.Table[0, 0] = 0;
                }
                if (i == 0 && j == 6)
                {
                    tableClass.Table[0, 5] = 02;
                    tableClass.Table[0, 7] = 0;
                }
                Castling = 1;
            }
            if (tableClass.Table[BeforeMove_I, BeforeMove_J] == 16)
            {
                if (i == 7 && j == 2)
                {
                    tableClass.Table[7, 3] = 12; tableClass.Table[7, 0] = 0;
                }
                if (i == 7 && j == 6)
                {
                    tableClass.Table[7, 5] = 12; tableClass.Table[7, 7] = 0;
                }
                Castling = 2;
            }
            //naturally we should null the previously position of the piece
            tableClass.Table[BeforeMove_I, BeforeMove_J] = 0;
            //we call our drawing method
            Pieces();
            //we should reset the color of the board at this point as well
            EndMove();
            //now we will save every possible move of the player who will turn in an array
            //that will be important because if that array is null that means the player got checkmate that equals to the game is over
            EveryPossibleMoves();
            //after we got our array we will check that is there checkmate or not
            CheckMateChecker(i,j);
            //if its Lan game, we call this method with the new position
            if (enablesocket && !singleGame && !GameOver)
            {
                SendMove(i, j);
            }
            //the opponent turn
            WhiteTurn = !WhiteTurn;
        }
        //several bools
        public void CastlingAndPawnPromotionChecker(int i,int j)
        {
            //if one of the player moved with his rook or king then they can not make castling anymore
            //we check this in this switch
            switch (tableClass.Table[BeforeMove_I, BeforeMove_J])
            {
                case 02:
                    BlackRookMoved1 = false;
                    break;
                case 07:
                    BlackRookMoved2 = false;
                    break;
                case 12:
                    WhiteRookMoved1 = false;
                    break;
                case 17:
                    WhiteRookMoved2 = false;
                    break;
                case 06:
                    BlackKingMoved = false;
                    break;
                case 16:
                    WhiteKingMoved = false;
                    break;
            }

            //if promotion is available we will active an other form where we can select that we want instead of the pawn
            if (tableClass.Table[BeforeMove_I, BeforeMove_J] == 01)
            {
                if (i == 7)
                {
                    PromotionForm Promotion = new PromotionForm(false);
                    Promotion.ShowDialog();
                    tableClass.Table[BeforeMove_I, BeforeMove_J] = Promotion.PromotedPiece;
                    Promotionvalue = Promotion.PromotedPiece;
                }
            }
            if (tableClass.Table[BeforeMove_I, BeforeMove_J] == 11)
            {
                if (i == 0)
                {
                    PromotionForm Promotion = new PromotionForm(true);
                    Promotion.ShowDialog();
                    tableClass.Table[BeforeMove_I, BeforeMove_J] = Promotion.PromotedPiece;
                    Promotionvalue = Promotion.PromotedPiece;
                }
            }
        }
        //get every moves of the player
        public void EveryPossibleMoves()
        {
            int i = 0;
            int j = 0;
            //we should reset the array after every move
            tableClass.AllPossibleMoves = new int[8,8];
            //this need because we need that player who isn't turned yet
            WhiteTurn = !WhiteTurn;
            //pieces
            for (int x = 1; x < 20; x++)
            {
                //and positions
                for (i = 0; i < 8; i++)
                {
                    for (j = 0; j < 8; j++)
                    {
                        if (tableClass.Table[i, j] == x)
                        {
                            //similar like earlier switchs
                            switch (x)
                            {
                                case 1:
                                    tableClass.AllPossibleMoves = blackPawn.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 2:
                                    tableClass.AllPossibleMoves = blackRook1.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 3:
                                    tableClass.AllPossibleMoves = blackKnight.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 4:
                                    tableClass.AllPossibleMoves = blackBishop.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 5:
                                    tableClass.AllPossibleMoves = blackQueen.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 6:
                                    tableClass.AllPossibleMoves = blackKing.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn, BlackKingMoved, BlackRookMoved1, BlackRookMoved2,OtherPlayerTurn);
                                    break;
                                case 7:
                                    tableClass.AllPossibleMoves = blackRook2.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 8:
                                    tableClass.AllPossibleMoves = blackKnight.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 9:
                                    tableClass.AllPossibleMoves = blackBishop2.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 11:
                                    tableClass.AllPossibleMoves = whitePawn.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 12:
                                    tableClass.AllPossibleMoves = whiteRook1.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 13:
                                    tableClass.AllPossibleMoves = whiteKnight.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 14:
                                    tableClass.AllPossibleMoves = whiteBishop.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 15:
                                    tableClass.AllPossibleMoves = whiteQueen.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 16:
                                    tableClass.AllPossibleMoves = whiteKing.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn, WhiteKingMoved, WhiteRookMoved1, WhiteRookMoved2, OtherPlayerTurn);
                                    break;
                                case 17:
                                    tableClass.AllPossibleMoves = whiteRook2.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 18:
                                    tableClass.AllPossibleMoves = whiteKnight2.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                                case 19:
                                    tableClass.AllPossibleMoves = whiteBishop2.GetPossibleMoves(tableClass.Table, tableClass.AllPossibleMoves, i, j, WhiteTurn,OtherPlayerTurn);
                                    break;
                            }
                            //okay we got moves by pieces, now we should delete that are invalids
                            RemoveMoveThatNotPossible2(x, i, j);
                        }
                    }
                }
            }
            //and now we change back it
            WhiteTurn = !WhiteTurn;
        }
        //lets delete invalid moves
        public void RemoveMoveThatNotPossible2(int x, int a, int b)
        {
            //you already know this
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //you can remember that value 2 is the possible moves
                    if (tableClass.AllPossibleMoves[i, j] == 2)
                    {
                        //and this is very familiar than the "RemoveMoveThatNotPossible", lets simulate
                        int lastHitPiece = tableClass.Table[i, j];
                        
                        tableClass.Table[i, j] = x;
                        tableClass.Table[a, b] = 0;
                        StaleArrays();
                        //invalid move
                        if (tableClass.NotValidMoveChecker(tableClass.Table, WhiteStaleArray, BlackStaleArray) == 1 && WhiteTurn)
                        {
                            tableClass.AllPossibleMoves[i, j] = 0;
                        }
                        //invalid move
                        if (tableClass.NotValidMoveChecker(tableClass.Table, WhiteStaleArray, BlackStaleArray) == 2 && !WhiteTurn)
                        {
                            tableClass.AllPossibleMoves[i, j] = 0;
                        }
                        //this is a valid move, so we increment the Moves integer
                        //if Moves not equals to 0, than its not checkmate
                        if (tableClass.NotValidMoveChecker(tableClass.Table, WhiteStaleArray, BlackStaleArray) == 3)
                        {
                            Moves++;
                        }
                        tableClass.Table[i, j] = lastHitPiece;
                        tableClass.Table[a, b] = x;
                        StaleArrays();
                    }
                }
            }
            tableClass.AllPossibleMoves = new int[8, 8];
        }
        //and this is where we decide that the game is over or not
        public void CheckMateChecker(int a, int b)
        {
            if (Moves == 0)
            {
                GameOver = true;
                //the game is over, but nevertheless we send the last move to the opponent
                if (enablesocket && !singleGame)
                {
                    SendMove(a, b);
                }
                MessageBox.Show("You Win!");
            }
        }
        //socket things
        private void MessageReceiver_DoWork(object sender, DoWorkEventArgs e)
        {
            ReceiveMove();
        }
        //and this is where we send datas using socket
        private void SendMove(int i, int j)
        {
            //we should send the moved piece, and its position, the new position, the checkmate counter and the value of castling, and promotionalue
            byte[] datas = { (byte)LastMovedPiece, (byte)BeforeMove_I, (byte)BeforeMove_J, (byte)i, (byte)j, (byte)Moves, (byte)Castling, (byte)Promotionvalue };
            sock.Send(datas);
            MessageReceiver.DoWork += MessageReceiver_DoWork;
            if (!MessageReceiver.IsBusy)
            {
                MessageReceiver.RunWorkerAsync();
            }
            OtherPlayerTurn = true;
        }
        //this is where the other player got datas
        private void ReceiveMove()
        {
            byte[] buffer = new byte[8];
            sock.Receive(buffer);
            //previously position have to be 0
            tableClass.Table[buffer[1], buffer[2]] = 0;
            //new position with the piece
            tableClass.Table[buffer[3], buffer[4]] = buffer[0];
            //castling
            if (buffer[6] == 1)
            {
                if (buffer[4] == 2)
                {
                    tableClass.Table[0, 3] = 02;
                    tableClass.Table[0, 0] = 0;
                }
                if (buffer[4] == 6)
                {
                    tableClass.Table[0, 5] = 02;
                    tableClass.Table[0, 7] = 0;
                }
            }
            if (buffer[6] == 2)
            {
                if (buffer[4] == 2)
                {
                    tableClass.Table[7, 3] = 12; tableClass.Table[7, 0] = 0;
                }
                if (buffer[4] == 6)
                {
                    tableClass.Table[7, 5] = 12; tableClass.Table[7, 7] = 0;
                }
            }
            //get promoted piece
            if (buffer[7] > 0)
            {
                tableClass.Table[buffer[3], buffer[4]] = buffer[7];
            }
            //toggle turn
            WhiteTurn = !WhiteTurn;
            //i think we know this
            Pieces();
            StaleArrays();
            tableClass.MarkStale(TableBackground, tableClass.Table, WhiteStaleArray, BlackStaleArray);
            OtherPlayerTurn = false;
            //losing screen
            if (buffer[5] == 0)
            {
                if (WhiteTurn)
                {
                    MessageBox.Show("You Lost!");
                }
                else
                {
                    MessageBox.Show("You lost!");
                }
            }
        }
        //end connection
        private void InGameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageReceiver.WorkerSupportsCancellation = true;
            MessageReceiver.CancelAsync();
            if (server != null)
                server.Stop();
        }
    }
}
