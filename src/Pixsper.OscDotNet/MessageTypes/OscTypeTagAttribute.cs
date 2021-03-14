using System;
using System.Linq;

namespace Pixsper.OscDotNet.MessageTypes
{
    internal class OscTypeTagAttribute : Attribute
    {
        public OscTypeTagAttribute(char typeTag, Type valueType)
        {
            TypeTag = typeTag;
            ValueType = valueType;
        }

        public char TypeTag { get; }

        public Type ValueType { get; }

        public static OscTypeTagAttribute GetAttribute(Enum enumVal)
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(OscTypeTagAttribute), false);

            if (!attributes.Any() || attributes[0] is not OscTypeTagAttribute oscAttribute)
                throw new InvalidOperationException($"Couldn't find {nameof(OscTypeTagAttribute)} on '{enumVal}'");

            return oscAttribute;
        }
    }
}