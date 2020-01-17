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
            var chatServer = new ChatServer("127.0.0.1", 14002);

            Task task = Task.Run(() => chatServer.Start());
            Console.ReadKey();

            chatServer.Stop();
            task.Wait();

//            IPAddress ip = IPAddress.Parse("127.0.0.1");
//            var serverSocket = new TcpListener(ip, 14001);
//            serverSocket.Start();
//            
//
//            using (TcpClient clientSocket = serverSocket.AcceptTcpClient())
//            {
//                Console.WriteLine($"client connected: {clientSocket.Client.RemoteEndPoint}");
//                Handle(clientSocket);
//
//                clientSocket.Close();
//            }
//            serverSocket.Stop();
        }

        private static void Handle(TcpClient clientSocket)
        {
            var binaryMessageProcessor = new BinaryMessageProcessor();
            var tcpMessageReceiver = new TcpMessageReceiver(clientSocket,binaryMessageProcessor);

            var loginSender = new TcpMessageSender(clientSocket, binaryMessageProcessor);
            var loginReceiver = new MessageReceiver<LoginRequest>(tcpMessageReceiver);

            LoginRequest msg = loginReceiver.Receive();
            Console.WriteLine(msg);

            loginSender.Send(new LoginResponse(true,Guid.NewGuid().ToString()));

            Console.ReadKey();
        }
    }
}
