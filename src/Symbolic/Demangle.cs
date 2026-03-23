using System;
using System.Runtime.InteropServices;

namespace Sentry
{
    public static class Symbolic
    {
        /// <summary>
        /// Demangles a C++, Rust, or Swift symbol name.
        /// Returns the demangled name, or null if the symbol could not be demangled.
        /// </summary>
        public static string? Demangle(string symbol)
        {
            ArgumentNullException.ThrowIfNull(symbol);

            IntPtr result = NativeMethods.symbolic_demangle_symbol(symbol);
            if (result == IntPtr.Zero)
            {
                return null;
            }

            try
            {
                return Marshal.PtrToStringUTF8(result);
            }
            finally
            {
                NativeMethods.symbolic_demangle_free(result);
            }
        }
    }
}
