using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Pixsper.OscDotNet.MessageTypes;

namespace Pixsper.OscDotNet
{
    public interface IOscParameter
    {
        OscTypeCode TypeCode { get; }
        object? ValueUntyped { get; }

        string TypeTag { get; }
    }

    
    public abstract class OscParameterTyped<T> : IOscParameter, IEquatable<OscParameterTyped<T>>, IEquatable<T>
    {
        protected OscParameterTyped(OscTypeCode typeCode, T value)
        {
            TypeCode = typeCode;
            Value = value;
        }

        public OscTypeCode TypeCode { get; }
        public T Value { get; }
        
        public object? ValueUntyped => Value;

        public string TypeTag => TypeCode.GetTypeCodeChar().ToString();

        public bool Equals(OscParameterTyped<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TypeCode == other.TypeCode && EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public bool Equals(T? value) => EqualityComparer<T>.Default.Equals(Value, value);

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((OscParameterTyped<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) TypeCode * 397) ^ (Value is not null ? EqualityComparer<T>.Default.GetHashCode(Value) : 1);
            }
        }

        public override string ToString() => Value?.ToString() ?? "[null]";

        public static bool operator ==(OscParameterTyped<T>? left, OscParameterTyped<T>? right) => Equals(left, right);

        public static bool operator ==(OscParameterTyped<T>? left, T right) => left is not null && Equals(left.Value, right);

        public static bool operator ==(OscParameterTyped<T>? left, object? right) => left is not null && Equals(left.Value, right);

        public static bool operator !=(OscParameterTyped<T>? left, OscParameterTyped<T>? right) => !Equals(left, right);

        public static bool operator !=(OscParameterTyped<T>? left, T right ) => !(left != null && Equals(left.Value, right));

