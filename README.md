# Symbolic.NET

Cross-platform C++ symbol demangler for .NET, powered by [getsentry/symbolic](https://github.com/getsentry/symbolic).

Supports demangling of:
- C++ (Itanium ABI and MSVC)
- Rust (legacy and v0)

## Supported platforms

| RID | OS | Arch |
|-----|----|------|
| win-x86 | Windows | x86 |
| win-x64 | Windows | x64 |
| win-arm64 | Windows | ARM64 |
| linux-x86 | Linux (glibc) | x86 |
| linux-x64 | Linux (glibc) | x64 |
| linux-arm | Linux (glibc) | ARM |
| linux-arm64 | Linux (glibc) | ARM64 |
| linux-musl-x64 | Linux (musl) | x64 |
| linux-musl-arm | Linux (musl) | ARM |
| linux-musl-arm64 | Linux (musl) | ARM64 |
| osx-x64 | macOS | x64 |
| osx-arm64 | macOS | ARM64 |

## Usage

```csharp
using Sentry;

// C++ (Itanium)
Symbolic.Demangle("_ZN3foo3barEv");       // "foo::bar()"

// C++ (MSVC)
Symbolic.Demangle("?foo@@YAHXZ");         // "int foo(void)"

// Rust
Symbolic.Demangle("_RNvCskwGfYPst2Cb_3foo3bar"); // "foo::bar"

// Unknown or plain symbols return null
Symbolic.Demangle("main");                // null
```

## Building native libraries

```bash
# Build for all platforms (requires cross for Linux targets):
./native/build.sh

# Build for specific platforms:
./native/build.sh osx-arm64 linux-x64
```

## License

MIT, see [LICENSE](LICENSE).
