using System.Collections.Generic;

namespace WChessConsole
{
	class GameBoard
	{
		public uint Height { get; }
		public uint Width { get; }

		private static readonly string LOGTAG = "GameBoard";

		private List<Piece>[] pieces;
		private Move temporaryMove;
		private Piece[,] board;
		private Stack<Move> moveHistory;
		private Vector2I[] kingPositions;

		////////////////////////////////////////////////////////////////////////
		// Constructor
		////////////////////////////////////////////////////////////////////////

		public GameBoard(uint width, uint height)
		{
			EventLog.a(height != 0 && width != 0, 
			           LOGTAG, 
					   string.Format("Invalid height({0}) or width({1})", height, width));

			Height = height;
			Width = width;

			initializeClassicGame();
		}

		////////////////////////////////////////////////////////////////////////
		// Public methods
		////////////////////////////////////////////////////////////////////////

		public bool ConvertTemporaryMoveToActual()
		{
			if (temporaryMove == null)
				return false;

			moveHistory.Push(temporaryMove);
			temporaryMove = null;
			return false;
		}

		public Vector2I GetKingPosition(uint teamID)
		{
			if (teamID >= kingPositions.Length)
				return new Vector2I(int.MinValue, int.MinValue);

			return kingPositions[teamID];
		}

		public Move GetLastMove()
		{
			return moveHistory.Count > 0 ? moveHistory.Peek() : null;
		}

		public Piece GetPieceAt(int x, int y)
		{
			EventLog.a(ValidatePosition(x, y),
					   LOGTAG,
					   string.Format("Cannot get piece at invalid position ({0}, {1})", x, y));

			return board[x, y];
		}

		public Piece GetPieceAt(Vector2I position)
		{
			return GetPieceAt(position.x, position.y);
		}

		public List<Piece> GetPieces(uint teamID)
		{
			if (teamID <= pieces.Length)
				return pieces[teamID];
			else
				return new List<Piece>();
		}

		public bool MakeTemporaryMove(Move move)
		{
			if (move == null || !executeMove(move))
				return false;

			temporaryMove = move;
			return true;
		}

		public bool UndoPlayerMove()
		{
			if (moveHistory.Count != 0 && temporaryMove == null)
			{
				return undoExecuteMove(moveHistory.Pop());
			}
			else
			{
				return false;
			}
		}

		public bool UndoTemporaryMove()
		{
			if (temporaryMove == null)
				return false;

			if (undoExecuteMove(temporaryMove))
			{
				temporaryMove = null;
				return true;
			}
			else
			{
				EventLog.e(LOGTAG, "Failed to undo temporary move!");
				return false;
			}
		}

		public bool ValidatePosition(int x, int y)
		{
			return x >= 0 && x < Width && y >= 0 && y < Height;
		}

		public bool ValidatePosition(Vector2I position)
		{
			return ValidatePosition(position.x, position.y);
		}

		////////////////////////////////////////////////////////////////////////
		// Private helper methods
		////////////////////////////////////////////////////////////////////////

		private bool executeMove(Move move)
		{
			EventLog.a(temporaryMove == null,
				       LOGTAG,
				       "Cannot move pieces while a temporary move has been applied!");

			Piece piece = GetPieceAt(move.origin);
			EventLog.d(LOGTAG,
				      string.Format("moving {0} from {1} to {2}", piece.PieceType, move.origin, move.destination));

			// Move current piece
			movePieceAt(move.origin, move.destination);

			// Handle en passant capture
			if (move.enPassant)
			{
				removePieceAt(GetLastMove().destination);
			}

			// Handle castling
			if (move.kingsideCastle)
			{
				Vector2I rookOrigin = new Vector2I(7, move.destination.y);
				Vector2I rookDestination = new Vector2I(5, move.destination.y);
				movePieceAt(rookOrigin, rookDestination);
			}
			else if (move.queensideCastle)
			{
				Vector2I rookOrigin = new Vector2I(0, move.destination.y);
				Vector2I rookDestination = new Vector2I(3, move.destination.y);
				movePieceAt(rookOrigin, rookDestination);
			}

			if (move.capturedPiece != null)
				move.capturedPiece.SetActive(false);

			return true;
		}

