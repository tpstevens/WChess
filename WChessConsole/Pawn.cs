using System;

namespace WChessConsole
{
	class Pawn : Piece
	{
		public Pawn(Game game, uint teamID, Vector2UI initialPosition)
			: base(game, teamID, initialPosition)
		{
			// Intentionally empty
		}

		public override String GetTwoCharRepresentation()
		{
			string result = teamID == 0 ? "w" : "b";
			return result + "P";
		}

		public override Move ValidateMove(Vector2UI destination)
		{
			int deltaX = (int)(destination.x - position.x);
			int deltaY = (int)(destination.y - position.y);
			int dirY = (teamID == 0) ? 1 : -1;

			// Check positions in same file
			if (deltaX == 0 && game.GetPiece(position.x, (uint)(position.y + dirY)) == null)
			{
				if (deltaY == dirY)
				{
					return new Move(position, destination, teamID);
				}
				else if (deltaY == 2 * dirY && game.GetPiece(destination) == null)
				{
					return new Move(position, destination, teamID);
				}
			}
			else if (Utilities.Abs(deltaX) == 1 
				&& deltaY == dirY
				&& game.GetPiece(destination) != null
				&& game.GetPiece(destination).teamID != teamID)
			{
				return new Move(position, destination, teamID);
			}

			return null;
		}
	}
}
