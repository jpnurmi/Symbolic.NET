using System;
using System.Runtime.InteropServices;

namespace Sentry
{
    internal static partial class NativeMethods
    {
        private const string LibraryName = "symbolic_native";

        [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
        public static partial IntPtr symbolic_demangle_symbol(string input);

        [LibraryImport(LibraryName)]
        public static partial void symbolic_demangle_free(IntPtr s);
    }
}
