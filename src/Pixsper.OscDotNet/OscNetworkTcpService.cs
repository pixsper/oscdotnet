using System;
using System.Threading.Tasks;

namespace Pixsper.OscDotNet
{
    public class OscListenTcpService : IOscListenService
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool IsListening { get; }
        public void StartListening()
        {
            throw new NotImplementedException();
        }

        public Task StopListening()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<OscPacket>? OscPacketReceived;
    }
}
