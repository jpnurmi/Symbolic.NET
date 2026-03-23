using Sentry;
using Xunit;

namespace Symbolic.IntegrationTests
{
    public class DemangleTests
    {
        [Theory]
        [InlineData("_ZN3foo3barEv", "foo::bar()")]
        [InlineData("?foo@@YAHXZ", "int foo(void)")]
        [InlineData("_RNvCskwGfYPst2Cb_3foo3bar", "foo::bar")]
        public void Demangle_ReturnsExpected(string mangled, string expected)
        {
            Assert.Equal(expected, Symbolic.Demangle(mangled));
        }

        [Fact]
        public void Demangle_UnknownSymbol_ReturnsNull()
        {
            Assert.Null(Symbolic.Demangle("main"));
        }
    }
}
