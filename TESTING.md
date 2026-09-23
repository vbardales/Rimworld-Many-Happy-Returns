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

**Written on 2026-09-23, not yet run.** Twelve feature files; scope, what stays manual, and the
mutation/teardown design are in [Tests/Pickle/README.md](Tests/Pickle/README.md). Summary here per
`AUDIT.md`'s instruction to name how many passes a verdict needs and what each covers.

| Pass | Command | What it establishes |
| --- | --- | --- |
| Minimal | `Run-PickleWsl.ps1 -Mod ManyHappyReturns` | The mod stands alone: no optional integration staged. Feature 01's Gifts and Birthdays scenario is expected green here *because* that mod is absent. |
| With Gifts and Birthdays | `Run-PickleWsl.ps1 -Mod ManyHappyReturns -DepMap wsl-deps.avec-gifts-and-birthdays.map` | The optional lookup actually finds `BirthdayCongratulationReceived` when that mod is loaded. Workshop id `3625791734` confirmed 2026-09-23 by reading the downloaded item's own `About.xml`: `packageId KrukuCoB.rout`, matching this mod's declaration. Feature 12 drives its real congratulation and its real party class (S13). |
| With RIMMSQOL | `Run-PickleWsl.ps1 -Mod ManyHappyReturns -DepMap wsl-deps.avec-rimmsqol.map` with `-Filter 08-rimmsqol-shortcut.feature`, then `-Filter 09-rimmsqol-restart-write.feature -Then @(10, 11)` from PowerShell | The shortcut revealed by RIMMSQOL's own settings instance, opening this mod's settings, and its visibility surviving two real restarts (S12). Never run unfiltered: the readers would run in the writer's process. |
| French | `Run-PickleWsl.ps1 -Mod ManyHappyReturns -Language French` | Same suite, same scenarios: nothing here spells an English string, so a missing key shows as accented gibberish in developer mode rather than passing quietly. |

No `incompatibleWith` is declared in `About.xml`, so there is no declared-incompatibility pass to
write.

Also covered by the written suite since 2026-09-23: S11 (the day closing on its own and the year
wrap, feature 07), S12 (features 08 to 11), and S13 (feature 12). Not yet written: S10's Downed,
caravan and fewer-than-two-witnesses exemptions, and S14's Anomaly guard — see `STATUS.md` for where
each stands. Birthday Variety's date randomization stays unclaimed as runtime-tested.
