using System;
using Sentry;
using Xunit;

namespace Symbolic.Tests
{
    public class DemangleTests
    {
        [Theory]
        [InlineData("_ZN3foo3barEv", "foo::bar()")]
        [InlineData("_ZN4main10main_innerEv", "main::main_inner()")]
        [InlineData("_ZNSt6vectorIiSaIiEE9push_backERKi", "std::vector<int, std::allocator<int> >::push_back(int const&)")]
        public void Demangle_CppItanium(string mangled, string expected)
        {
            var result = Sentry.Symbolic.Demangle(mangled);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("?foo@@YAHXZ", "int foo(void)")]
        [InlineData("?bar@Foo@@QEAAHH@Z", "public: int Foo::bar(int)")]
        public void Demangle_CppMsvc(string mangled, string expected)
        {
            var result = Sentry.Symbolic.Demangle(mangled);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("_RNvCskwGfYPst2Cb_3foo3bar", "foo::bar")]
        public void Demangle_Rust(string mangled, string expected)
        {
            var result = Sentry.Symbolic.Demangle(mangled);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Demangle_PlainSymbol_ReturnsNull()
        {
            Assert.Null(Sentry.Symbolic.Demangle("main"));
        }

        [Fact]
        public void Demangle_EmptyString_ReturnsNull()
        {
            Assert.Null(Sentry.Symbolic.Demangle(""));
        }

        [Fact]
        public void Demangle_NullArgument_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Sentry.Symbolic.Demangle(null!));
        }
    }
}
