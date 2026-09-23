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

**Written on 2026-09-23, not yet run.** Six feature files; scope, what stays manual, and the
mutation/teardown design are in [Tests/Pickle/README.md](Tests/Pickle/README.md). Summary here per
`AUDIT.md`'s instruction to name how many passes a verdict needs and what each covers.

| Pass | Command | What it establishes |
| --- | --- | --- |
| Minimal | `Run-PickleWsl.ps1 -Mod ManyHappyReturns` | The mod stands alone: no optional integration staged. Feature 01's Gifts and Birthdays scenario is expected green here *because* that mod is absent. |
| With Gifts and Birthdays | `Run-PickleWsl.ps1 -Mod ManyHappyReturns -DepMap wsl-deps.avec-gifts-and-birthdays.map` | The optional lookup actually finds `BirthdayCongratulationReceived` when that mod is loaded. **Not ready**: `wsl-deps.avec-gifts-and-birthdays.map` has its one dependency line commented out because KrukuCoB's Workshop id could not be confirmed when this suite was written (Steam rate-limited two lookups). Confirm the id before running this pass. |
| French | `Run-PickleWsl.ps1 -Mod ManyHappyReturns -Language French` | Same suite, same scenarios: nothing here spells an English string, so a missing key shows as accented gibberish in developer mode rather than passing quietly. |

No `incompatibleWith` is declared in `About.xml`, so there is no declared-incompatibility pass to
write.

Left manual, matching `TEST_SCENARIOS.md`'s own scenarios: RIMMSQOL's own interface and restart
persistence (S12), a Downed or caravan celebrant and fewer than two witnesses (part of S10),
Anomaly's Inhumanized guard under that DLC (S14), and Birthday Variety's date randomization if it
is ever advertised as runtime-tested. Reasons for each are in Tests/Pickle/README.md.
