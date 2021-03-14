using System;
using System.Globalization;

namespace Pixsper.OscDotNet
{
    public readonly struct OscTimeTag : IEquatable<OscTimeTag>
    {
        public static int ByteLength = 8;

        public static readonly DateTime Epoch = new DateTime(1900, 1, 1, 0, 0, 0, 0);

        public static readonly OscTimeTag MinValue = new OscTimeTag(Epoch + TimeSpan.FromMilliseconds(1d));
        
        public static OscTimeTag Now => new OscTimeTag(DateTime.Now);

        public static OscTimeTag FromByteArray(byte[] data)
        {
            var secondsSinceEpochData = new byte[4];
            Buffer.BlockCopy(data, 0, secondsSinceEpochData, 0, 4);

            var fractionalSecondData = new byte[4];
            Buffer.BlockCopy(data, 4, fractionalSecondData, 0, 4);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(secondsSinceEpochData, 0, secondsSinceEpochData.Length);
                Array.Reverse(fractionalSecondData, 0, fractionalSecondData.Length);
            }

            uint secondsSinceEpoch = BitConverter.ToUInt32(secondsSinceEpochData, 0);
            uint fractionalSecond = BitConverter.ToUInt32(fractionalSecondData, 0);

            var timeStamp = Epoch.AddSeconds(secondsSinceEpoch).AddMilliseconds(fractionalSecond);

            if (IsValidTime(timeStamp) == false)
                throw new InvalidOperationException("Not a valid OSC TimeTag discovered.");

            return new OscTimeTag(timeStamp);
        }

        public OscTimeTag(DateTime timeStamp)
        {
            timeStamp = new DateTime(timeStamp.Ticks - timeStamp.Ticks % TimeSpan.TicksPerMillisecond,
                timeStamp.Kind);

            if (IsValidTime(timeStamp) == false)
                throw new ArgumentException("Not a valid OSC TimeTag.", nameof(timeStamp));

            Time = timeStamp;
        }

        public DateTime Time { get; }

        public uint SecondsSinceEpoch => (uint) (Time - Epoch).TotalSeconds;

        public uint FractionalSecond => (uint) (Time - Epoch).Milliseconds;


        public byte[] ToByteArray()
        {
            var timeStamp = new byte[8];

            var secondsSinceEpochData = BitConverter.GetBytes(SecondsSinceEpoch);
            var fractionalSecondData = BitConverter.GetBytes(FractionalSecond);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(secondsSinceEpochData, 0, secondsSinceEpochData.Length);
                Array.Reverse(fractionalSecondData, 0, fractionalSecondData.Length);
            }

            Array.Copy(secondsSinceEpochData, 0, timeStamp, 0, secondsSinceEpochData.Length);
            Array.Copy(fractionalSecondData, 0, timeStamp, 4, fractionalSecondData.Length);

            return timeStamp;
        }


        public override string ToString()
        {
            return Time.ToString(CultureInfo.InvariantCulture);
        }


        public static bool IsValidTime(DateTime timeStamp)
        {
            return timeStamp >= Epoch + TimeSpan.FromMilliseconds(1d);
        }


        public bool Equals(OscTimeTag other)
        {
            return Time.Equals(other.Time);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is OscTimeTag other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Time.GetHashCode();
        }


        public static bool operator ==(OscTimeTag left, OscTimeTag right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OscTimeTag left, OscTimeTag right)
        {
            return !left.Equals(right);
        }


        public static bool operator <(OscTimeTag lhs, OscTimeTag rhs)
        {
            return lhs.Time < rhs.Time;
        }

        public static bool operator <=(OscTimeTag lhs, OscTimeTag rhs)
        {
            return lhs.Time <= rhs.Time;
        }

        public static bool operator >(OscTimeTag lhs, OscTimeTag rhs)
        {
            return lhs.Time > rhs.Time;
        }

        public static bool operator >=(OscTimeTag lhs, OscTimeTag rhs)
        {
            return lhs.Time >= rhs.Time;
        }


        public static implicit operator DateTime(OscTimeTag p) => p.Time;
        public static implicit operator OscTimeTag(DateTime value) => new OscTimeTag(value);
    }
}