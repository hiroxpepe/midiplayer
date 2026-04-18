# Copilot Instructions for MidiPlayer repository

## Quick commands
- Build entire solution (Windows with Visual Studio/MSBuild):
  - msbuild "MidiPlayer.sln" /p:Configuration=Release
  - Or open MidiPlayer.sln in Visual Studio (recommended for Xamarin Android/Win64).

- Build from .NET SDK (only SDK-style projects):
  - dotnet build MidiPlayer.Midi\MidiPlayer.Midi.csproj

- Run tests (project uses MSTest / .NET 5):
  - dotnet test .\MidiPlayerTest\MidiPlayerTest.csproj
  - Run a single test by fully-qualified name:
    dotnet test .\MidiPlayerTest\MidiPlayerTest.csproj --filter "FullyQualifiedName=MidiPlayerTest.SynthTests.YourTestMethod"
  - Or use --filter "Name=YourTestMethod" depending on runner version.

- Lint: No repository-wide linter configured.

## High-level architecture
- Mono-repo for a Xamarin-based MIDI player with multiple projects:
  - MidiPlayer (shared UI/logic)
  - MidiPlayer.Midi (core MIDI abstractions/logic)
  - MidiPlayer.FluidSynth (FluidSynth wrapper/native integration)
  - MidiPlayer.SoundFont (soundfont handling)
  - MidiPlayer.Droid (Xamarin.Android app; older non-SDK csproj)
  - MidiPlayer.Win64 (Windows build, includes native libs)
  - MidiPlayerTest (MSTest test project targeting net5.0)
- Native binaries and platform-specific libraries are bundled per-project under `libs\` (Windows) and `libs\<arch>\` for Android. The Android project lists many AndroidNativeLibrary entries. Tests copy native DLLs from MidiPlayerTest\libs to test output.

## Key conventions and notes for Copilot sessions
- Mixed project styles: repo contains both SDK-style (net5.0) projects and legacy MSBuild Xamarin projects. Do NOT assume `dotnet build` will build Android/Xamarin projects—use Visual Studio/MSBuild for those.
- Native libraries are tracked in project files and expected in `libs/` or `libs\<arch>`. When proposing changes that touch native integration, reference these entries and ensure CopyToOutputDirectory is preserved.
- Tests use MSTest + Microsoft.NET.Test.Sdk. Use `dotnet test` in the `MidiPlayerTest` folder to run CI-friendly tests.
- When suggesting CI steps: build tests with `dotnet test` and build native/Xamarin artifacts with `msbuild` on Windows agents with Xamarin workloads installed.
- Keep edits minimal and targeted: many projects are interdependent; changing public APIs in core projects (MidiPlayer.Midi, MidiPlayer.FluidSynth) likely requires updating references across projects and tests.

## Where to look first
- README.md for project overview
- MidiPlayer.sln to see project grouping
- MidiPlayerTest/ for test examples showing usage of the public API and how native libs are loaded
- MidiPlayer.Droid/ and MidiPlayer.Win64/ csproj files to understand native library packaging

---
Created: Copilot instructions (concise build/test details, architecture, key conventions)
