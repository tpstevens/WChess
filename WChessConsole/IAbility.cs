using System.Collections.Generic;

namespace WChessConsole
{
    interface IAbility
    {
        List<Move> GeneratePotentialMoves(GameBoard board, Piece piece);
    }
}
