using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Pixsper.OscDotNet.Helpers
{
    internal static class TcpClientExtensions
    {
        public static async Task<TcpClient> AcceptTcpClientAsync(this TcpListener listener, CancellationToken token)
        {
            try
            {
                return await listener.AcceptTcpClientAsync();
            }
            catch (Exception ex) when (token.IsCancellationRequested)
            {
                throw new OperationCanceledException("Cancellation was requested while awaiting TCP client connection.",
                    ex);
            }
        }
    }
}