namespace WChessConsole
{
	struct Vector2I
	{
		public int x, y;

		public Vector2I(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

        public override bool Equals(object obj)
        {
            return (obj is Vector2I && (this == (Vector2I)(obj)));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", x, y);
        }

        ////////////////////////////////////////////////////////////////////////
        // Static Operators
        ////////////////////////////////////////////////////////////////////////

        public static Vector2I operator+(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.x + b.x, a.y + b.y);
        }

        public static Vector2I operator*(Vector2I v, int i)
        {
            return new Vector2I(v.x * i, v.y * i);
        }

        public static Vector2I operator*(int i, Vector2I v)
        {
            return new Vector2I(v.x * i, v.y * i);
        }

        public static bool operator==(Vector2I a, Vector2I b)
        {
            return (a.x == b.x) && (a.y == b.y);
        }

        public static bool operator!=(Vector2I a, Vector2I b)
        {
            return !(a == b);
        }
    }

	struct Pair<T>
	{
		public T first, second;

		public Pair(T first, T second)
		{
			this.first = first;
			this.second = second;
		}
	}
}
