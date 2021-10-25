using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakk_Alkalmazás_2._0
{
    class BlackKnight2
    {
        TypeOfMoves typeOfMoves = new TypeOfMoves();
        int i, j;
        public int[,] GetPossibleMoves(int[,] Table, int[,] PossibleMoves, int i, int j, bool WhiteTurn, bool OtherPlayerTurn)
        {
            if (WhiteTurn|| OtherPlayerTurn)
            {
                return PossibleMoves;
            }
            PossibleMoves = typeOfMoves.BlackHorseMoves(Table, PossibleMoves, i, j); return PossibleMoves;
        }
        public int[,] IsStale(int[,] Table, int[,] PossibleMoves)
        {
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (Table[i, j] == 08)
                    {
                        PossibleMoves = typeOfMoves.BlackHorseMoves(Table, PossibleMoves, i, j); return PossibleMoves;
                    }
                }
            }
            return PossibleMoves;
        }
    }
}
