using System.Diagnostics;
using System.Text.Json;

namespace ConsoleTcpTickTackToe
{

    public class TicTacAI
    {
        private static int Minimax(bool isMaxTurn, Player maximizerMark, Board board)
        {
            Result state = board.GetState();
            if (state == Result.Draw)
            {
                return 0;
            }
            else if (state == Result.Winner)
            {
                return (board.GetWinner() == maximizerMark) ? 1 : -1;
            }

            List<int> scores = new List<int>();
            foreach (int move in board.PossibleMoves)
            {
                if (!board.MakeMove(move)) continue;
                int score = Minimax(!isMaxTurn, maximizerMark, board);
                board.Undo();
                scores.Add(score);
            }

            return isMaxTurn ? scores.Max() : scores.Min();
        }

        public static int MakeBestMove(Board ticTacBoard, Player aiPlayer)
        {
            int[,] winingMoves = ticTacBoard.PossibleMoves;
            for (int i = 0; i < winingMoves.GetLength(0); i++)
            {
                int a = winingMoves[i, 0] - 1; //1
                int b = winingMoves[i, 1] - 1; //5
                int c = winingMoves[i, 2] - 1; //9

                if (ticTacBoard.Squares[a].Mark == 'X' && ticTacBoard.Squares[b].Mark == 'X' && ticTacBoard.Squares[c].Mark != 'O')
                {
                    return c + 1;
                }
                else if (ticTacBoard.Squares[b].Mark == 'X' && ticTacBoard.Squares[c].Mark == 'X' && ticTacBoard.Squares[a].Mark != 'O')
                {
                    return a + 1;
                }
                else if (ticTacBoard.Squares[a].Mark == 'X' && ticTacBoard.Squares[c].Mark == 'X' && ticTacBoard.Squares[b].Mark != 'O')
                {
                    return b + 1;
                }
            }

            int bestScore = int.MinValue;
            int bestMove = -1;
            foreach (int move in ticTacBoard.PossibleMoves)
            {
                if (!ticTacBoard.MakeMove(move)) continue;
                int score = Minimax(false, aiPlayer, ticTacBoard);
                ticTacBoard.Undo();
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }
            return bestMove;
        }
    }

    internal class Program
    {
        static int size = 3;
        static Board board = new(size);
        static async Task Main(string[] args)
        {
            /*board.MakeMove(2);
            int move = TicTacAI.MakeBestMove(board, Player.Server);
            board.MakeMove(move);
            board.MakeMove(2);
            board.MakeMove(7);
            board.MakeMove(3);
            board.Undo();
            board.MakeMove(3);*/
            int p = 1;
            Result result = Result.None;
            while (result == Result.None)
            {
                Console.Clear();
                board.Print();
                p++;
                if (p % 2 == 0)
                {
                    bool isValid;
                    do
                    {
                        Console.Write("Your move: ");
                        isValid = int.TryParse(Console.ReadLine(), out int index) &&
                            index > 0 && index <= size * size && board.SetPlayer(index, Player.You);

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
                    do
                    {
                        int move = TicTacAI.MakeBestMove(board, Player.Server);
                        isValid = board.SetPlayer(move, Player.Server);
                    } while (!isValid);
                }

                result = board.GetState();
            }


            var s = board.GetState();

            if (s == Result.Winner) 
            {
                if (board.GetWinner() == Player.Server)
                {
                    Console.WriteLine($"Server won");
                }
                else
                {
                    Console.WriteLine($"You won");
                }
            }
            else if (s == Result.Draw)
            {
                Console.WriteLine($"Draw");
            }

            board.Print();

            


            return;


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

        private static bool Confirm(string title)
        {
            ConsoleKey response;
            do
            {
                Console.Write($"{title} [y/n] ");
                response = Console.ReadKey(false).Key;
            } while (response != ConsoleKey.Y && response != ConsoleKey.N);

            return response == ConsoleKey.Y;
        }

        private static async Task Play()
        {
            int p = 1;
            Result result = Result.None;
            while (result == Result.None)
            {
                Console.Clear();
                board.Print();
                p++;
                if (p % 2 == 0)
                {
                    bool isValid;
                    do
                    {
                        Console.Write("Your move: ");
                        isValid = int.TryParse(Console.ReadLine(), out int index) &&
                            index > 0 && index <= size * size && board.SetPlayer(index, Player.You);

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
                    string message = JsonSerializer.Serialize(board.ToList());
                    do
                    {
                        isValid = board.SetPlayer(await Client.RequestNextMove(message), Player.Server);
                    } while (!isValid);
                }

                result = board.GetState();
            }

            Console.Clear();
            board.Print();
            ShowResult((p % 2 == 0) ? "You" : "Server", result);
        }

        private static void ShowResult(string playerName, Result result)
        {
            if (result == Result.Winner)
            {
                Console.WriteLine($"{playerName} won");
            }
            else if (result == Result.Draw)
            {
                Console.WriteLine("Draw");
            }
        }
    }
}
