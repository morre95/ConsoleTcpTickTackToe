using System.Text.Json;

namespace ConsoleTcpTickTackToe
{

    internal class Program
    {
        static int size = 3;
        
        static Board board = new(size);

        static Difficulty difficulty = Difficulty.Easy;

        static async Task Main(string[] args)
        {
            Console.Title = "Client";

            SelectDifficulty();

            await Play();
            while (true)
            {
                if (Confirm("Play again?"))
                {
                    board = new Board(size);
                    await Play();
                }
                else
                {
                    break;
                }
            }

        }

        private static void SelectDifficulty()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"{(Difficulty)i} = {i + 1}");
            }

            bool isValid;
            int index;
            do
            {
                Console.Write("Select Difficulty: ");
                isValid = int.TryParse(Console.ReadLine(), out index) && index > 0 && index <= 3;

                if (!isValid)
                {
                    Console.WriteLine($"Please enter number between 1 - 3");
                }
            }
            while (!isValid);

            difficulty = (Difficulty)index - 1;
        }

        private static bool Confirm(string title)
        {
            ConsoleKey response;
            do
            {
                Console.Write($"{title} [y/n] ");
                response = Console.ReadKey(false).Key;

                if(response != ConsoleKey.Y && response != ConsoleKey.N)
                {
                    Console.WriteLine("\nPlease enter Y for yes or N for no");
                }
            } while (response != ConsoleKey.Y && response != ConsoleKey.N);

            return response == ConsoleKey.Y;
        }

        private static async Task Play()
        {
            State state = State.None;
            while (state == State.None)
            {
                PrintBoard();

                if (board.WhosTurn() == Player.You)
                {
                    bool isValid;
                    do
                    {
                        Console.Write("Your move: ");
                        isValid = int.TryParse(Console.ReadLine(), out int index) &&
                            index > 0 && index <= size * size && board.MakeMove(index);

                        if (!isValid)
                        {
                            Console.WriteLine($"Please enter number between 1 - {size * size}");
                        }
                    }
                    while (!isValid);
                }
                else
                {
                    bool isValid;
                    string message = ResponseFactory();

                    do
                    {
                        isValid = board.MakeMove(await Client.RequestNextMove(message));
                    } while (!isValid);
                }

                state = board.GetState();
            }

            PrintBoard();

            ShowResult(board.GetWinner().ToString(), state);
        }

        private static void PrintBoard()
        {
            Console.Clear();
            Console.WriteLine($"{difficulty}\nYou:X and Server:O");
            board.Print();
        }

        private static string ResponseFactory()
        {
            string message;
            if (difficulty == Difficulty.Normal)
            {
                message = JsonSerializer.Serialize(board.ToList());
            }
            else if (difficulty == Difficulty.Hard)
            {
                message = JsonSerializer.Serialize(board);
            }
            else
            {
                message = string.Empty;
            }

            return message;
        }

        private static void ShowResult(string playerName, State state)
        {
            if (state == State.Winner)
            {
                Console.WriteLine($"{playerName} won");
            }
            else if (state == State.Draw)
            {
                Console.WriteLine("Draw");
            }
        }
    }
}
