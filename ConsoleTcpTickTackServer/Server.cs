using System.Net.Sockets;
using System.Net;
using System.Text;
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
                        response = DeCompress(buffer, received);

                        var eom = "<|EOM|>";
                        if (response.IndexOf(eom) > -1 /* is end of message */)
                        {
                            response = response.Replace(eom, "");

                            List<char> list = JsonSerializer.Deserialize<List<char>>(response)!;
                            strategy = new(list);

                            var ackMessage = $"{strategy.OWin()}<|ACK|>";
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
