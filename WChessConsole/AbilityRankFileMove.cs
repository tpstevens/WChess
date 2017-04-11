using System.Collections.Generic;

namespace WChessConsole
{
	class AbilityRankFileMove : IAbility
	{
		private static readonly Vector2I[] combinations = {
			new Vector2I(1, 0),
			new Vector2I(0, 1),
			new Vector2I(0, -1),
			new Vector2I(-1, 0)
		};

		private readonly uint distanceLimit;

		public AbilityRankFileMove(uint distanceLimit = uint.MaxValue)
		{
			this.distanceLimit = distanceLimit;
		}

		public List<Move> GeneratePotentialMoves(GameBoard board, Piece piece)
		{
			List<Move> moves = new List<Move>();
			Piece targetPiece;
			uint distanceChecked;
			Vector2I targetPosition;

			for (int i = 0; i < combinations.Length; ++i)
			{
				targetPosition = piece.Position + combinations[i];

				distanceChecked = 0;
				while (board.ValidatePosition(targetPosition) && ++distanceChecked <= distanceLimit)
				{
					targetPiece = board.GetPieceAt(targetPosition);
					if (targetPiece == null || targetPiece.TeamID != piece.TeamID)
					{
						moves.Add(new Move(piece.Position, targetPosition, piece.TeamID, targetPiece));
						if (targetPiece != null)
							break;
					}

					targetPosition += combinations[i];
				}
			}

			return moves;
		}
	}
}
