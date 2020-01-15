using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatClientLogic
{
    // public class Test
    // {
    //     private Socket socket;
    //
    //     public Test()
    //     {
    //       
    //     }
    //
    //
    //     public async Task<LoginResult> TryLoginAsync(string userName,string password)
    //     {
    //         nameof 
    //     }
    //     public async Task<bool> TryConnectAsync()
    //     {
    //         const int port = 11111;
    //
    //         IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
    //         IPAddress ipAddr = ipHost.AddressList[0];
    //         IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);
    //
    //         try
    //         {
    //             socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
    //             await socket.ConnectTaskAsync(localEndPoint);
    //         }
    //         catch (SocketException e)
    //         {
    //             //Console.WriteLine(e);
    //             return await Task.FromResult(false);
    //         }
    //
    //         return await Task.FromResult(true);
    //     }
    // }

    public class LoginResult
    {

    }
}