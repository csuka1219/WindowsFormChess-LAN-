using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakk_Alkalmazás_2._0
{
    class BlackBishop
    {
        TypeOfMoves typeOfMoves = new TypeOfMoves();
        int i, j;
        public int[,] GetPossibleMoves(int[,] Table, int[,] PossibleMoves, int i, int j, bool WhiteTurn,bool OtherPlayerTurn)
        {
            if (WhiteTurn|| OtherPlayerTurn)
            {
                return PossibleMoves;
            }
            PossibleMoves = typeOfMoves.BlackDiagonalMoves(Table, PossibleMoves, i, j);
            return PossibleMoves;
        }
        public int[,] IsStale(int[,] Table, int[,] PossibleMoves)
        {
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (Table[i, j] == 04)
                    {
                        int b;
                        //balra felfele
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
                        //jobbra felfele
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
                        //balra felfele
                        for (b = 1; b < 8; b++)
                        {
                            if (i + b < 8 && j - b >= 0)
                            {
                                if (Table[i + b, j - b] == 0 || Table[i + b, j - b] > 10)
                                {
                                    PossibleMoves[i + b, j - b] = 2; if (Table[i + b, j - b] > 10) break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        //jobbra lefele
                        for (b = 1; b < 8; b++)
                        {
                            if (i + b < 8 && j + b < 8)
                            {
                                if (Table[i + b, j + b] == 0 || Table[i + b, j + b] > 10)
                                {
                                    PossibleMoves[i + b, j + b] = 2; if (Table[i + b, j + b] > 10) break;
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
