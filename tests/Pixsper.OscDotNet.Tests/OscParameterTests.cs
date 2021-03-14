using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pixsper.OscDotNet.Tests
{
    [TestClass]
    public class OscParameterTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
        public void IsParameterEqualToDataType(IOscParameter parameter, object value)
        {
            var result = parameter == value;

            (parameter == value).Should().BeTrue();
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[] { new OscParameterInt(0), 0 };
            yield return new object[] { new OscParameterFloat(0f), 0f };
            yield return new object[] { new OscParameterString("foo"), "foo" };
            yield return new object[] { new OscParameterBlob(new byte[] { 0, 1, 2 }), new byte[] { 0, 1, 2 } };
        }
    }
}
