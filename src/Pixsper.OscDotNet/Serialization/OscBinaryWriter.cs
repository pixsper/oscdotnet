using System;
using System.IO;
using System.Text;

namespace Pixsper.OscDotNet.Serialization
{
    internal class OscBinaryWriter : BinaryWriter
    {
        public OscBinaryWriter(Stream output)
            : base(output, Encoding.ASCII)
        {
        }

        public void Pad()
        {
            int pad = 3 - ((int) BaseStream.Position - 1) % 4;

            for (int i = 0; i < pad; ++i)
                base.Write(byte.MinValue);
        }

        public override void Write(int value)
        {
            var data = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data, 0, data.Length);

            base.Write(data);
        }

        public override void Write(long value)
        {
            var data = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data, 0, data.Length);

            base.Write(data);
        }

        public override void Write(float value)
        {
            var data = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data, 0, data.Length);

            base.Write(data);
        }

        public override void Write(double value)
        {
            var data = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data, 0, data.Length);

            base.Write(data);
        }

        public override void Write(string? value)
        {
            string terminatedValue = (value ?? string.Empty) + "\0";

            base.Write(Encoding.ASCII.GetBytes(terminatedValue));

            Pad();
        }

        public override void Write(byte[] buffer)
        {
            base.Write(buffer);
            Pad();
        }
    }
}