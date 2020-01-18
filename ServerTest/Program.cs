using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Common;
using Common.Interfaces;
using Common.Messages;

namespace ServerTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Server started!");
            var chatServer = new ChatServer("127.0.0.1", 14002,new ConsoleLogger());

            Task task = Task.Run(() => chatServer.Start());
            Console.ReadKey();

            chatServer.Stop();

            task.Wait();

        }
    }
}
