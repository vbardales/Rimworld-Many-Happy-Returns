# Verification results — 2026-09-13

Run `powershell -ExecutionPolicy Bypass -File Tests/Run.ps1` from the repository.
Requires .NET SDK 8, .NET Framework 4.8, and installed RimWorld 1.6 assemblies. Override
`-GameManaged` if necessary. The project pins compilation references to Krafs.Rimworld.Ref
1.6.4871. No test doubles or redistributed game DLLs are included.

Current output is in Results/build.txt, automated.txt and xml.txt. Results/manifest.json
records the standalone base revision, timestamp and SHA-256 of the checked working files,
including the delivered assembly. The runner rejects a tested/distributed DLL mismatch.

- Compilation: no warnings or errors.
- Behavior: 15 cases passed, including defaults/reset, finite and non-finite limits,
  real Scribe value callbacks for round-trip/old/partial settings, score boundaries,
  invalid interaction inputs, native shortcut inheritance and memory factor snapshots.
- XML: eight documents parse; all 16 source-owned Keyed keys have nonempty EN/FR text
  and matching parameters; all 18 French injection paths cover native English source;
  metadata, optional shortcut and distribution/image checks pass.
- Additional parent workflow validators: DefInjected, XML fields and Def references were
  run against the installed game. Their captured output is in Results/definjected.txt,
  xml-fields.txt and defrefs.txt. These external validator scripts are not a dependency
  of this standalone test runner.

## Limits and first test attempt

This is headless verification against the actual installed managed game assembly, not a
running Unity game. The first test harness attempted ScribeLoader.FinalizeLoading; Unity's
native profiler/logging calls failed outside Unity (nine cases passed, five encountered that
harness limitation). The harness was corrected to exercise the mod's actual LoadingVars and
PostLoadInit callbacks independently, with actual Scribe XML reads/writes. It does not replace
or fake those callbacks. Full save cross-reference processing and game restart remain untested.

MainButton visibility is checked through the delivered XML and inheritance of the game's
native Visible/Disabled implementations. Code inspection confirms Activate opens
Dialog_ModSettings for the same loaded ManyHappyReturnsMod instance; native PreClose writes
its settings. The boolean effects are traced to TrySendLetter and TryGiveForgotten; their
complete game behavior, GUI activation, rendering and RIMMSQOL integration are covered by the
pending scenarios. No customization mod version has been tested in game.

See TEST_SCENARIOS.md: every in-game scenario is **pending**, including EN/FR layout, full
saves, dependency combinations and new/existing colonies. These results justify technical
readiness under the user's options-gate clarification; they do not justify `tested`.
