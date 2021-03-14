using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Pixsper.OscDotNet.Helpers;

namespace Pixsper.OscDotNet
{
    public sealed class TcpOscListenService : IOscListenService
    {
        private const int SlipProtocolEnd = 0xC0;
        private const int SlipProtocolEsc = 0xDB;
        private const int SlipProtocolEscEnd = 0xDC;
        private const int SlipProtocolEscEsc = 0xDD;

        private static int BufferLength = ushort.MaxValue;

        public enum PacketMode
        {
            PacketLengthHeaders,
            Slip
        }

        private readonly TcpListener _tcpListener;
        private CancellationTokenSource _cancellationTokenSource;
        private Task? _listenTask;


        public TcpOscListenService(IPEndPoint localEndPoint, PacketMode mode)
        {
            _tcpListener = new TcpListener(localEndPoint);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            if (IsListening)
                StopListening().Wait();
        }

        public event EventHandler<OscPacket>? OscPacketReceived;

        public PacketMode Mode { get; }


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

            _cancellationTokenSource.Cancel();

            Debug.Assert(_listenTask != null);

            await _listenTask.ConfigureAwait(false);

            _listenTask = null;
            _cancellationTokenSource.Dispose();
        }

        private async Task receiveMessagesAsync(CancellationToken cancellationToken)
        {
            _tcpListener.Start();

            var data = new byte[BufferLength];

            try
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    var client = await _tcpListener.AcceptTcpClientAsync(cancellationToken).ConfigureAwait(false);
                    var stream = client.GetStream();

                    while (true)
                    {
                        int bytesReceived = await stream.ReadAsync(data, 0, data.Length, cancellationToken)
                            .ConfigureAwait(false);

                        if (bytesReceived == 0)
                            break;

                        switch (Mode)
                        {
                            case PacketMode.PacketLengthHeaders:

                                break;

                            case PacketMode.Slip:

                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch(OperationCanceledException){ }
            finally
            {
                _tcpListener.Stop();
            }
        }
    }   
}