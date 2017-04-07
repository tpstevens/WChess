using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WChess.Chess
{
    public struct Move
    {
        public readonly Vector2I origin;
        public readonly Vector2I destination;

        public Move(Vector2I origin, Vector2I destination)
        {
            this.origin = origin;
            this.destination = destination;
        }

        public override bool Equals(object obj)
        {
            if (obj is Move)
            {
                Move move = (Move)obj;
                return (origin == move.origin) && (destination == move.destination);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return origin.GetHashCode() ^ (destination.GetHashCode() << 2);
        }

        public override string ToString()
        {
            // TODO
            return base.ToString();
        }
    }
}
