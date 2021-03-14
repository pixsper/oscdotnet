using System;
using System.Threading.Tasks;

namespace Pixsper.OscDotNet
{
    public interface IOscListenService : IDisposable
    {
        bool IsListening { get; }

        void StartListening();
        Task StopListening();

        event EventHandler<OscPacket>? OscPacketReceived;
    }
}