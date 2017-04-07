namespace WChessConsole
{
	class Move
	{
		public readonly bool kingsideCastle = false;
		public readonly bool placesEnemyInCheck = false;
		public readonly bool queensideCastle = false;
		public readonly uint teamID;
		public readonly Vector2UI origin;
		public readonly Vector2UI destination;

		public Move(Vector2UI origin, 
		            Vector2UI destination, 
		            uint teamID,
		            bool placesEnemyInCheck = false,
		            bool kingsideCastle = false,
		            bool queensideCastle = false) 
		{
			this.destination = destination;
			this.origin = origin;
			this.teamID = teamID;

			this.placesEnemyInCheck = placesEnemyInCheck;
			this.kingsideCastle = kingsideCastle;
			this.queensideCastle = queensideCastle;
		}
	}
}
