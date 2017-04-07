using System;

namespace WChessConsole
{
	class Game
	{
		private Piece[,] board;
		private uint nextTeamID = 0;
		private uint turnNumber = 0;

		////////////////////////////////////////////////////////////////////////
		// Constructor
		////////////////////////////////////////////////////////////////////////

		public Game(uint width, uint height)
		{
			board = new Piece[width, height];

			for (uint x = 0; x < width; ++x)
			{
				board[x, 1] = new Pawn(this, 0, new Vector2UI(x, 1));
				board[x, height - 2] = new Pawn(this, 1, new Vector2UI(x, height - 2));
			}
		}

		////////////////////////////////////////////////////////////////////////
		// Public methods
		////////////////////////////////////////////////////////////////////////

		public Piece GetPiece(uint x, uint y)
		{
			return validatePosition(x, y) ? board[x, y] : null;
		}

		public Piece GetPiece(Vector2UI position)
		{
			return validatePosition(position) ? board[position.x, position.y] : null;
		}

		public bool MakeMove(string move)
		{
			Console.WriteLine("  Unimplemented input format!");
			return false;
		}

		public bool MakeMove(Vector2UI origin, Vector2UI destination)
		{
			Move move;
			Piece piece;

			if ((piece = GetPiece(origin)) != null
				&& piece.teamID == nextTeamID
				&& (move = piece.ValidateMove(destination)) != null)
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
			string emptyRow = "";
			string result = "";
			string[,] pieces = new string[width, height];

			// fill in the empty row
			emptyRow = "|";
			for (int i = 0; i < width; ++i)
				emptyRow += "--|";

			// Print whose turn it is
			result += string.Format("Turn {0}: {1} to move\n", turnNumber + 1, nextTeamID == 0 ? "white" : "black");

			// convert each piece into its character representation
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Piece piece = GetPiece((uint)x, (uint)y);

					if (piece != null)
					{
						pieces[x, y] = piece.GetTwoCharRepresentation();
					}
					else
					{
						pieces[x, y] = "  ";
					}
				}
			}

			for (int y = height - 1; y >= 0; --y)
			{
				result += "  " + emptyRow + "\n";
				result += (y + 1) + " |";

				for (int x = 0; x < width; ++x)
				{
					result += pieces[x, y] + "|";
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

		private bool validatePosition(uint x, uint y)
		{
			return x < board.GetLength(0) && y < board.GetLength(1);
		}

		private bool validatePosition(Vector2UI position)
		{
			return position.x < board.GetLength(0) 
				&& position.y < board.GetLength(1);
		}
	}
}
