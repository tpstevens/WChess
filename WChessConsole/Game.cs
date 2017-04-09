﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WChessConsole
{
	class Game
	{
		private Move temporaryMove;
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
			temporaryMove = null;

			// add pawns
			for (int x = 0; x < width; ++x)
			{
				setPosition(x, 1, PieceFactory.CreateClassicPawn(0, new Vector2I(x, 1)));
				setPosition(x, (int)height - 2, PieceFactory.CreateClassicPawn(1, new Vector2I(x, (int)height - 2)));
			}

			// add knights
			setPosition(1, 0, PieceFactory.CreateClassicKnight(0, new Vector2I(1, 0)));
			setPosition(6, 0, PieceFactory.CreateClassicKnight(0, new Vector2I(6, 0)));
			setPosition(1, 7, PieceFactory.CreateClassicKnight(1, new Vector2I(1, 7)));
			setPosition(6, 7, PieceFactory.CreateClassicKnight(1, new Vector2I(6, 7)));

			// add bishops
			setPosition(2, 0, PieceFactory.CreateClassicBishop(0, new Vector2I(2, 0)));
			setPosition(5, 0, PieceFactory.CreateClassicBishop(0, new Vector2I(5, 0)));
			setPosition(2, 7, PieceFactory.CreateClassicBishop(1, new Vector2I(2, 7)));
			setPosition(5, 7, PieceFactory.CreateClassicBishop(1, new Vector2I(5, 7)));

			// add rooks
			setPosition(0, 0, PieceFactory.CreateClassicRook(0, new Vector2I(0, 0)));
			setPosition(7, 0, PieceFactory.CreateClassicRook(0, new Vector2I(7, 0)));
			setPosition(0, 7, PieceFactory.CreateClassicRook(1, new Vector2I(0, 7)));
			setPosition(7, 7, PieceFactory.CreateClassicRook(1, new Vector2I(7, 7)));

			// add queens
			setPosition(3, 0, PieceFactory.CreateClassicQueen(0, new Vector2I(3, 0)));
			setPosition(3, 7, PieceFactory.CreateClassicQueen(1, new Vector2I(3, 7)));

			// add kings
			setPosition(4, 0, PieceFactory.CreateClassicKing(0, new Vector2I(4, 0)));
			setPosition(4, 7, PieceFactory.CreateClassicKing(1, new Vector2I(4, 7)));
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

		public bool PlayerMove(string move)
		{
			Console.WriteLine("  Unimplemented input format!");
			return false;
		}

		public bool PlayerMove(Vector2I origin, Vector2I destination)
		{
			Move move;
			Piece piece;

			if ((piece = GetPiece(origin)) != null
				&& piece.TeamID == nextTeamID
				&& (move = piece.ValidateMove(this, destination)) != null
				&& movePieces(move))
			{
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

		public bool UndoPlayerMove()
		{
			if (moveHistory.Count == 0)
			{
				return false;
			}
			else if (temporaryMove != null)
			{
				Console.WriteLine("Cannot undo the player's move when there's a temporary move queued...");
				return false;
			}

			nextTeamID = (nextTeamID + 1) % 2;

			return undoMovePieces(moveHistory.Pop());
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

		////////////////////////////////////////////////////////////////////////
		// Private helper methods
		////////////////////////////////////////////////////////////////////////

		private void clearPosition(int x, int y)
		{
			Debug.Assert(ValidatePosition(x, y));
			board[x, y] = null;
		}

		private void clearPosition(Vector2I v)
		{
			clearPosition(v.x, v.y);
		}

		private bool MakeTemporaryMove(Move move)
		{
			if (move == null)
				return false;

			// TODO implement
			return false;
		}

		private bool movePieces(Move move)
		{
			Piece piece = GetPiece(move.origin);
			if (piece == null)
				return false;

			Console.WriteLine(string.Format("moving {0} from {1} to {2}", piece.PieceType, move.origin, move.destination));

			// Move current piece
			piece.Move(move.destination);
			setPosition(move.destination, piece);
			clearPosition(move.origin);

			// Handle en passant capture
			if (move.enPassant)
			{
				clearPosition(GetLastMove().destination);
			}

			// Handle castling
			if (move.kingsideCastle)
			{
				Piece rook = GetPiece(7, move.destination.y);
				Vector2I rookDestination = new Vector2I(5, move.destination.y);
				rook.Move(rookDestination);
				setPosition(rookDestination, rook);
				clearPosition(7, move.destination.y);
			}
			else if (move.queensideCastle)
			{
				Piece rook = GetPiece(0, move.destination.y);
				Vector2I rookDestination = new Vector2I(3, move.destination.y);
				rook.Move(rookDestination);
				setPosition(rookDestination, rook);
				clearPosition(0, move.destination.y);
			}

			return true;
		}

		private void setPosition(int x, int y, Piece piece)
		{
			Debug.Assert(ValidatePosition(x, y));
			board[x, y] = piece;
		}

		private void setPosition(Vector2I v, Piece piece)
		{
			setPosition(v.x, v.y, piece);
		}

		private bool undoMovePieces(Move move)
		{
			// Handle castling
			if (move.kingsideCastle)
			{
				Piece rook = GetPiece(5, move.destination.y);
				if (rook == null)
					return false;

				Vector2I rookOrigin = new Vector2I(7, move.destination.y);
				rook.UndoMove(rookOrigin);
				setPosition(rookOrigin, rook);
				clearPosition(5, move.destination.y);
			}
			else if (move.queensideCastle)
			{
				Piece rook = GetPiece(4, move.destination.y);
				if (rook == null)
					return false;

				Vector2I rookOrigin = new Vector2I(0, move.destination.y);
				rook.UndoMove(rookOrigin);
				setPosition(rookOrigin, rook);
				clearPosition(4, move.destination.y);
			}

			// Move current piece
			Piece piece = GetPiece(move.destination);
			if (piece == null)
				return false;

			Console.WriteLine(string.Format("undoing {0} from {1} to {2}", piece.PieceType, move.destination, move.origin));

			piece.UndoMove(move.origin);
			setPosition(move.origin, piece);

			clearPosition(move.destination);
			if (move.capturedPiece != null)
				setPosition(move.capturedPiece.Position, move.capturedPiece);

			return true;
		}

		private bool undoTemporaryMove()
		{
			if (temporaryMove == null)
				return false;

			if (undoMovePieces(temporaryMove))
				temporaryMove = null;

			return false;
		}
	}
}
