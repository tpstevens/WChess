using System;
using System.Collections.Generic;

namespace WChess.Chess
{
    public partial class Game
    {
        private abstract class Piece
        {
            protected uint numMoves = 0;
            protected Vector2I position;

            public abstract List<Move> returnAvailableMoves(Board board);
            public abstract bool threateningKing(Board board, Move lastOpponentMove); // won't recalculate if not necessary

            public void updatePosition(Vector2I position)
            {
                this.position = position;
            }
        }

        private class Pawn : Piece
        {
            public override List<Move> returnAvailableMoves(Board board)
            {
                throw new NotImplementedException();
            }
        }
    }
}
