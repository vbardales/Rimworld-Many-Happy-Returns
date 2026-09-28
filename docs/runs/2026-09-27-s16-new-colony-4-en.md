# New-colony pass, English, 2026-09-27, fourth attempt (S16's new-colony half, feature 17)

| Request | Label SHA | exitReason | Result |
| --- | --- | --- | --- |
| `20260927-180434-553-df29` | `f9249e1` | `failed` | 0 of 1. Steps 1 to 24 passed, the colonists landed (9.3 s after the dialog step), the birthday ran from the letter to the verdict and the settings opened and closed. The last step, the load audit, failed: 2 warnings, "Child is not a direct relation." and "Sibling is not a direct relation.", stack in `ManyHappyReturns.BirthdayUtility.IsCloseTo` |

- **Command**: `Submit-PickleRun.ps1 -Mod ManyHappyReturns -Language English -DepMap wsl-deps.avec-newcolony.map -Filter 17-new-colony -Extra "-pickle-scenario-timeout=400"` (evidence `2026-09-27-s16-nouvelle-colonie-4-en`)
- **Real defect of the mod**: `IsCloseTo` called `Pawn_RelationsTracker.DirectRelationExists` for Child and Sibling. Both are implied relations: the game logs a warning and returns false. Effect: children and siblings never counted as a close relation in the verdict, and the game logged a warning at every verdict. Fixed in `93bb86b` with `Pawn.GetRelations`.
- **Earlier logs**: the gallery runs of 2026-09-25 hold 10 such warnings each and were not read for it; the S16 runs on the saved colony hold none.
- **Fixed by**: `93bb86b`. The revision changed, so the previous green runs no longer prove the current build.
- **Evidence**: `junit.xml`, `summary.json`, `summary.md`, `Player.log`, the captures; `report.html` and `messages.ndjson` deleted.
