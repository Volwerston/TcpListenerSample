using System;
using System.IO;
using System.Net.Sockets;

namespace MyTcpListener
{
    class Program
    {
        static async void ListenToPort(TcpListener listener)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            StreamReader sr = new StreamReader(client.GetStream());
            StreamWriter sw = new StreamWriter(client.GetStream());

            try
            {
                // read request
                string request = sr.ReadLine();
                Console.WriteLine(request);

                sw.WriteLine("HTTP/1.1 200 OK\n");
                sw.WriteLine("<h1>Accepted</h1>");
                sw.Flush();
            }
            catch
            {
                sw.WriteLine("HTTP/1.1 404 Not Found");
                sw.WriteLine("<h1>Internal Server Error</h1>");
                sw.Flush();
            }

            client.Close();
        }

        static void Main(string[] args)
        {
            TcpListener listener1 = new TcpListener(1302); // for user
            listener1.Start();
            TcpListener listener2 = new TcpListener(1303); // for admin
            listener2.Start();

            while (true)
            {
                ListenToPort(listener1);
                ListenToPort(listener2);
            }
        }
    }
}
