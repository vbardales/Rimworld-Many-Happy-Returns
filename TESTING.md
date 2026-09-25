# Testing plan — Many Happy Returns

Two layers, per `AUDIT.md`'s priority: everything provable outside a running game is proven
outside one; Pickle covers only what a running game alone can show.

## Offline (`Tests/ManyHappyReturns.Tests.csproj`)

Written, executed, green. See [Tests/RESULTS.md](Tests/RESULTS.md) for commands, evidence paths
and the exact revision tested. 15 behavior cases, 8 XML documents, 16 Keyed keys and 18 French
DefInjected paths checked. Re-run any time with:

```powershell
powershell -ExecutionPolicy Bypass -File Tests/Run.ps1
```

## Pickle (`Tests/Pickle/`)

**Written on 2026-09-23. The minimal English and French passes ran that day and their failures were fixed; the other
passes have not run yet** (`docs/runs/`). Fifteen feature files; scope, what stays manual, and the
mutation/teardown design are in [Tests/Pickle/README.md](Tests/Pickle/README.md). Summary here per
`AUDIT.md`'s instruction to name how many passes a verdict needs and what each covers.

| Pass | Command | What it establishes |
| --- | --- | --- |
| Minimal | `Run-PickleWsl.ps1 -Mod ManyHappyReturns` | The mod stands alone: no optional integration staged. Feature 01's Gifts and Birthdays scenario is expected green here *because* that mod is absent. |
| With Gifts and Birthdays | `Run-PickleWsl.ps1 -Mod ManyHappyReturns -DepMap wsl-deps.avec-gifts-and-birthdays.map` | The optional lookup actually finds `BirthdayCongratulationReceived` when that mod is loaded. Workshop id `3625791734` confirmed 2026-09-23 by reading the downloaded item's own `About.xml`: `packageId KrukuCoB.rout`, matching this mod's declaration. Feature 12 drives its real congratulation and its real party class (S13). |
| With RIMMSQOL | `Run-PickleWsl.ps1 -Mod ManyHappyReturns -DepMap wsl-deps.avec-rimmsqol.map` with `-Filter 08-rimmsqol-shortcut.feature`, then `-Filter 09-rimmsqol-restart-write.feature -Then @(10, 11)` from PowerShell | The shortcut revealed by RIMMSQOL's own settings instance, opening this mod's settings, and its visibility surviving two real restarts (S12). Never run unfiltered: the readers would run in the writer's process. |
| Without Anomaly | `Run-PickleWsl.ps1 -Mod ManyHappyReturns -DepMap wsl-deps.sans-anomaly.map` | S14's first half: the `MayRequire` guard on `Inhumanized` reads absent, with no unresolved reference and no error, where the default pass reads it listed. |
| Showcase | `Submit-PickleRun.ps1 -Mod ManyHappyReturns ... -DepMap wsl-deps.studio.map -Language English -Filter 15-publication-shots.feature` | The five Workshop images (`PUBLICATION.md`), on the showcase colony. Produced by the run, judged by a person who opens each one. |
| French | `Run-PickleWsl.ps1 -Mod ManyHappyReturns -Language French` | Same suite, same scenarios: nothing here spells an English string, so a missing key shows as accented gibberish in developer mode rather than passing quietly. |

No `incompatibleWith` is declared in `About.xml`, so there is no declared-incompatibility pass to
write.

Every scenario of TEST_SCENARIOS.md that can be automated is now written: S10 (features 06, 13, 14),
S11 (07), S12 (08 to 11), S13 (12), S14's first half (01, run in two passes). Left, with reasons:
the engine's own refusal of a memory for an inhumanized pawn (the game's `nullifyingHediffs` handling,
not this mod's), S15 and S16 (a person reading the interface and the logs), and Birthday Variety's date
randomization, which is not claimed as runtime-tested.
