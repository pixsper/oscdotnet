using System;
using System.IO;
using System.Text;

namespace Pixsper.OscDotNet.Serialization
{
    internal class OscBinaryReader : BinaryReader
    {
        public OscBinaryReader(Stream input)
            : base(input, Encoding.ASCII)
        {
        }

        public void Pad()
        {
            int pad = 3 - ((int) BaseStream.Position - 1) % 4;

            for (int i = 0; i < pad; ++i)
                ReadByte();
        }

        public override int ReadInt32()
        {
            var data = base.ReadBytes(sizeof(int));

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data, 0, data.Length);

            return BitConverter.ToInt32(data, 0);
        }

        public override long ReadInt64()
        {
            var data = base.ReadBytes(sizeof(long));

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data, 0, data.Length);

            return BitConverter.ToInt64(data, 0);
        }

        public override float ReadSingle()
        {
            var data = base.ReadBytes(sizeof(float));

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data, 0, data.Length);

            return BitConverter.ToSingle(data, 0);
        }

        public override double ReadDouble()
        {
            var data = base.ReadBytes(sizeof(double));

            if (BitConverter.IsLittleEndian)
                Array.Reverse(data, 0, data.Length);

            return BitConverter.ToDouble(data, 0);
        }

        public override string ReadString()
        {
            var result = new StringBuilder(32);

            for (int i = 0; i < BaseStream.Length; ++i)
            {
                char c = ReadChar();

                if (c == '\0')
                    break;

                result.Append(c);
            }

            string value = result.ToString();

            Pad();

            return value;
        }

        public override byte[] ReadBytes(int count)
        {
            var bytes = base.ReadBytes(count);

            Pad();

            return bytes;
        }
    }
}