using System.Collections.Generic;

namespace WChessConsole
{
    interface IAbility
    {
        List<Move> GeneratePotentialMoves(Game game, Piece piece);
    }
}
