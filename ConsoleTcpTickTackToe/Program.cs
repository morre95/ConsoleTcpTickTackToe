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
        static Board board;
        static async Task Main(string[] args)
        {
            board = new Board(size);
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
                            index > 0 && index < 10 &&
                            board.SetPlayer(index - 1, Player.You);

                        if (!isValid)
                        {
                            Console.WriteLine("Please enter number between 1 - 9");
                        }
                    }
                    while (!isValid);


                }
                else
                {
                    bool isOk;
                    string msg = JsonSerializer.Serialize(board.ToList());
                    do
                    {
                        isOk = board.SetPlayer(await Client.RequestNextMove(msg) - 1, Player.Server);
                    } while (!isOk);
                }

                result = board.CheckWin();
            }

            Console.Clear();
            board.Print();

            if (result == Result.Winner)
            {
                string playerName = (p % 2 == 0) ? "You" : "Server";
                Console.WriteLine($"{playerName} won");
            }
            else if (result == Result.Draw)
            {
                Console.WriteLine("Draw");
            }
        }
    }

    /*internal class Program
    {
        static List<char> list = new List<char>();
        static int player = 1;
        static int choice;
        static Result flag = Result.None;

        static async Task Main(string[] args)
        {
            char[] arr = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            list = new(arr);

            await Play();

            while (true)
            {
                if (Confirm("Play again?"))
                {
                    list = new(arr);
                    await Play();
                }
                else
                {
                    break;
                }
            } 
        }

        private static async Task Play()
        {
            do
            {
                
                Board();
                if (player % 2 == 0)
                {
                    bool isOk = false;
                    string msg = JsonSerializer.Serialize(list);
                    do
                    {
                        choice = await Client.RequestNextMove(msg);

                        isOk = list[choice] != 'X' && list[choice] != 'O';
                    } while (!isOk);

                }
                else
                {
                    int number;
                    bool isValid;
                    if (player > 1)
                    {
                        Console.WriteLine($"Your opponent's previous move: {choice}");
                    }
                    else
                    {
                        Console.WriteLine("Enter number between 1 and 9: ");
                    }

                    do
                    {
                        Console.Write("Your move: ");

                        isValid = int.TryParse(Console.ReadLine(), out number) && number > 0 && number < 10;

                        if (isValid)
                        {
                            Console.WriteLine("Please enter the correct number");
                        }
                    }
                    while (!isValid);

                    choice = number;
                }

                if (list[choice] != 'X' && list[choice] != 'O')
                {
                    if (player % 2 == 0) 
                    {
                        list[choice] = 'O';
                        player++;
                    }
                    else
                    {
                        list[choice] = 'X';
                        player++;
                    }
                }
                else
                {
                    Console.WriteLine($"Sorry the row {choice} is already marked with {list[choice]}");
                    Console.WriteLine("\n");
                    Console.WriteLine("Please press any key to try again.....");
                    Console.ReadKey();
                }
                flag = CheckWin();// calling of check win
            }
            while (flag != Result.Winner && flag != Result.Draw);
            Console.Clear();
            Board();
            if (flag == Result.Winner)
            {
                string playerName = (player % 2 == 0) ? "You" : "Server";
                Console.WriteLine($"{playerName} won");
            }
            else
            {
                Console.WriteLine("Draw");
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

        private static void Board()
        {
            Console.Clear();
            string print = "You:X and Server:O";
            print += "\n";
            print += "     |     |      ";
            print += "\n";
            print += $"  {list[1]}  |  {list[2]}  |  {list[3]}";
            print += "\n";
            print += "-----|-----|----- ";
            print += "\n";
            print += $"  {list[4]}  |  {list[5]}  |  {list[6]}";
            print += "\n";
            print += "-----|-----|----- ";
            print += "\n";
            print += $"  {list[7]}  |  {list[8]}  |  {list[9]}";
            print += "\n";
            print += "     |     |      ";

            Console.WriteLine(print);
        }

        private static Result CheckWin()
        {

            int[,] winConditions =
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 },
                { 1, 4, 7 },
                { 2, 5, 8 },
                { 3, 6, 9 },
                { 1, 5, 9 },
                { 3, 5, 7 }
            };

            for (int i = 0; i < winConditions.GetLength(0); i++)
            {
                int a = winConditions[i, 0];
                int b = winConditions[i, 1];
                int c = winConditions[i, 2];
                if (list[a] == list[b] && list[b] == list[c])
                {
                    return Result.Winner;
                }
            }

            if (list.All(cell => cell != '1' && cell != '2' && cell != '3' && cell != '4' && cell != '5' && cell != '6' && cell != '7' && cell != '8' && cell != '9'))
            {
                return Result.Draw; 
            }

            return Result.None;
        }
    }*/
}
