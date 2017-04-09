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

			// add pawns
			for (int x = 0; x < width; ++x)
			{
				board[x, 1] = PieceFactory.CreateClassicPawn(0, new Vector2I(x, 1));
				board[x, height - 2] = PieceFactory.CreateClassicPawn(1, new Vector2I(x, (int)height - 2));
			}

			// add knights
			board[1, 0] = PieceFactory.CreateClassicKnight(0, new Vector2I(1, 0));
			board[6, 0] = PieceFactory.CreateClassicKnight(0, new Vector2I(6, 0));
			board[1, 7] = PieceFactory.CreateClassicKnight(1, new Vector2I(1, 7));
			board[6, 7] = PieceFactory.CreateClassicKnight(1, new Vector2I(6, 7));

			// add bishops
			board[2, 0] = PieceFactory.CreateClassicBishop(0, new Vector2I(2, 0));
			board[5, 0] = PieceFactory.CreateClassicBishop(0, new Vector2I(5, 0));
			board[2, 7] = PieceFactory.CreateClassicBishop(1, new Vector2I(2, 7));
			board[5, 7] = PieceFactory.CreateClassicBishop(1, new Vector2I(5, 7));

			// add rooks
			board[0, 0] = PieceFactory.CreateClassicRook(0, new Vector2I(0, 0));
			board[7, 0] = PieceFactory.CreateClassicRook(0, new Vector2I(7, 0));
			board[0, 7] = PieceFactory.CreateClassicRook(1, new Vector2I(0, 7));
			board[7, 7] = PieceFactory.CreateClassicRook(1, new Vector2I(7, 7));

			// add queens
			board[3, 0] = PieceFactory.CreateClassicQueen(0, new Vector2I(3, 0));
			board[3, 7] = PieceFactory.CreateClassicQueen(1, new Vector2I(3, 7));

			// add kings
			board[4, 0] = PieceFactory.CreateClassicKing(0, new Vector2I(4, 0));
			board[4, 7] = PieceFactory.CreateClassicKing(1, new Vector2I(4, 7));
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
			return ValidatePosition(position) ? board[position.x, position.y] : null;
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
				// Move current piece
				piece.Move(destination);
				board[destination.x, destination.y] = piece;
				board[origin.x, origin.y] = null;

                // Handle en passant capture
                if (move.enPassant)
                {
                    board[GetLastMove().destination.x, GetLastMove().destination.y] = null;
                }

                // Handle castling
                if (move.kingsideCastle)
                {
                    Piece rook = GetPiece(7, move.destination.y);
                    Vector2I rookDestination = new Vector2I(5, move.destination.y);
                    rook.Move(rookDestination);
                    board[rookDestination.x, rookDestination.y] = rook;
                    board[7, move.destination.y] = null;
                }
                else if (move.queensideCastle)
                {
                    Piece rook = GetPiece(0, move.destination.y);
                    Vector2I rookDestination = new Vector2I(3, move.destination.y);
                    rook.Move(rookDestination);
                    board[rookDestination.x, rookDestination.y] = rook;
                    board[0, move.destination.y] = null;
                }

				nextTeamID = (nextTeamID + 1) % 2;
				if (nextTeamID == 0)
					++turnNumber;

                moveHistory.Push(move);

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

		public bool ValidatePosition(int x, int y)
		{
			return x >= 0 && x < board.GetLength(0)
				&& y >= 0 && y < board.GetLength(1);
		}

		public bool ValidatePosition(Vector2I position)
		{
			return position.x >= 0 && position.x < board.GetLength(0)
				&& position.y >= 0 && position.y < board.GetLength(1);
		}
	}
}
