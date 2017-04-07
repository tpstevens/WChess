using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WChess.Chess
{
    public class Board
    {
        private Piece[,] gameboard;

        Board(int numRows, int numColumns)
        {
            gameboard = new Piece[numRows,numColumns];
            Vector2 i;
        }

        bool isPositionEmpty(Vector2I position)
        {
            assert(validPosition(position));

            return gameboard[position.x][position.y] == null;
        }

        bool isPositionOccupiedByTeam(Vector2I position, uint teamID)
        {
            assert(validPosition(position));

            Piece piece = gameboard[position.x][position.y];
            return (piece != null) && (piece.teamID == teamID);
        }

        bool isPositionOccupiedByTeam(Vector2I position, uint[] teamIDs)
        {
            assert(validPosition(position));

            Piece piece = gameboard[position.x][position.y];
            return (piece != null) && (IndexOf(teamIDs, piece.teamID) != -1);
        }

        bool validPosition(Vector2I position)
        {
            return position.x < numRows && position.y < numColumns;
        }
    } // class Board
} // namespace WChess.Chess