        public static bool operator !=(OscParameterTyped<T>? left, object? right) => !(left is not null && Equals(left.Value, right));
    }
    
    public class OscParameterInt : OscParameterTyped<int>
    {
        public OscParameterInt(int value)
            : base(OscTypeCode.Int32, value)
        {
        }

        public static implicit operator int(OscParameterInt p) => p.Value;
        public static implicit operator OscParameterInt(int value) => new OscParameterInt(value);
    }
    
    public class OscParameterFloat : OscParameterTyped<float>
    {
        public OscParameterFloat(float value)
            : base(OscTypeCode.Float, value)
        {
        }

        public static implicit operator float(OscParameterFloat p) => p.Value;
        public static implicit operator OscParameterFloat(float value) => new OscParameterFloat(value);
    }
    
    public class OscParameterString : OscParameterTyped<string>
    {
        public OscParameterString(string value)
            : base(OscTypeCode.String, value)
        {
        }

        public static implicit operator string(OscParameterString p) => p.Value;
        public static implicit operator OscParameterString(string value) => new OscParameterString(value);
    }
    
    public class OscParameterBlob : OscParameterTyped<byte[]>
    {
        public OscParameterBlob(byte[] value)
            : base(OscTypeCode.Blob, value)
        {
        }

        public static implicit operator byte[](OscParameterBlob p) => p.Value;
        public static implicit operator OscParameterBlob(byte[] value) => new OscParameterBlob(value);
    }
    
    public class OscParameterInt64 : OscParameterTyped<long>
    {
        public OscParameterInt64(long value)
            : base(OscTypeCode.Int64, value)
        {
        }

        public static implicit operator long(OscParameterInt64 p) => p.Value;
        public static implicit operator OscParameterInt64(long value) => new OscParameterInt64(value);
    }
    
    public class OscParameterTimeTag : OscParameterTyped<OscTimeTag>
    {
        public OscParameterTimeTag(OscTimeTag value)
            : base(OscTypeCode.TimeTag, value)
        {
        }

        public static implicit operator OscTimeTag(OscParameterTimeTag p) => p.Value;
        public static implicit operator OscParameterTimeTag(OscTimeTag value) => new OscParameterTimeTag(value);
    }
    
    public class OscParameterDouble : OscParameterTyped<double>
    {
        public OscParameterDouble(double value)
            : base(OscTypeCode.Double, value)
        {
        }

        public static implicit operator double(OscParameterDouble p) => p.Value;
        public static implicit operator OscParameterDouble(double value) => new OscParameterDouble(value);
    }
    
    public class OscParameterSymbol : OscParameterTyped<string>
    {
        public OscParameterSymbol(string value)
            : base(OscTypeCode.SymbolString, value)
        {
        }

        public override string ToString() => $"Symbol: '{Value}'";

        public static implicit operator string(OscParameterSymbol p) => p.Value;
        public static implicit operator OscParameterSymbol(string value) => new OscParameterSymbol(value);
    }
    
    public class OscParameterChar : OscParameterTyped<char>
    {
        public OscParameterChar(char value)
            : base(OscTypeCode.Char, value)
        {
        }

        public static implicit operator char(OscParameterChar p) => p.Value;
        public static implicit operator OscParameterChar(char value) => new OscParameterChar(value);
    }
    
    public class OscParameterColor : OscParameterTyped<OscColor>
    {
        public OscParameterColor(OscColor value)
            : base(OscTypeCode.RgbaColor, value)
        {
        }

        public static implicit operator OscColor(OscParameterColor p) => p.Value;
        public static implicit operator OscParameterColor(OscColor value) => new OscParameterColor(value);
    }
    
    public class OscParameterMidiMessage : OscParameterTyped<OscMidiMessage>
    {
        public OscParameterMidiMessage(OscMidiMessage value)
            : base(OscTypeCode.MidiMessage, value)
        {
        }

        public static implicit operator OscMidiMessage(OscParameterMidiMessage p) => p.Value;

        public static implicit operator OscParameterMidiMessage(OscMidiMessage value) =>
            new OscParameterMidiMessage(value);
    }
    
    public class OscParameterTrue : IOscParameter, IEquatable<bool>
    {
        public OscTypeCode TypeCode => OscTypeCode.TrueValue;
        public object ValueUntyped => true;

        public string TypeTag => TypeCode.GetTypeCodeChar().ToString();

        public static implicit operator bool(OscParameterTrue p) => true;

        public bool Equals(bool value) => value;

        public override string ToString() => true.ToString();
    }
    
    public class OscParameterFalse : IOscParameter, IEquatable<bool>
    {
        public OscTypeCode TypeCode => OscTypeCode.FalseValue;
        public object ValueUntyped => false;

        public string TypeTag => TypeCode.GetTypeCodeChar().ToString();

        public static implicit operator bool(OscParameterFalse p) => false;

        public bool Equals(bool value) => !value;

        public override string ToString() => false.ToString();
    }
    
    public class OscParameterNil : IOscParameter
    {
        public OscTypeCode TypeCode => OscTypeCode.NilValue;
        public object? ValueUntyped => null;

        public string TypeTag => TypeCode.GetTypeCodeChar().ToString();

        public override string ToString() => "[nil]";
    }
    
    public class OscParameterImpulse : IOscParameter
    {
        public OscTypeCode TypeCode => OscTypeCode.ImpulseValue;
        public object ValueUntyped => "[impulse]";

        public string TypeTag => TypeCode.GetTypeCodeChar().ToString();

        public override string ToString() => "Impulse";
    }
    
    public class OscParameterArray : IOscParameter
    {
        public OscParameterArray(IEnumerable<IOscParameter> value)
        {
            Value = value.ToImmutableList();
        }

        public OscParameterArray(params IOscParameter[] values)
            : this(values.ToImmutableList())
        {
        }

        public OscParameterArray(ImmutableList<IOscParameter> value)
        {
            Value = value;
        }

        public ImmutableList<IOscParameter> Value { get; }

        public OscTypeCode TypeCode => OscTypeCode.ParameterArrayStart;
        public object ValueUntyped => Value;

        public string TypeTag => $"[{string.Join("", Value.Select(p => p.TypeTag))}]";

        public static implicit operator ImmutableList<IOscParameter>(OscParameterArray p) => p.Value;

        public static implicit operator OscParameterArray(ImmutableList<IOscParameter> value) =>
            new OscParameterArray(value);
    }
}