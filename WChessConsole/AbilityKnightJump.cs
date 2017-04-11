using System.Collections.Generic;

namespace WChessConsole
{
    class AbilityKnightJump : IAbility
    {
        private static readonly Vector2I[] combinations = {
            new Vector2I(2, 1),
            new Vector2I(1, 2),
            new Vector2I(2, -1),
            new Vector2I(1, -2),
            new Vector2I(-2, 1),
            new Vector2I(-1, 2),
            new Vector2I(-2, -1),
            new Vector2I(-1, -2)
        };

        public List<Move> GeneratePotentialMoves(GameBoard board, Piece piece)
        {
            List<Move> moves = new List<Move>();

            for (int i = 0; i < combinations.Length; ++i)
            {
                Vector2I targetPosition = piece.Position + combinations[i];
				if (board.ValidatePosition(targetPosition))
				{
					Piece targetPiece = board.GetPieceAt(targetPosition);
					if (targetPiece == null || targetPiece.TeamID != piece.TeamID)
					    moves.Add(new Move(piece.Position, targetPosition, piece.TeamID, targetPiece));
				}
            }

            return moves;
        }
    }
}
