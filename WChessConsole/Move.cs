namespace WChessConsole
{
	class Move
	{
        public readonly bool enPassant;
		public readonly bool kingsideCastle;
		public readonly bool queensideCastle;
		public readonly Piece capturedPiece;
		public readonly uint teamID;
		public readonly Vector2I origin;
		public readonly Vector2I destination;

		public Move(Vector2I origin, 
		            Vector2I destination, 
		            uint teamID,
					Piece capturedPiece = null,
                    bool enPassant = false,
		            bool kingsideCastle = false,
		            bool queensideCastle = false) 
		{
			this.destination = destination;
			this.origin = origin;
			this.teamID = teamID;

			this.capturedPiece = capturedPiece;
            this.enPassant = enPassant;
			this.kingsideCastle = kingsideCastle;
			this.queensideCastle = queensideCastle;
		}
	}

    // add "string ToString(bool check, bool checkMate)";
    // doesn't take into account special notation (avoiding pawn names, specifying files, etc)
    // create factory for generating move string from a Move and a Game's current state?
}
