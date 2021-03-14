using System;
using System.Collections.Immutable;
using System.Linq;
using Pixsper.OscDotNet.MessageTypes;

namespace Pixsper.OscDotNet.Helpers
{
    public static class OscTypeCodeHelper
    {
        static OscTypeCodeHelper()
        {
            CharToTypeCodes = Enum.GetValues(typeof(OscTypeCode))
                .Cast<OscTypeCode>()
                .ToImmutableDictionary(e => OscTypeTagAttribute.GetAttribute(e).TypeTag);

            TypeCodesToChar = CharToTypeCodes.ToImmutableDictionary(p => p.Value, p => p.Key);

            OscTypeCodeToSystemTypeCode = Enum.GetValues(typeof(OscTypeCode))
                .Cast<OscTypeCode>()
                .ToImmutableDictionary(e => e, e => Type.GetTypeCode(OscTypeTagAttribute.GetAttribute(e).ValueType));
        }
        

        public static ImmutableDictionary<char, OscTypeCode> CharToTypeCodes { get; }

        public static ImmutableDictionary<OscTypeCode, char> TypeCodesToChar { get; }

        public static ImmutableDictionary<OscTypeCode, TypeCode> OscTypeCodeToSystemTypeCode { get; }
    }
}