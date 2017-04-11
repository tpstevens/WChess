using System.Collections.Generic;
using System.Diagnostics;

namespace WChessConsole
{
    class AbilityPawn : IAbility
    {
        public List<Move> GeneratePotentialMoves(GameBoard board, Piece piece)
        {
            List<Move> moves = new List<Move>();
			Piece targetPiece;
            Vector2I targetPosition;

            // Check positions in same file
            targetPosition = piece.Position + piece.MoveDirection;
            if (board.ValidatePosition(targetPosition) && board.GetPieceAt(targetPosition) == null)
            {
                moves.Add(new Move(piece.Position, targetPosition, piece.TeamID));

                targetPosition = piece.Position + 2 * piece.MoveDirection;
                if (piece.NumMoves == 0 && board.GetPieceAt(targetPosition) == null)
                {
                    moves.Add(new Move(piece.Position, targetPosition, piece.TeamID));
                }
            }

            // Check capture opportunities to left of movement direction
            targetPosition = piece.Position + piece.MoveDirection + new Vector2I(-1, 0);
			if (board.ValidatePosition(targetPosition))
			{
				targetPiece = board.GetPieceAt(targetPosition);
				if (targetPiece != null && targetPiece.TeamID != piece.TeamID)
				{
					moves.Add(new Move(piece.Position, targetPosition, piece.TeamID, targetPiece));
				}
			}

            // Check capture opportunities to right of movement direction
            targetPosition = piece.Position + piece.MoveDirection + new Vector2I(1, 0);
			if (board.ValidatePosition(targetPosition))
			{
				targetPiece = board.GetPieceAt(targetPosition);
				if (targetPiece != null && targetPiece.TeamID != piece.TeamID)
				{
					moves.Add(new Move(piece.Position, targetPosition, piece.TeamID, targetPiece));
				}
			}

            // check en passant
            Move lastEnemyMove = board.GetLastMove(); // need to move history to GameBoard...
            if (lastEnemyMove != null
                && (lastEnemyMove.destination == piece.Position + new Vector2I(1, 0)
                    || lastEnemyMove.destination == piece.Position + new Vector2I(-1, 0)))
            {
                targetPiece = board.GetPieceAt(lastEnemyMove.destination);
                Debug.Assert(targetPiece != null);

                if (targetPiece.PieceType == ePieceType.PAWN)
                {
                    Vector2I moveDelta = lastEnemyMove.destination - lastEnemyMove.origin;
                    if (Utilities.Abs(moveDelta.y) == 2
                        && board.GetPieceAt(lastEnemyMove.destination + piece.MoveDirection) == null)
                    {
                        moves.Add(new Move(piece.Position, lastEnemyMove.destination + piece.MoveDirection, piece.TeamID, targetPiece, true));
                    }
                }
            }

            // TODO: check promotion (how to represent in move?)

            return moves;
        }
    }
}
