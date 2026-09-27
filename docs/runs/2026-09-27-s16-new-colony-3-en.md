# New-colony pass, English, 2026-09-27, third attempt (S16's new-colony half, feature 17)

| Request | Label SHA | exitReason | Result |
| --- | --- | --- | --- |
| `20260927-112957-699-7b8c` | `7e9af90` | `failed` | 0 of 1. "any open message dialog is accepted" passed (0.267 s); "the new colony's colonists have landed" then failed after its full 90 s: "Marjot spawned False, downed False, in ActiveTransporterInfo; Bruce ...; Tater ..." — same message as run `3efc` |

- **Command**: `Submit-PickleRun.ps1 -Mod ManyHappyReturns -Language English -DepMap wsl-deps.avec-newcolony.map -Filter 17-new-colony -Extra "-pickle-scenario-timeout=400"` (evidence `2026-09-27-s16-nouvelle-colonie-3-en`)
- **New fact**: the dialog-accept step reported success, but the capture taken after the failure still shows the same Crashlanded intro dialog open ("The three of you awake..."), game paused, no colonist on the map. The step's own success does not mean the dialog was gone afterward.
- **Not the mod**: nothing of the mod ran, third time.
- **Next**: facts sent to PickleTools. No replay before its answer.
- **Evidence**: `junit.xml`, `summary.json`, `summary.md`, `Player.log`, the one capture; `report.html` and `messages.ndjson` deleted.
