using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Pixsper.OscDotNet
{
    public sealed class UdpOscListenService : IOscListenService
    {
        private readonly UdpClient _udpClient;
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _listenTask;

        public UdpOscListenService(IPEndPoint localEndPoint)
        {
            _udpClient = new UdpClient(localEndPoint)
            {
                ExclusiveAddressUse = false,
                EnableBroadcast = true
            };
        }

        public void Dispose()
        {
            if (IsListening)
                StopListening().Wait();

            _udpClient.Dispose();
        }


        public bool IsListening => _listenTask != null;

        public void StartListening()
        {
            if (IsListening)
                throw new InvalidOperationException("Cannot start listening, service is already listening");

            _cancellationTokenSource = new CancellationTokenSource();
            _listenTask = receiveMessagesAsync(_cancellationTokenSource.Token);
        }

        public async Task StopListening()
        {
            if (!IsListening)
                throw new InvalidOperationException("Cannot stop listening, service is not currently listening");

            Debug.Assert(_cancellationTokenSource != null);

            _cancellationTokenSource.Cancel();

            Debug.Assert(_listenTask != null);

            await _listenTask.ConfigureAwait(false);

            _listenTask = null;
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        public event EventHandler<OscPacket>? OscPacketReceived;

        private async Task receiveMessagesAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                bool didReceive = false;
                UdpReceiveResult message;

                try
                {
                    message = await _udpClient.ReceiveAsync().ConfigureAwait(false);
                    didReceive = true;
                }
                catch
                {
                    // Exception may be thrown by cancellation
                    if (!cancellationToken.IsCancellationRequested)
                        throw;
                }

                // If nothing received, must have been cancelled
                if (!didReceive)
                    return;
            }
        }
    }
}