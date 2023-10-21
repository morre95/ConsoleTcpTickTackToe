﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleTcpTickTackToe
{
    internal class Program
    {
        static int size = 3;
        static Board board = new(size);
        static async Task Main(string[] args)
        {
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

                result = board.CheckWin();
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
