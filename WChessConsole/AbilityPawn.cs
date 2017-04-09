using System.Collections.Generic;
using System.Diagnostics;

namespace WChessConsole
{
    class AbilityPawn : IAbility
    {
        public List<Move> GeneratePotentialMoves(Game game, Piece piece)
        {
            Vector2I targetPosition;
            List<Move> moves = new List<Move>();

            // Check positions in same file
            targetPosition = piece.Position + piece.MoveDirection;
            if (game.GetPiece(targetPosition) == null)
            {
                moves.Add(new Move(piece.Position, targetPosition, piece.TeamID));

                targetPosition = piece.Position + 2 * piece.MoveDirection;
                if (piece.NumMoves == 0 && game.GetPiece(targetPosition) == null)
                {
                    moves.Add(new Move(piece.Position, targetPosition, piece.TeamID));
                }
            }

            // Check capture opportunities to left of movement direction
            targetPosition = piece.Position + piece.MoveDirection + new Vector2I(-1, 0);
            if (game.GetPiece(targetPosition) != null && game.GetPiece(targetPosition).TeamID != piece.TeamID)
            {
                moves.Add(new Move(piece.Position, targetPosition, piece.TeamID));
            }

            // Check capture opportunities to right of movement direction
            targetPosition = piece.Position + piece.MoveDirection + new Vector2I(1, 0);
            if (game.GetPiece(targetPosition) != null && game.GetPiece(targetPosition).TeamID != piece.TeamID)
            {
                moves.Add(new Move(piece.Position, targetPosition, piece.TeamID));
            }

            // check en passant
            Move lastEnemyMove = game.GetLastMove();
            if (lastEnemyMove != null
                && (lastEnemyMove.destination == piece.Position + new Vector2I(1, 0)
                    || lastEnemyMove.destination == piece.Position + new Vector2I(-1, 0)))
            {
                Piece movedPiece = game.GetPiece(lastEnemyMove.destination);
                Debug.Assert(movedPiece != null);

                if (movedPiece.PieceType == 'P') // need better way to separate piece type definition
                {
                    Vector2I moveDelta = lastEnemyMove.destination - lastEnemyMove.origin;
                    if (Utilities.Abs(moveDelta.y) == 2
                        && game.GetPiece(lastEnemyMove.destination + piece.MoveDirection) == null)
                    {
                        moves.Add(new Move(piece.Position, lastEnemyMove.destination + piece.MoveDirection, piece.TeamID, true));
                    }
                }
            }

            // TODO: check en passant and promotion (how to represent in move?)

            return moves;
        }
    }
}
