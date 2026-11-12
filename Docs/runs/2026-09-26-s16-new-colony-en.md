# New-colony pass, English, 2026-09-26 (S16's new-colony half, feature 17)

| Request | Label SHA | exitReason | Result |
| --- | --- | --- | --- |
| `20260926-104659-688-ed90` | `543f4f8` | `failed` | 0 of 1. The step "a new colony is started" passed (7.1 s); the next, "the celebrant is the map's first eligible colonist", failed: "the map has 0 eligible, non-downed free colonists" |

- **Command**: `Submit-PickleRun.ps1 -Mod ManyHappyReturns -Language English -DepMap wsl-deps.avec-newcolony.map -Filter 17-new-colony -Extra "-pickle-scenario-timeout=400"` (evidence `2026-09-26-s16-nouvelle-colonie-en`)
- **Reading**: the capture after the failure shows the Crashlanded intro dialog still open, three colonists in the top bar and none on the map. Hypothesis, not proven: the pawns are still in their pods when the step returns.
- **Not the mod**: nothing of the mod ran. The failure is in the scenario's set-up on a colony that has not landed yet.
- **Next**: a question sent to the PickleTools session (does the step promise spawned colonists, and what to wait for). No replay before its answer: a new colony is not reproducible.
- **Evidence**: `junit.xml`, `summary.json`, `summary.md`, `Player.log`, the one capture; `report.html` and `messages.ndjson` deleted.
