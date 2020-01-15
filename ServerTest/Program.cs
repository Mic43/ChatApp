﻿using System;
using System.Net;
using System.Net.Sockets;
using Common;
using Common.Interfaces;
using Common.Messages;

namespace ServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            var serverSocket = new TcpListener(ip, 14001);
            serverSocket.Start();
            

            using (TcpClient clientSocket = serverSocket.AcceptTcpClient())
            {
                Console.WriteLine($"client connected: {clientSocket.Client.RemoteEndPoint}");
                Handle(clientSocket);

                clientSocket.Close();
            }
            serverSocket.Stop();
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