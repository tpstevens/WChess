﻿using System.Collections.Generic;

namespace WChessConsole
{
    class PieceFactory
    {
        private static readonly AbilityCastle kingCastle = new AbilityCastle();
		private static readonly AbilityDiagonalMove diagonalLimited = new AbilityDiagonalMove(1);
		private static readonly AbilityDiagonalMove diagonalUnlimited = new AbilityDiagonalMove();
		private static readonly AbilityKnightJump knightJump = new AbilityKnightJump();
		private static readonly AbilityPawn pawnAbilities = new AbilityPawn();
		private static readonly AbilityRankFileMove rankFileLimited = new AbilityRankFileMove(1);
		private static readonly AbilityRankFileMove rankFileUnlimited = new AbilityRankFileMove();

		public static Piece CreateClassicBishop(uint teamID, Vector2I position)
		{
			return new Piece(teamID, ePieceType.BISHOP, position, diagonalUnlimited);
		}

		public static Piece CreateClassicKing(uint teamID, Vector2I position)
		{
			List<IAbility> abilityList = new List<IAbility>();
			abilityList.Add(diagonalLimited);
			abilityList.Add(rankFileLimited);
            abilityList.Add(kingCastle);
			return new Piece(teamID, ePieceType.KING, position, abilityList);
		}

		public static Piece CreateClassicKnight(uint teamID, Vector2I position)
		{
			return new Piece(teamID, ePieceType.KNIGHT, position, knightJump);
		}

        public static Piece CreateClassicPawn(uint teamID, Vector2I position)
        {
            return new Piece(teamID, ePieceType.PAWN, position, pawnAbilities);
        }

		public static Piece CreateClassicQueen(uint teamID, Vector2I position)
		{
			List<IAbility> abilityList = new List<IAbility>();
			abilityList.Add(diagonalUnlimited);
			abilityList.Add(rankFileUnlimited);
			return new Piece(teamID, ePieceType.QUEEN, position, abilityList);
		}

		public static Piece CreateClassicRook(uint teamID, Vector2I position)
		{
			return new Piece(teamID, ePieceType.ROOK, position, rankFileUnlimited);
		}
    }
}
