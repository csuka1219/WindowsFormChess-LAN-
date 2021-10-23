using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakk_Alkalmazás_2._0
{
    class WhiteRook2
    {
        TypeOfMoves typeOfMoves = new TypeOfMoves();
        int i, j;
        public int[,] GetPossibleMoves(int[,] Table, int[,] PossibleMoves, int i, int j, bool WhiteTurn,bool OtherPlayerTurn)
        {
            if (!WhiteTurn|| OtherPlayerTurn)
            {
                return PossibleMoves;
            }
            PossibleMoves = typeOfMoves.WhiteVerticalAndHorizontalMoves(Table, PossibleMoves, i, j);
            return PossibleMoves;
        }
        public int[,] IsStale(int[,] Table, int[,] PossibleMoves)
        {
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (Table[i, j] == 17)
                    {
                        goto End;
                    }
                }
                if (i == 7)
                {
                    return PossibleMoves;
                }
            }
        End:
            int b;
            //felfele lépés ütközésig
            for (b = i - 1; b > -1; b--)
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
            //lefele lépés ütközésig
            for (b = i + 1; b < 8; b++)
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
            //balra lépés ütközésig
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
            //jobbra lépés ütközésig
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
            return PossibleMoves;
        }

    }
}
