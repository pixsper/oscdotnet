using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Pixsper.OscDotNet
{
    public class OscBundle : IOscPacketPart, IEquatable<OscBundle>, IComparable<OscBundle>, IComparable
    {
        public OscBundle(OscTimeTag timeTag, ImmutableList<IOscPacketPart> parts)
        {
            TimeTag = timeTag;
            Parts = parts;
        }

        public OscBundle(OscTimeTag timeTag, IEnumerable<IOscPacketPart> parts)
            : this(timeTag, parts.ToImmutableList())
        {
        }

        public OscBundle(OscTimeTag timeTag, params IOscPacketPart[] parts)
            : this(timeTag, parts.ToImmutableList())
        {
        }

        public OscBundle(ImmutableList<IOscPacketPart> parts)
        {
            TimeTag = OscTimeTag.Now;
            Parts = parts;
        }

        public OscBundle(IEnumerable<IOscPacketPart> parts)
            : this(parts.ToImmutableList())
        {
            TimeTag = OscTimeTag.Now;
        }

        public OscBundle(params IOscPacketPart[] parts)
            : this(parts.ToImmutableList())
        {
        }


        public OscTimeTag TimeTag { get; }
        public ImmutableList<IOscPacketPart> Parts { get; }

        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            return obj is OscBundle other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(OscBundle)}");
        }


        public int CompareTo(OscBundle? other)
        {
            return other is null ? 1 : TimeTag.Time.CompareTo(other.TimeTag.Time);
        }


        public bool Equals(OscBundle? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TimeTag.Equals(other.TimeTag) && Parts.Equals(other.Parts);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OscBundle) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (TimeTag.GetHashCode() * 397) ^ Parts.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"#bundle {string.Join(",", Parts.ToString())}";
        }


        public static bool operator <(OscBundle left, OscBundle right)
        {
            return Comparer<OscBundle>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(OscBundle left, OscBundle right)
        {
            return Comparer<OscBundle>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(OscBundle left, OscBundle right)
        {
            return Comparer<OscBundle>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(OscBundle left, OscBundle right)
        {
            return Comparer<OscBundle>.Default.Compare(left, right) >= 0;
        }
    }
}