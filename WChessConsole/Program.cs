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
			Pair<Vector2UI> positionPair;
			string input;
			string[] splitInput;

			positionPair = new Pair<Vector2UI>();

			do
			{
				Console.Write("Enter a move (in algebraic notation) or 'q' to quit: ");
				input = Console.ReadLine().Trim().ToLower();

				if (input == "q")
				{
					continueGame = false;
					validInput = true;
				}
				else
				{
					splitInput = input.Split(' ');

					if (splitInput.Length == 1 && splitInput[0].Length > 0)
					{
						validInput = game.MakeMove(splitInput[0]);
					}
					else if (splitInput.Length == 2)
					{
						if (parsePosition(splitInput[0], out positionPair.first)
							&& parsePosition(splitInput[1], out positionPair.second))
						{
							validInput = game.MakeMove(positionPair.first, positionPair.second);
						}
					}
				}

				if (!validInput)
					Console.WriteLine("  Invalid move - can't tell you if it's illegal or bad format, TODO");
			} while (!validInput && continueGame);

			return continueGame;
		}

		private static bool parsePosition(string positionString, out Vector2UI positionVector)
		{
			string[] ints = positionString.Trim().Split(',');
			if (ints.Length == 2)
			{
				positionVector = new Vector2UI(uint.Parse(ints[0]) - 1, uint.Parse(ints[1]) - 1);
				return true;
			}
			else
			{
				positionVector = new Vector2UI(uint.MaxValue, uint.MaxValue);
				return false;
			}
		}
	}
}