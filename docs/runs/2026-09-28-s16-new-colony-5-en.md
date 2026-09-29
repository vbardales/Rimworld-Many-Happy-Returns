# New-colony pass, English, 2026-09-28, fifth attempt — green (S16's new-colony half, feature 17)

| Request | Label SHA | exitReason | Result |
| --- | --- | --- | --- |
| `20260928-182624-407-59df` | `78b0b54` | `passed` | 1 of 1, 19.1 s: colony started (Crashlanded, Cassandra, Rough, seed many-happy-returns-s16, 3 colonists), all landed in 8.0 s, birthday played from the letter to the verdict, settings opened and closed, `load-audit`: 386 log lines, 14 types, 6 defs, 0 finding, 0 translation problem |

- **Command**: `Submit-PickleRun.ps1 -Mod ManyHappyReturns -Language English -DepMap wsl-deps.avec-newcolony.map -Filter 17-new-colony -Extra "-pickle-scenario-timeout=400"` (evidence `2026-09-28-s16-nouvelle-colonie-5-en`)
- **What it closes**: the two PickleTools steps added this week (dialog accept, colonists-landed wait) work; the relation defect fixed in `93bb86b` no longer logs a warning. Feature 17, S16's last untested half, is now green once. Not reproducible: not replayed for nothing.
- **Evidence**: `junit.xml`, `summary.json`, `summary.md`, `Player.log`; `report.html` and `messages.ndjson` deleted.
