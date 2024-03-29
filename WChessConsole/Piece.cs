﻿using System.Collections.Generic;
using System.Diagnostics;

namespace WChessConsole
{
	enum ePieceType { PAWN, KNIGHT, BISHOP, ROOK, QUEEN, KING };

	class Piece
	{

        // Public member variables
        public readonly ePieceType PieceType;
		public readonly uint TeamID;
        public readonly Vector2I MoveDirection;

        // Public properties
		public bool Active
		{
			get { return active; }
		}

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

		private bool active = true;
        private HashSet<Move> potentialMoves;
        private uint numMoves = 0;
		private Vector2I position;

		////////////////////////////////////////////////////////////////////////
		// Constructor
		////////////////////////////////////////////////////////////////////////
        public Piece(uint teamID, ePieceType pieceType, Vector2I position, IAbility ability)
        {
            this.position = position;

            MoveDirection = new Vector2I(0, teamID == 0 ? 1 : -1);
            PieceType = pieceType;
            TeamID = teamID;

            abilityList = new List<IAbility>();
            abilityList.Add(ability);

            potentialMoves = new HashSet<Move>();
        }


        public Piece(uint teamID, ePieceType pieceType, Vector2I position, List<IAbility> abilityList = null)
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
        public void GeneratePotentialMoves(GameBoard board)
        {
			Debug.Assert(Active);

            potentialMoves.Clear();

            foreach (IAbility a in abilityList)
            {
                List<Move> abilityMoves = a.GeneratePotentialMoves(board, this);
                foreach (Move m in abilityMoves)
                    potentialMoves.Add(m);
            }
        }

        public string GetTwoCharRepresentation()
        {
            return string.Format("{0}{1}", TeamID == 0 ? "w" : "b", convertPieceTypeToChar());
        }

		public void Move(Vector2I destination)
		{
			Debug.Assert(Active);

			position = destination;
			++numMoves;
		}

		public void SetActive(bool active)
		{
			this.active = active;
		}

		public bool ThreateningEnemyKing(GameBoard board)
		{
			Vector2I kingPosition = board.GetKingPosition((TeamID + 1) % 2);

			GeneratePotentialMoves(board);
			foreach(Move m in potentialMoves)
			{
				if (m.destination == kingPosition)
					return true;
			}

			return false;
		}

		public void UndoMove(Vector2I origin)
		{
			position = origin;
			--numMoves;
		}

        public Move ValidateMove(GameBoard board, Vector2I destination)
        {
			Debug.Assert(Active);

			GeneratePotentialMoves(board);

            foreach (Move m in potentialMoves)
            {
                if (m.destination == destination)
                    return m;
            }

            return null;
        }

		private char convertPieceTypeToChar()
		{
			switch (PieceType)
			{
				case ePieceType.BISHOP:
					return 'B';
				case ePieceType.KING:
					return 'K';
				case ePieceType.KNIGHT:
					return 'N';
				case ePieceType.PAWN:
					return 'P';
				case ePieceType.QUEEN:
					return 'Q';
				case ePieceType.ROOK:
					return 'R';
				default:
					return ' ';
			}
		}
	}
}
