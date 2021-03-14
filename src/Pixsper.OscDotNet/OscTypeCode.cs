using System;
using System.Collections.Immutable;
using System.Linq;
using Pixsper.OscDotNet.MessageTypes;

namespace Pixsper.OscDotNet
{
    public enum OscTypeCode
    {
        [OscTypeTag('i', typeof(int))] Int32,
        [OscTypeTag('f', typeof(float))] Float,
        [OscTypeTag('s', typeof(string))] String,
        [OscTypeTag('b', typeof(byte[]))] Blob,
        [OscTypeTag('h', typeof(long))] Int64,
        [OscTypeTag('t', typeof(DateTime))] TimeTag,
        [OscTypeTag('d', typeof(double))] Double,
        [OscTypeTag('S', typeof(string))] SymbolString,
        [OscTypeTag('c', typeof(char))] Char,
        [OscTypeTag('r', typeof(OscColor))] RgbaColor,
        [OscTypeTag('m', typeof(OscMidiMessage))]MidiMessage,
        [OscTypeTag('T', typeof(bool))] TrueValue,
        [OscTypeTag('F', typeof(bool))] FalseValue,
        [OscTypeTag('N', typeof(OscTypeNil))] NilValue,
        [OscTypeTag('I', typeof(OscTypeImpulse))] ImpulseValue,
        [OscTypeTag('[', typeof(OscTypeImpulse))] ParameterArrayStart,
        [OscTypeTag(']', typeof(OscTypeImpulse))] ParameterArrayEnd
    }

    internal static class OscTypeCodeExtensions
    {
        private static readonly ImmutableDictionary<OscTypeCode, Tuple<char, Type>> TypeCodeChars;

        static OscTypeCodeExtensions()
        {
            TypeCodeChars = Enum.GetValues(typeof(OscTypeCode))
                .Cast<OscTypeCode>()
                .ToImmutableDictionary(t => t, t =>
                {
                    var memInfo = typeof(OscTypeCode).GetMember(t.ToString());
                    var attribute = (OscTypeTagAttribute) memInfo[0]
                        .GetCustomAttributes(typeof(OscTypeTagAttribute), false).First();
                    return Tuple.Create(attribute.TypeTag, attribute.ValueType);
                });
        }

        public static char GetTypeCodeChar(this OscTypeCode typeCode) => TypeCodeChars[typeCode].Item1;
    }
}