		private void initializeClassicGame()
		{
			pieces = new List<Piece>[2];
			pieces[0] = new List<Piece>();
			pieces[1] = new List<Piece>();

			temporaryMove = null;
			board = new Piece[Width, Height];
			moveHistory = new Stack<Move>();
			kingPositions = new Vector2I[2];

			// add pawns
			for (int x = 0; x < Width; ++x)
			{
				pieces[0].Add(PieceFactory.CreateClassicPawn(0, new Vector2I(x, 1)));
				pieces[1].Add(PieceFactory.CreateClassicPawn(1, new Vector2I(x, (int)Height - 2)));
			}

			// add knights
			pieces[0].Add(PieceFactory.CreateClassicKnight(0, new Vector2I(1, 0)));
			pieces[0].Add(PieceFactory.CreateClassicKnight(0, new Vector2I(6, 0)));
			pieces[1].Add(PieceFactory.CreateClassicKnight(1, new Vector2I(1, 7)));
			pieces[1].Add(PieceFactory.CreateClassicKnight(1, new Vector2I(6, 7)));

			// add bishops
			pieces[0].Add(PieceFactory.CreateClassicBishop(0, new Vector2I(2, 0)));
			pieces[0].Add(PieceFactory.CreateClassicBishop(0, new Vector2I(5, 0)));
			pieces[1].Add(PieceFactory.CreateClassicBishop(1, new Vector2I(2, 7)));
			pieces[1].Add(PieceFactory.CreateClassicBishop(1, new Vector2I(5, 7)));

			// add rooks
			pieces[0].Add(PieceFactory.CreateClassicRook(0, new Vector2I(0, 0)));
			pieces[0].Add(PieceFactory.CreateClassicRook(0, new Vector2I(7, 0)));
			pieces[1].Add(PieceFactory.CreateClassicRook(1, new Vector2I(0, 7)));
			pieces[1].Add(PieceFactory.CreateClassicRook(1, new Vector2I(7, 7)));

			// add queens
			pieces[0].Add(PieceFactory.CreateClassicQueen(0, new Vector2I(3, 0)));
			pieces[1].Add(PieceFactory.CreateClassicQueen(1, new Vector2I(3, 7)));

			// add kings
			pieces[0].Add(PieceFactory.CreateClassicKing(0, new Vector2I(4, 0)));
			pieces[1].Add(PieceFactory.CreateClassicKing(1, new Vector2I(4, 7)));

			// Set all piece positions
			for (int i = 0; i < pieces.Length; ++i)
			{
				foreach (Piece p in pieces[i])
					setPieceAt(p.Position, p);
			}
		}

		private void movePieceAt(Vector2I origin, Vector2I destination)
		{
			Piece piece = removePieceAt(origin);
			if (piece != null)
				piece.Move(destination);
			setPieceAt(destination, piece);
		}

		private Piece removePieceAt(Vector2I position)
		{
			return removePieceAt(position.x, position.y);
		}

		private Piece removePieceAt(int x, int y)
		{
			EventLog.a(ValidatePosition(x, y),
					   LOGTAG,
					   string.Format("Cannot clear invalid position ({0}, {1})", x, y));

			Piece lastPiece = board[x, y];
			board[x, y] = null;
			return lastPiece;
		}

		private Piece setPieceAt(int x, int y, Piece piece)
		{
			EventLog.a(ValidatePosition(x, y),
					   LOGTAG,
					   string.Format("Cannot set invalid position ({0}, {1})", x, y));

			Piece lastPiece = board[x, y];
			board[x, y] = piece;

			if (piece.PieceType == ePieceType.KING)
				kingPositions[piece.TeamID] = piece.Position;

			return lastPiece;
		}

		private Piece setPieceAt(Vector2I position, Piece piece)
		{
			return setPieceAt(position.x, position.y, piece);
		}

		private bool undoExecuteMove(Move move)
		{
			// Handle castling
			if (move.kingsideCastle)
			{
				Vector2I currentRookPosition = new Vector2I(5, move.destination.y);
				Vector2I previousRookPosition = new Vector2I(7, move.destination.y);
				undoMovePieceAt(currentRookPosition, previousRookPosition);
			}
			else if (move.queensideCastle)
			{
				Vector2I currentRookPosition = new Vector2I(4, move.destination.y);
				Vector2I previousRookPosition = new Vector2I(0, move.destination.y);
				undoMovePieceAt(currentRookPosition, previousRookPosition);
			}

			// Move current piece
			Piece piece = GetPieceAt(move.destination);
			EventLog.d(LOGTAG,
					  string.Format("moving {0} from {1} to {2}", piece.PieceType, move.destination, move.origin));
			
			undoMovePieceAt(move.destination, move.origin);

			if (move.capturedPiece != null)
			{
				setPieceAt(move.capturedPiece.Position, move.capturedPiece);
				move.capturedPiece.SetActive(true);
			}

			return true;
		}

		private void undoMovePieceAt(Vector2I currentPosition, Vector2I previousPosition)
		{
			Piece piece = removePieceAt(currentPosition);
			if (piece != null)
				piece.UndoMove(previousPosition);
			setPieceAt(previousPosition, piece);
		}
	}
}
