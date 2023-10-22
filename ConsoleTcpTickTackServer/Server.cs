using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Text.Json;
using System.IO.Compression;
using ConsoleTcpTickTackToe;
using System.Text.RegularExpressions;

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
                        response = DeCompress(buffer, received);

                        var eom = "<|EOM|>";
                        if (response.IndexOf(eom) > -1 /* is end of message */)
                        {
                            response = response.Replace(eom, "");

                            string ackMessage = MessageFactory(response);

                            await Send(ackMessage, handler);

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

        private string MessageFactory(string response)
        {
            string ackMessage = string.Empty;

            string charArrayPattern = @"^\[\s*(?:""([^""]*)"",?\s*)+\]$";

            string boardPattern = @"^{""(JsonSquares|Squares)"":.*}$";

            // Normal
            if (Regex.IsMatch(response, charArrayPattern))
            {
                List<char> list = JsonSerializer.Deserialize<List<char>>(response)!;
                Strategy strategy = new(list);
                Debug.WriteLine("Sever Normal");
                return $"{strategy.MakeBestMove()}<|ACK|>";
            } 
            // Hard
            else if (Regex.IsMatch(response, boardPattern))
            {
                Board board = JsonSerializer.Deserialize<Board>(response)!;
                Debug.WriteLine("Sever Hard");
                return $"{TicTacAI.MakeBestMove(board, Player.Server)}<|ACK|>";
            } 
            // Easy
            else if (response == "")
            {
                Random rnd = new Random();
                Debug.WriteLine("Sever Easy");
                return $"{rnd.Next(1, 10)}<|ACK|>";
            }

            return ackMessage;
        }

        private static async Task Send(string ackMessage, Socket handler)
        {
            var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
            await handler.SendAsync(echoBytes, 0);
            Console.WriteLine($"Socket server sent acknowledgment: \"{ackMessage}\"");
        }

        private static string DeCompress(byte[] buffer, int received)
        {
            string response;
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

            return response;
        }
    }
}
