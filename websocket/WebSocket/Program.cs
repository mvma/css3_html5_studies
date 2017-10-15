using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WebSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new TcpListener(IPAddress.Parse("127.0.0.1"), 1300);
            server.Start();

            Console.WriteLine("Server has started on 127.0.0.1:80. {0} Waiting for a connection...");

            var client = server.AcceptTcpClient();

            Console.WriteLine("A client connected");

            var networkStream = client.GetStream();

            while (true)
            {
                while (!networkStream.DataAvailable);

                var bytes = new byte[client.Available];
                networkStream.Read(bytes, 0, bytes.Length);

                var data = Encoding.UTF8.GetString(bytes);

                if (new Regex("^GET").IsMatch(data))
                {
                    var headers = new string[]{"HTTP/1.1 101 Switching Protocols",
                                               "Connection: Upgrade",
                                               "Upgrade: websocket",
                                               "Sec-WebSocket-Accept:"};
                    var response =
                        Encoding.UTF8.GetBytes(headers[0] + Environment.NewLine +
                                headers[1] + Environment.NewLine +
                                headers[2] + Environment.NewLine +
                                headers[3] + Convert.ToBase64String(
                                    SHA1.Create().ComputeHash(
                                        Encoding.UTF8.GetBytes(
                                            new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() +
                                                                                "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                        )
                                    )
                                ) + Environment.NewLine
                                  + Environment.NewLine);

                    networkStream.Write(response, 0, response.Length);
                }
            }
        }
    }
}
