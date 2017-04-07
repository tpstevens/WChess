using System;
using System.Collections.Generic;

namespace WChessConsole
{
	class Game
	{
		private Piece[,] board;
        private Stack<Move> moveHistory;
		private uint nextTeamID = 0;
		private uint turnNumber = 0;

		////////////////////////////////////////////////////////////////////////
		// Constructor
		////////////////////////////////////////////////////////////////////////

		public Game(uint width, uint height)
		{
			board = new Piece[width, height];
            moveHistory = new Stack<Move>();

			for (int x = 0; x < width; ++x)
			{
				board[x, 1] = PieceFactory.CreatePawn(0, new Vector2I(x, 1));
				board[x, height - 2] = PieceFactory.CreatePawn(1, new Vector2I(x, (int)height - 2));
			}
		}

		////////////////////////////////////////////////////////////////////////
		// Public methods
		////////////////////////////////////////////////////////////////////////

        public void CalculatePotentialMoves()
        {
            // TODO - for displaying threats...
        }

        public Move GetLastMove()
        {
            return moveHistory.Count > 0 ? moveHistory.Peek() : null;
        }

        public Piece GetPiece(int x, int y)
        {
            return board[x, y];
        }

		public Piece GetPiece(Vector2I position)
		{
			return validatePosition(position) ? board[position.x, position.y] : null;
		}

		public bool MakeMove(string move)
		{
			Console.WriteLine("  Unimplemented input format!");
			return false;
		}

		public bool MakeMove(Vector2I origin, Vector2I destination)
		{
			Move move;
			Piece piece;

			if ((piece = GetPiece(origin)) != null
				&& piece.TeamID == nextTeamID
				&& (move = piece.ValidateMove(this, destination)) != null)
			{
				// Remove enemy piece if captured - TODO
				
				// Move current piece
				piece.Move(destination);
				board[destination.x, destination.y] = piece;
				board[origin.x, origin.y] = null;

				nextTeamID = (nextTeamID + 1) % 2;
				if (nextTeamID == 0)
					++turnNumber;

				return true;
			}

			return false;
		}

		public override string ToString()
		{
			int width = board.GetLength(0);
			int height = board.GetLength(1);
            Piece piece;
			string emptyRow = "";
			string result = "";

			// fill in the empty row
			emptyRow = "|";
			for (int i = 0; i < width; ++i)
				emptyRow += "--|";

			// print whose turn it is
			result += string.Format("Turn {0}: {1} to move\n", turnNumber + 1, nextTeamID == 0 ? "white" : "black");

			for (int y = height - 1; y >= 0; --y)
			{
				result += "  " + emptyRow + "\n";
				result += (y + 1) + " |";

				for (int x = 0; x < width; ++x)
				{
                    piece = GetPiece(x, y);
                    result += string.Format("{0}|", piece != null ? piece.GetTwoCharRepresentation() : "  ");
				}

				result += "\n";
			}

			result += "  " + emptyRow + "\n";
			result += "   a  b  c  d  e  f  g  h \n";

			return result;
		}

		////////////////////////////////////////////////////////////////////////
		// Private methods
		////////////////////////////////////////////////////////////////////////

		private bool validatePosition(int x, int y)
		{
			return x >= 0 && x < board.GetLength(0) 
                && y >= 0 && y < board.GetLength(1);
		}

		private bool validatePosition(Vector2I position)
		{
			return position.x >= 0 && position.x < board.GetLength(0) 
				&& position.y >= 0 && position.y < board.GetLength(1);
		}
	}
}
