using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakk_Alkalmazás_2._0
{
    class WhitePawn
    {
        public int[,] GetPossibleMoves(int[,] Table, int[,] PossibleMoves, int i, int j, bool WhiteTurn,bool OtherPlayerTurn)
        {
            if (!WhiteTurn|| OtherPlayerTurn)
            {
                return PossibleMoves;
            }
            //lépes előre ha nincs előtte senki
            if (Table[i - 1, j] == 0)
            {
                PossibleMoves[i - 1, j] = 2;
            }
            //ellenfél bábú leütése balra
            if (j - 1 >= 0)
            {
                if (Table[i - 1, j - 1] < 10 && Table[i - 1, j - 1] != 0)
                {
                    PossibleMoves[i - 1, j - 1] = 2;
                }
            }
            //ellenfél bábú leütése jobra
            if (j + 1 < 8)
            {
                if (Table[i - 1, j + 1] < 10 && Table[i - 1, j + 1] != 0)
                {
                    PossibleMoves[i - 1, j + 1] = 2;
                }
            }
            //dupla lépés előre kezdőpontról, ha nincs előtte senki
            if (i == 6)
            {
                if (Table[i - 2, j] == 0 && Table[i - 1, j] == 0)
                {
                    PossibleMoves[i - 2, j] = 2;
                }
            }
            return PossibleMoves;
        }
        public int[,] IsStale(int[,] Table, int[,] PossibleMoves)
        {
            for (int i = 1; i < 7; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Table[i, j] == 11)
                    {
                        //ellenfél bábú leütése balra
                        if (j - 1 >= 0)
                        {
                            if (Table[i - 1, j - 1] < 10 && Table[i - 1, j - 1] != 0)
                            {
                                PossibleMoves[i - 1, j - 1] = 2;
                            }
                        }
                        //ellenfél bábú leütése jobra
                        if (j + 1 < 8)
                        {
                            if (Table[i - 1, j + 1] < 10 && Table[i - 1, j + 1] != 0)
                            {
                                PossibleMoves[i - 1, j + 1] = 2;
                            }
                        }
                    }
                }
            }
            return PossibleMoves;
        }

    }
}
