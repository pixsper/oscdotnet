using System;
using System.IO;
using Pixsper.OscDotNet.Serialization;

namespace Pixsper.OscDotNet
{
    public class OscPacket
    {
        public static OscPacket? FromByteArray(byte[] data)
        {
            OscPacket? packet = null;

            try
            {
                var ms = new MemoryStream(data);
                var reader = new OscBinaryReader(ms);

                var idChar = reader.ReadChar();

                switch (idChar)
                {
                    case '/':
                        break;

                    case '#':
                        break;

                    default:
                        throw new FormatException("Unknown packet identifier");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (IOException)
            {
            }
            finally
            {
            }

            return packet;
        }
    }

    public interface IOscPacketPart
    {
    }
}