using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace SteamVRServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string message = "012012010102012010201021020120102012010210201021";
            int port = 32222;
            int remotePort = 23333;
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ip = new IPEndPoint(ipAddr, port);
            IPEndPoint remoteIP = new IPEndPoint(ipAddr, remotePort);
            UdpClient server = new UdpClient(ip);

            for(int i=0;i<message.Length;++i)
            {
                int c = message[i] - '0';
                c--;
                byte[] sendBytes = Encoding.Unicode.GetBytes(c.ToString());
                server.Send(sendBytes, sendBytes.Length, remoteIP);
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
