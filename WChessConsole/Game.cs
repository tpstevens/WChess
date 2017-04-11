using System;

namespace WChessConsole
{
	class Game
	{
		private GameBoard board;
		private uint nextTeamID = 0;
		private uint turnNumber = 0;

		////////////////////////////////////////////////////////////////////////
		// Constructor
		////////////////////////////////////////////////////////////////////////

		public Game(uint width, uint height)
		{
			board = new GameBoard(width, height);
		}

		////////////////////////////////////////////////////////////////////////
		// Public methods
		////////////////////////////////////////////////////////////////////////

        public void CalculatePotentialMoves()
        {
            // TODO - for displaying threats...
        }

		public bool PlayerMove(string move)
		{
			Console.WriteLine("  Unimplemented input format!");
			return false;
		}

		public bool PlayerMove(Vector2I origin, Vector2I destination)
		{
			bool validMove = true;
			Move move;
			Piece piece;

			if ((piece = board.GetPieceAt(origin)) != null
				&& piece.TeamID == nextTeamID
				&& (move = piece.ValidateMove(board, destination)) != null)
			{
				if (board.MakeTemporaryMove(move))
				{
					uint enemyID = (nextTeamID + 1) % 2;

					foreach (Piece p in board.GetPieces(enemyID))
					{
						if (p.Active && p.ThreateningEnemyKing(board))
						{
							Console.WriteLine("Invalid move: piece " + p.GetTwoCharRepresentation() + " is threatening the king");
							validMove = false;
						}
					}

					if (validMove)
					{
						board.ConvertTemporaryMoveToActual();
						nextTeamID = (nextTeamID + 1) % 2;
						if (nextTeamID == 0)
							++turnNumber;
					}
					else
					{
						board.UndoTemporaryMove();
					}
				}

				return validMove;
			}

			return false;
		}

		public override string ToString()
		{
			Piece piece;
			string emptyRow = "";
			string result = "";

			// fill in the empty row
			emptyRow = "|";
			for (int i = 0; i < board.Width; ++i)
				emptyRow += "--|";

			// print whose turn it is
			result += string.Format("Turn {0}: {1} to move\n", turnNumber + 1, nextTeamID == 0 ? "white" : "black");

			for (int y = (int)board.Height - 1; y >= 0; --y)
			{
				result += "  " + emptyRow + "\n";
				result += (y + 1) + " |";

				for (int x = 0; x < board.Width; ++x)
				{
					piece = board.GetPieceAt(x, y);
					result += string.Format("{0}|", piece != null ? piece.GetTwoCharRepresentation() : "  ");
				}

				result += "\n";
			}

			result += "  " + emptyRow + "\n";
			result += "   a  b  c  d  e  f  g  h \n";

			return result;
		}

		public bool UndoPlayerMove()
		{
			if (board.UndoPlayerMove())
			{
				nextTeamID = (nextTeamID + 1) % 2;
				if (nextTeamID != 0)
					--turnNumber;

				return true;
			}
			else
			{
				return false;
			}
		}

		////////////////////////////////////////////////////////////////////////
		// Private helper methods
		////////////////////////////////////////////////////////////////////////
	}
}
