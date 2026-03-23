using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Sentry
{
    internal static class NativeResolver
    {
        private const string LibraryName = "symbolic_native";

        [System.Runtime.CompilerServices.ModuleInitializer]
        internal static void Initialize()
        {
            NativeLibrary.SetDllImportResolver(typeof(NativeResolver).Assembly, Resolve);
        }

        private static IntPtr Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName != LibraryName)
            {
                return IntPtr.Zero;
            }

            string rid = GetRuntimeIdentifier();
            string ext = GetLibraryExtension();
            string prefix = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "" : "lib";
            string fileName = $"{prefix}{LibraryName}{ext}";

            string assemblyDir = Path.GetDirectoryName(assembly.Location) ?? ".";
            string path = Path.Combine(assemblyDir, "runtimes", rid, "native", fileName);

            if (NativeLibrary.TryLoad(path, out IntPtr handle))
            {
                return handle;
            }

            return IntPtr.Zero;
        }

        private static string GetRuntimeIdentifier()
        {
            string os;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                os = "win";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                os = "osx";
            else
                os = "linux";

            string arch = RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X86 => "x86",
                Architecture.X64 => "x64",
                Architecture.Arm => "arm",
                Architecture.Arm64 => "arm64",
                _ => throw new PlatformNotSupportedException(
                    $"Unsupported architecture: {RuntimeInformation.ProcessArchitecture}")
            };

            return $"{os}-{arch}";
        }

        private static string GetLibraryExtension()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return ".dll";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return ".dylib";
            return ".so";
        }
    }
}
