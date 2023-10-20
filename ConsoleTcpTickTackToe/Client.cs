using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleTcpTickTackToe
{
    public class Client
    {
        private static IPEndPoint ipEndPoint = new(IPAddress.Parse("127.0.0.1"), 13);

        public static async Task<int> RequestNextMove(string msg)
        {
            using Socket client = new(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);
            await client.ConnectAsync(ipEndPoint);

            msg += "<|EOM|>";

            string response;
            while (true)
            {
                var messageBytes = Encoding.UTF8.GetBytes(msg);
                using (var memoryStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                    {
                        await gzipStream.WriteAsync(messageBytes, 0, messageBytes.Length);
                    }
                    var compressedData = memoryStream.ToArray();
                    _ = await client.SendAsync(compressedData, SocketFlags.None);
                }
                Debug.WriteLine($"Socket client sent message: \"{msg}\"");

                // Receive ack.
                var buffer = new byte[1_024];
                var received = await client.ReceiveAsync(buffer, SocketFlags.None);
                response = Encoding.UTF8.GetString(buffer, 0, received);
                string ack = "<|ACK|>";
                if (response.EndsWith(ack))
                {
                    Debug.WriteLine($"Socket client received acknowledgment: \"{response}\"");
                    response = response.Replace(ack, "");
                    break;
                }
            }

            client.Shutdown(SocketShutdown.Both);
            Debug.WriteLine($"Message received: '{response}'");
            return int.Parse(response);
        }
    }
}
