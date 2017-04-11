using System.Collections.Generic;

namespace WChessConsole
{
    class AbilityCastle : IAbility
    {
        private static readonly Vector2I[] combinations = {
            new Vector2I(2, 1),
            new Vector2I(1, 2),
            new Vector2I(2, -1),
            new Vector2I(1, -2),
            new Vector2I(-2, 1),
            new Vector2I(-1, 2),
            new Vector2I(-2, -1),
            new Vector2I(-1, -2)
        };

        public List<Move> GeneratePotentialMoves(GameBoard board, Piece piece)
        {
            bool valid;
            List<Move> moves = new List<Move>();
            Piece rook;

            if (piece.PieceType == ePieceType.KING && piece.NumMoves == 0)
            {
                // Check kingside castle
                valid = true;
                rook = board.GetPieceAt(7, piece.Position.y);
                if (rook.TeamID == piece.TeamID && rook.NumMoves == 0)
                {
                    for (int i = 1; i <= 2; ++i)
                    {
                        if (board.GetPieceAt(piece.Position + new Vector2I(i, 0)) != null)
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (valid)
                    {
                        moves.Add(new Move(piece.Position, piece.Position + new Vector2I(2, 0), piece.TeamID, null, false, true));
                    }
                }

                // Check queenside castle
                valid = true;
                rook = board.GetPieceAt(0, piece.Position.y);
                if (rook.TeamID == piece.TeamID && rook.NumMoves == 0)
                {
                    for (int i = 1; i <= 3; ++i)
                    {
                        if (board.GetPieceAt(piece.Position - new Vector2I(i, 0)) != null)
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (valid)
                    {
                        moves.Add(new Move(piece.Position, piece.Position + new Vector2I(-2, 0), piece.TeamID, null, false, false, false));
                    }
                }
            }

            return moves;
        }
    }
}
