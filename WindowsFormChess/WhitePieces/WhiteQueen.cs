using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakk_Alkalmazás_2._0
{
    class WhiteQueen
    {
        TypeOfMoves typeOfMoves = new TypeOfMoves();
        int i, j;
        public int[,] GetPossibleMoves(int[,] Table, int[,] PossibleMoves, int i, int j, bool WhiteTurn,bool OtherPlayerTurn)
        {
            if (!WhiteTurn|| OtherPlayerTurn)
            {
                return PossibleMoves;
            }
            PossibleMoves = typeOfMoves.WhiteDiagonalMoves(Table, PossibleMoves, i, j);
            PossibleMoves = typeOfMoves.WhiteVerticalAndHorizontalMoves(Table, PossibleMoves, i, j);
            return PossibleMoves;
        }
        public int[,] IsStale(int[,] Table, int[,] PossibleMoves)
        {
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (Table[i, j] == 15)
                    {
                        int b;
                        //balra
                        for (b = j - 1; b > -1; b--)
                        {
                            if (Table[i, b] == 0)
                            {
                                PossibleMoves[i, b] = 2;
                            }
                            else
                            {
                                if (Table[i, b] > 10)
                                {
                                    break;
                                }
                                else
                                {
                                    PossibleMoves[i, b] = 2;
                                    break;
                                }
                            }
                        }
                        //jobbra
                        for (b = j + 1; b < 8; b++)
                        {
                            if (Table[i, b] == 0)
                            {
                                PossibleMoves[i, b] = 2;
                            }
                            else
                            {
                                if (Table[i, b] > 10)
                                {
                                    break;
                                }
                                else
                                {
                                    PossibleMoves[i, b] = 2;
                                    break;
                                }
                            }
                        }
                        //fel
                        for (b = i - 1; b > -1; b--)
                        {
                            if (i - b >= 0)
                            {
                                if (Table[b, j] == 0)
                                {
                                    PossibleMoves[b, j] = 2;
                                }
                                else
                                {
                                    if (Table[b, j] > 10)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        PossibleMoves[b, j] = 2;
                                        break;
                                    }
                                }
                            }
                        }
                        //le
                        for (b = i + 1; b < 8; b++)
                        {
                            if (i + b >= 0)
                            {
                                if (Table[b, j] == 0)
                                {
                                    PossibleMoves[b, j] = 2;
                                }
                                else
                                {
                                    if (Table[b, j] > 10)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        PossibleMoves[b, j] = 2;
                                        break;
                                    }
                                }
                            }
                        }
                        //balra fel
                        for (b = 1; b < 8; b++)
                        {
                            if (i - b >= 0 && j - b >= 0)
                            {
                                if (Table[i - b, j - b] < 10)
                                {
                                    PossibleMoves[i - b, j - b] = 2; if (Table[i - b, j - b] < 10 && Table[i - b, j - b] != 0) break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        //jobbra fel
                        for (b = 1; b < 8; b++)
                        {
                            if (i - b >= 0 && j + b < 8)
                            {
                                if (Table[i - b, j + b] < 10)
                                {
                                    PossibleMoves[i - b, j + b] = 2; if (Table[i - b, j + b] < 10 && Table[i - b, j + b] != 0) break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        //balra le
                        for (b = 1; b < 8; b++)
                        {
                            if (i + b < 8 && j - b >= 0)
                            {
                                if (Table[i + b, j - b] < 10)
                                {
                                    PossibleMoves[i + b, j - b] = 2; if (Table[i + b, j - b] < 10 && Table[i + b, j - b] != 0) break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        //jobbra le
                        for (b = 1; b < 8; b++)
                        {
                            if (i + b < 8 && j + b < 8)
                            {
                                if (Table[i + b, j + b] < 10)
                                {
                                    PossibleMoves[i + b, j + b] = 2; if (Table[i + b, j + b] < 10 && Table[i + b, j + b] != 0) break;
                                }
                                else
                                {
                                    break;
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
