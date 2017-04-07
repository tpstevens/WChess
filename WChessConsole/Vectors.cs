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
	}

	struct Vector2UI
	{
		public uint x, y;

		public Vector2UI(uint x, uint y)
		{
			this.x = x;
			this.y = y;
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
