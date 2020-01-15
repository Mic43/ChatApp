using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatClientLogic
{
    static class SocketExtensions
    {
        public static Task ConnectTaskAsync(this Socket socket, EndPoint endpoint)
        {
            return new TaskFactory().FromAsync(socket.BeginConnect, socket.EndConnect, endpoint, null);
        }
        // public static Task SendTaskAsync(this Socket socket, EndPoint endpoint, byte[] buffer, int offset,
        //     int count)
        // {
        //     return new TaskFactory<int>().FromAsync(socket.BeginSend, socket.EndSend,buffer,
        // }
    }
}