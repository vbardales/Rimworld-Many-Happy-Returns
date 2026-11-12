# New-colony pass, English, 2026-09-26, second attempt (S16's new-colony half, feature 17)

| Request | Label SHA | exitReason | Result |
| --- | --- | --- | --- |
| `20260926-165841-873-3efc` | `252dcab` | `failed` | 0 of 1. The new step "the new colony's colonists have landed" ran its full 90 s budget then failed: "Marjot spawned False, downed False, in ActiveTransporterInfo; Bruce ...; Tater ..." (all three, same state) |

- **Command**: `Submit-PickleRun.ps1 -Mod ManyHappyReturns -Language English -DepMap wsl-deps.avec-newcolony.map -Filter 17-new-colony -Extra "-pickle-scenario-timeout=400"` (evidence `2026-09-26-s16-nouvelle-colonie-2-en`)
- **Confirmed, not just a hypothesis this time**: the capture taken right after the failure shows the Crashlanded intro dialog ("The three of you awake...") still open, game paused, no colonist on the map. 90 s at fast speed did not move past it because PickleTools' step does not dismiss it.
- **Not the mod**: nothing of the mod ran again. The failure is entirely in the scenario's set-up.
- **Next**: facts sent to PickleTools (dialog confirmed as the blocker). No replay before its answer.
- **Evidence**: `junit.xml`, `summary.json`, `summary.md`, `Player.log`, the one capture; `report.html` and `messages.ndjson` deleted.
