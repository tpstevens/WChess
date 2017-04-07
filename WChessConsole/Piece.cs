namespace WChessConsole
{
	abstract class Piece
	{
		// Public member variables
		public readonly uint teamID;

		// Protected member variables
		protected Game game;
		protected uint numMoves = 0;
		protected Vector2UI position;

		////////////////////////////////////////////////////////////////////////
		// Constructor
		////////////////////////////////////////////////////////////////////////
		public Piece(Game game, uint teamID, Vector2UI initialPosition)
		{
			this.game = game;
			this.teamID = teamID;
			position = initialPosition;
		}

		////////////////////////////////////////////////////////////////////////
		// Base member functions
		////////////////////////////////////////////////////////////////////////
		public virtual void Move(Vector2UI destination)
		{
			position = destination;
			++numMoves;
		}

		////////////////////////////////////////////////////////////////////////
		// Abstract member functions
		////////////////////////////////////////////////////////////////////////
		public abstract string GetTwoCharRepresentation();
		public abstract Move ValidateMove(Vector2UI destination);
	}
}
