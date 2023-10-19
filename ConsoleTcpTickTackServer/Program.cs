using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ConsoleTcpTickTackServer
{

    public class Server
    {
        private IPEndPoint ipEndPoint = new(IPAddress.Any, 13);

        public Server() { }

        public async Task Listen()
        {
            TcpListener listener = new(ipEndPoint);

            try
            {
                listener.Start();
                Random rnd = new Random();
                while (true)
                {
                    using TcpClient handler = await listener.AcceptTcpClientAsync();
                    await using NetworkStream stream = handler.GetStream();


                    var message = rnd.Next(1, 10).ToString();
                    var dateTimeBytes = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(dateTimeBytes);

                    Console.WriteLine($"Sent message: '{message}'");
                }  
            }
            finally
            {
                listener.Stop();
            }
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
