using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.IO.Compression;

namespace ConsoleTcpTickTackServer
{

    public class Server
    {
        private IPEndPoint ipEndPoint = new(IPAddress.Any, 13);

        public Server() { }

        public async Task Listen()
        {
            using Socket listener = new(
                    ipEndPoint.AddressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp);

            listener.Bind(ipEndPoint);
            listener.Listen(100);

            try
            {
               
                Strategy strategy;
                while (true)
                {
                    var handler = await listener.AcceptAsync();
                    string response;
                    while (true)
                    {
                        // Receive message.
                        var buffer = new byte[1_024];
                        var received = await handler.ReceiveAsync(buffer, SocketFlags.None);

                        using (var memoryStream = new MemoryStream(buffer, 0, received))
                        {
                            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                            {
                                using (var reader = new StreamReader(gzipStream, Encoding.UTF8))
                                {
                                    response = reader.ReadToEnd();
                                }
                            }
                        }

                        var eom = "<|EOM|>";
                        if (response.IndexOf(eom) > -1 /* is end of message */)
                        {
                            response = response.Replace(eom, "");

                            List<char> list = JsonSerializer.Deserialize<List<char>>(response)!;
                            strategy = new(list);

                            Debug.WriteLine($"Socket server received message: \"{response}\"");

                            var ackMessage = $"{strategy.OWin()}<|ACK|>";
                            var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                            await handler.SendAsync(echoBytes, 0);
                            Console.WriteLine($"Socket server sent acknowledgment: \"{ackMessage}\"");

                            break;
                        }
                    }
                }  
            }
            finally
            {
                listener.Dispose();
            }
        }
    }

    public class Strategy
    {
        private List<char> list = new();

        public List<char> Board => list;

        public Strategy(List<char> tableList) 
        { 
            list = tableList;
        }

        public int OWin()
        {
            /*
             * 1,2,3
             * 4,5,6
             * 7,8,9
             */
            int[][] winPatterns = new int[][]
            {
                new int[] {1, 2, 3},
                new int[] {4, 5, 6},
                new int[] {7, 8, 9},
                new int[] {1, 4, 7},
                new int[] {2, 5, 8},
                new int[] {3, 6, 9},
                new int[] {1, 5, 9},
                new int[] {3, 5, 7}
            };

            for (int i = 0; i < winPatterns.Length; i++)
            {
                int[] pattern = winPatterns[i];
                int a = pattern[0]; //1
                int b = pattern[1]; //5
                int c = pattern[2]; //9

                int? winMove = IsWining(a, b, c, 'O');
                if (winMove != null)
                {
                    return (int)winMove;
                }

                winMove = IsWining(a, b, c, 'X');

                if (winMove != null)
                {
                    return (int)winMove;
                }
            }


            if (list.FindAll(x => x == 'X').Count == 1)
            {
                if (list[1] == 'X' && list[5] != 'O')
                {
                    return 5;
                }
            }


            if (list[1] == 'X' && list[5] == 'O')
            {
                if (list[9] == 'X')
                {
                    if (list[2] != 'O') return 2;
                    if (list[4] != 'O') return 4;
                    if (list[6] != 'O') return 6;
                    if (list[8] != 'O') return 8;
                }
                else if (list[6] == 'X')
                {
                    if (list[2] != 'O') return 2;
                    if (list[3] != 'O') return 3;
                    if (list[8] != 'O') return 8;
                    if (list[9] != 'O') return 9;
                }
            }


            Random rnd = new();

            return rnd.Next(1, 10);
        }

        public int? IsWining(int a, int b, int c, char player = 'O')
        {
            char op = player == 'X' ? 'O' : 'X';

            if (list[a] == player && list[b] == player && list[c] != op)
            {
                return c;
            }
            else if (list[b] == player && list[c] == player && list[a] != op)
            {
                return a;
            }
            else if (list[a] == player && list[c] == player && list[b] != op)
            {
                return b;
            }
            return null;
        }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            Server server = new Server();
            await server.Listen();
        }
    }
}
