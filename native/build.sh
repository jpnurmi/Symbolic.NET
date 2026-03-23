#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
RUNTIMES_DIR="$PROJECT_ROOT/runtimes"

declare -A TARGETS=(
    ["win-x86"]="i686-pc-windows-msvc"
    ["win-x64"]="x86_64-pc-windows-msvc"
    ["win-arm64"]="aarch64-pc-windows-msvc"
    ["linux-x86"]="i686-unknown-linux-gnu"
    ["linux-x64"]="x86_64-unknown-linux-gnu"
    ["linux-arm"]="armv7-unknown-linux-gnueabihf"
    ["linux-arm64"]="aarch64-unknown-linux-gnu"
    ["linux-musl-x86"]="i686-unknown-linux-musl"
    ["linux-musl-x64"]="x86_64-unknown-linux-musl"
    ["linux-musl-arm"]="armv7-unknown-linux-musleabihf"
    ["linux-musl-arm64"]="aarch64-unknown-linux-musl"
    ["osx-x64"]="x86_64-apple-darwin"
    ["osx-arm64"]="aarch64-apple-darwin"
)

declare -A LIB_NAMES=(
    ["win"]="symbolic_native.dll"
    ["linux"]="libsymbolic_native.so"
    ["osx"]="libsymbolic_native.dylib"
)

get_lib_name() {
    local rid="$1"
    case "$rid" in
        win-*)   echo "${LIB_NAMES[win]}" ;;
        linux-*) echo "${LIB_NAMES[linux]}" ;;
        osx-*)   echo "${LIB_NAMES[osx]}" ;;
    esac
}

build_target() {
    local rid="$1"
    local rust_target="${TARGETS[$rid]}"
    local lib_name
    lib_name="$(get_lib_name "$rid")"

    echo "Building $rid ($rust_target)..."

    local use_cross=false
    case "$rust_target" in
        *-apple-darwin)
            # Use cargo for macOS targets (cross doesn't support them well)
            ;;
        *)
            if command -v cross &>/dev/null; then
                use_cross=true
            fi
            ;;
    esac

    local build_cmd="cargo"
    if [ "$use_cross" = true ]; then
        build_cmd="cross"
    fi

    (cd "$SCRIPT_DIR" && $build_cmd build --release --target "$rust_target")

    local out_dir="$RUNTIMES_DIR/$rid/native"
    mkdir -p "$out_dir"
    cp "$SCRIPT_DIR/target/$rust_target/release/$lib_name" "$out_dir/"
    echo "  -> $out_dir/$lib_name"
}

if [ $# -gt 0 ]; then
    for rid in "$@"; do
        if [ -z "${TARGETS[$rid]+x}" ]; then
            echo "Unknown RID: $rid"
            echo "Available: ${!TARGETS[*]}"
            exit 1
        fi
        build_target "$rid"
    done
else
    for rid in "${!TARGETS[@]}"; do
        build_target "$rid"
    done
fi

echo "Done."
