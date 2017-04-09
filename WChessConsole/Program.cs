using System;

namespace WChessConsole
{
	class Program
	{
		static Game game;

		static void Main(string[] args) 
		{
			bool continueGame;
			game = new Game(8, 8);

			do
			{
				Console.WriteLine(game.ToString());
				continueGame = makeMove();
				Console.WriteLine();
			} while (continueGame);

			Console.WriteLine("Thanks for playing!\n");
		}

		private static bool makeMove()
		{
			bool continueGame = true;
			bool validInput = false;
			Pair<Vector2I> positionPair;
			string input;
			string[] splitInput;

			positionPair = new Pair<Vector2I>();

			do
			{
				Console.Write("Enter a move (in algebraic notation), 'u' to undo, or 'q' to quit: ");
				input = Console.ReadLine().Trim().ToLower();

				if (input == "d")
				{
					// TODO: debug mode (must be at start? or can't disable debug mode after enabled until new game)
				}
				else if (input == "q")
				{
					continueGame = false;
					validInput = true;
				}
				else if (input == "u")
				{
					validInput = true;
					game.UndoPlayerMove();
				}
				else
				{
					splitInput = input.Split(' ');

					if (splitInput.Length == 1 && splitInput[0].Length > 0)
					{
						validInput = game.PlayerMove(splitInput[0]);
					}
					else if (splitInput.Length == 2)
					{
						if (parsePosition(splitInput[0], out positionPair.first)
							&& parsePosition(splitInput[1], out positionPair.second))
						{
							validInput = game.PlayerMove(positionPair.first, positionPair.second);
						}
					}
				}

				if (!validInput)
					Console.WriteLine("  Invalid move - can't tell you if it's illegal or bad format, TODO");
			} while (!validInput && continueGame);

			return continueGame;
		}

		private static bool parsePosition(string positionString, out Vector2I positionVector)
		{
            int x, y;
			string[] ints = positionString.Trim().Split(',');

			if (ints.Length == 2
                && int.TryParse(ints[0], out x)
                && int.TryParse(ints[1], out y))
			{
                positionVector = new Vector2I(int.Parse(ints[0]) - 1, int.Parse(ints[1]) - 1);
                return true;
			}
			else if (ints.Length == 1
                && ints[0].Length == 2
                && int.TryParse("" + ints[0][1], out y))
			{
                x = ints[0][0] - 97;
                positionVector = new Vector2I(x, y - 1);
                return true;
            }
            else
            {
			    positionVector = new Vector2I(int.MaxValue, int.MaxValue);
			    return false;
            }
		}
	}
}