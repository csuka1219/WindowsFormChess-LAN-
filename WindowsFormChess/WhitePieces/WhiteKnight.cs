using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakk_Alkalmazás_2._0
{
    class WhiteKnight
    {
        TypeOfMoves typeOfMoves = new TypeOfMoves();
        int i, j;
        public int[,] GetPossibleMoves(int[,] Table, int[,] PossibleMoves, int i, int j, bool WhiteTurn,bool OtherPlayerTurn)
        {
            if (!WhiteTurn|| OtherPlayerTurn)
            {
                return PossibleMoves;
            }
            PossibleMoves = typeOfMoves.WhiteHorseMoves(Table, PossibleMoves, i, j);
            return PossibleMoves;
        }
        public int[,] IsStale(int[,] Table, int[,] PossibleMoves)
        {
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (Table[i, j] == 13)
                    {
                        //felfele lépés álló L alakban
                        if (i - 2 >= 0)
                        {
                            if (j - 1 >= 0)
                            {
                                if (Table[i - 2, j - 1] < 10)
                                {
                                    PossibleMoves[i - 2, j - 1] = 2;
                                }
                            }


                            if (j + 1 < 8)
                            {
                                if (Table[i - 2, j + 1] < 10)
                                {
                                    PossibleMoves[i - 2, j + 1] = 2;
                                }
                            }
                        }
                        //lefele lépés álló L alakban
                        if (i + 2 < 8)
                        {
                            if (j - 1 >= 0)
                            {
                                if (Table[i + 2, j - 1] < 10)
                                {
                                    PossibleMoves[i + 2, j - 1] = 2;
                                }
                            }


                            if (j + 1 < 8)
                            {
                                if (Table[i + 2, j + 1] < 10)
                                {
                                    PossibleMoves[i + 2, j + 1] = 2;
                                }
                            }
                        }
                        //felfele lépés fektetett L alakban
                        if (j - 2 >= 0)
                        {
                            if (i - 1 >= 0)
                            {
                                if (Table[i - 1, j - 2] < 10)
                                {
                                    PossibleMoves[i - 1, j - 2] = 2;
                                }
                            }
                            if (i + 1 < 8)
                            {
                                if (Table[i + 1, j - 2] < 10)
                                {
                                    PossibleMoves[i + 1, j - 2] = 2;
                                }
                            }
                        }
                        //lefele lépés fektetett L alakban
                        if (j + 2 < 8)
                        {
                            if (i - 1 >= 0)
                            {
                                if (Table[i - 1, j + 2] < 10)
                                {
                                    PossibleMoves[i - 1, j + 2] = 2;
                                }
                            }
                            if (i + 1 < 8)
                            {
                                if (Table[i + 1, j + 2] < 10)
                                {
                                    PossibleMoves[i + 1, j + 2] = 2;
                                }
                            }
                        }

                    }
                }
            }
            return PossibleMoves;
        }

    }
}
