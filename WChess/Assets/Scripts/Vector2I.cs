///
/// Written based on Unity's Vector2, leaving out methods that don't apply to int-based vectors.
///

using UnityEngine;

namespace WChess
{
    public struct Vector2I
    {
        public int x;
        public int y;

        public Vector2I(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int this[int index]
        {
            get
            {
                switch (index) {
                case 0:
                    return x;
                case 1:
                    return y;
                default:
                    return int.MinValue;
                }
            }

            set
            {
                switch (index)
                {
                case 0:
                    x = value;
                    break;
                case 1:
                    y = value;
                    break;
                default:
                    break;
                }
            }
        }

        public static Vector2I down { get { return new Vector2I(0, -1); } }
        public static Vector2I left { get { return new Vector2I(-1, 0); } }
        public static Vector2I one { get { return new Vector2I(1, 1); } }
        public static Vector2I right { get { return new Vector2I(1, 0); } }
        public static Vector2I up { get { return new Vector2I(0, 1); } }
        public static Vector2I zero { get { return new Vector2I(0, 0); } }

        public float magnitude { get { return Mathf.Sqrt(x * x + y * y); } }
        public float sqrMagnitude { get { return (x * x + y * y); } }

        public static float Distance(Vector2I a, Vector2I b)
        {
            return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2));
        }

        public static Vector2I Max(Vector2I lhs, Vector2I rhs)
        {
            int x = (lhs.x > rhs.x) ? lhs.x : rhs.x;
            int y = (lhs.y > rhs.y) ? lhs.y : rhs.y;
            return new Vector2I(x, y);
        }

        public static Vector2I Min(Vector2I lhs, Vector2I rhs)
        {
            int x = (lhs.x < rhs.x) ? lhs.x : rhs.x;
            int y = (lhs.y < rhs.y) ? lhs.y : rhs.y;
            return new Vector2I(x, y);
        }

        public static Vector2I Scale(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.x * b.x, a.y * b.y);
        }

        public static float SqrMagnitude(Vector2I a)
        {
            return (a.x * a.x) + (a.y * a.y);
        }

        public override bool Equals(object other)
        {
            if (other.GetType() == GetType())
            {
                return (Vector2I)other == this;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() * (y.GetHashCode() << 2);
        }

        public void Scale(Vector2I scale)
        {
            x *= scale.x;
            y *= scale.y;
        }

        public void Set(int newX, int newY)
        {
            x = newX;
            y = newY;
        }

        public int SqrMagnitude()
        {
            return x * x + y * y;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", x, y);
        }

        public string ToString(string format)
        {
            return string.Format(format, x, y);
        }

        public static Vector2I operator +(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.x + b.x, a.y + b.y);
        }

        public static Vector2I operator -(Vector2I a)
        {
            return new Vector2I(-a.x, -a.y);
        }

        public static Vector2I operator -(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.x - b.x, a.y - b.y);
        }

        public static Vector2I operator *(int d, Vector2I a)
        {
            return new Vector2I(a.x * d, a.y * d);
        }

        public static Vector2I operator *(Vector2I a, int d)
        {
            return new Vector2I(a.x * d, a.y * d);
        }

        public static bool operator ==(Vector2I lhs, Vector2I rhs)
        {
            return (lhs.x == rhs.x) && (lhs.y == rhs.y);
        }

        public static bool operator !=(Vector2I lhs, Vector2I rhs)
        {
            return !(lhs == rhs);
        }
    } // struct Vector2I
} // namespace WChess

