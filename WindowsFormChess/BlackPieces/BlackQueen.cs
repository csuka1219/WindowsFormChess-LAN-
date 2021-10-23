using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakk_Alkalmazás_2._0
{
    class BlackQueen
    {
        TypeOfMoves typeOfMoves = new TypeOfMoves();
        int i,j;
        public int[,] GetPossibleMoves(int[,] Table, int[,] PossibleMoves, int i, int j, bool WhiteTurn,bool OtherPlayerTurn)
        {
            if (WhiteTurn|| OtherPlayerTurn)
            {
                return PossibleMoves;
            }
            PossibleMoves = typeOfMoves.BlackDiagonalMoves(Table, PossibleMoves, i, j);
            PossibleMoves = typeOfMoves.BlackVerticalAndHorizontalMoves(Table, PossibleMoves, i, j);
            return PossibleMoves;
        }
        public int[,] IsStale(int[,] Table, int[,] PossibleMoves)
        {
            for (i = 0; i < 8; i++)
            {
                for ( j = 0; j < 8; j++)
                {
                    if (Table[i, j] == 05)
                    {
                        int b;
                        //balra
                        for (b = 1; b < 8; b++)
                        {
                            if (j - b >= 0)
                            {
                                if (Table[i, j - b] == 0 || Table[i, j - b] > 10)
                                {
                                    PossibleMoves[i, j - b] = 2;
                                    if (Table[i, j - b] > 10)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        //jobbra
                        for (b = 1; b < 8; b++)
                        {
                            if (j + b < 8)
                            {
                                if (Table[i, j + b] == 0 || Table[i, j + b] > 10)
                                {
                                    PossibleMoves[i, j + b] = 2;
                                    if (Table[i, j + b] > 10)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        //fel
                        for (b = 1; b < 8; b++)
                        {
                            if (i - b >= 0)
                            {
                                if (Table[i - b, j] == 0 || Table[i - b, j] > 10)
                                {
                                    PossibleMoves[i - b, j] = 2;
                                    if (Table[i - b, j] > 10)
                                    {
                                        break;
                                    }

                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        //le
                        for (b = 1; b < 8; b++)
                        {
                            if (i + b < 8)
                            {
                                if (Table[i + b, j] == 0 || Table[i + b, j] > 10)
                                {
                                    PossibleMoves[i + b, j] = 2; if (Table[i + b, j] > 10) break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        //balra fel
                        for (b = 1; b < 8; b++)
                        {
                            if (i - b >= 0 && j - b >= 0)
                            {
                                if (Table[i - b, j - b] == 0 || Table[i - b, j - b] > 10)
                                {
                                    PossibleMoves[i - b, j - b] = 2; if (Table[i - b, j - b] > 10) break;
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
                                if (Table[i - b, j + b] == 0 || Table[i - b, j + b] > 10)
                                {
                                    PossibleMoves[i - b, j + b] = 2; if (Table[i - b, j + b] > 10) break;
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
                                if (Table[i + b, j - b] == 0 || Table[i + b, j - b] > 10)
                                {
                                    PossibleMoves[i + b, j - b] = 2;
                                    if (Table[i + b, j - b] > 10) break;

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
                                if (Table[i + b, j + b] == 0 || Table[i + b, j + b] > 10)
                                {
                                    PossibleMoves[i + b, j + b] = 2;

                                    if (Table[i + b, j + b] > 10)
                                    {
                                        break;
                                    }
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
