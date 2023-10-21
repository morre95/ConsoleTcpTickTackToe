
namespace ConsoleTcpTickTackServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Server";

            Server server = new Server();
            await server.Listen();
        }
    }
}
