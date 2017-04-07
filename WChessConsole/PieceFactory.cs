namespace WChessConsole
{
    class PieceFactory
    {
        public static Piece CreatePawn(uint teamID, Vector2I position)
        {
            return new Piece(teamID, 'P', position, new AbilityPawn());
        }
    }
}
