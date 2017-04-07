using System.Collections.Generic;

namespace WChessConsole
{
	class Piece
	{
        // Public member variables
        public readonly char PieceType;
		public readonly uint TeamID;
        public readonly Vector2I MoveDirection;

        // Public properties
        public uint NumMoves
        {
            get { return numMoves; }
        }

        public Vector2I Position
        {
            get { return position; }
        }

		// Private member variables
        private readonly List<IAbility> abilityList;

        private HashSet<Move> potentialMoves;
        private uint numMoves = 0;
		private Vector2I position;

		////////////////////////////////////////////////////////////////////////
		// Constructor
		////////////////////////////////////////////////////////////////////////
        public Piece(uint teamID, char pieceType, Vector2I position, IAbility ability)
        {
            this.position = position;

            MoveDirection = new Vector2I(0, teamID == 0 ? 1 : -1);
            PieceType = pieceType;
            TeamID = teamID;

            abilityList = new List<IAbility>();
            abilityList.Add(ability);

            potentialMoves = new HashSet<Move>();
        }


        public Piece(uint teamID, char pieceType, Vector2I position, List<IAbility> abilityList = null)
		{
            this.abilityList = (abilityList != null) ? abilityList : new List<IAbility>();
			this.position = position;

            MoveDirection = new Vector2I(0, teamID == 0 ? 1 : -1);
            PieceType = pieceType;
			TeamID = teamID;

            potentialMoves = new HashSet<Move>();
		}

		////////////////////////////////////////////////////////////////////////
		// Public functions
		////////////////////////////////////////////////////////////////////////
        public void GeneratePotentialMoves(Game game)
        {
            potentialMoves.Clear();

            foreach (IAbility a in abilityList)
            {
                List<Move> abilityMoves = a.GeneratePotentialMoves(game, this);
                foreach (Move m in abilityMoves)
                    potentialMoves.Add(m);
            }
        }

        public string GetTwoCharRepresentation()
        {
            return string.Format("{0}{1}", TeamID == 0 ? "w" : "b", PieceType);
        }

		public void Move(Vector2I destination)
		{
			position = destination;
			++numMoves;
		}

        public Move ValidateMove(Game game, Vector2I destination)
        {
            GeneratePotentialMoves(game);

            foreach (Move m in potentialMoves)
            {
                if (m.destination == destination)
                    return m;
            }

            return null;
        }
	}
}
