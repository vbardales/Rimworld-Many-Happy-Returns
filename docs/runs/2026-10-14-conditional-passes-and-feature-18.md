# Conditional passes and feature 18, 2026-10-12 to 2026-10-14

Revision: step fixes `715fb90`, `621346d`, `593c6a9` (tests only, `Source/` and `Mod/` code unchanged since `v1.0.0`).
Evidence on disk: `Tests/Pickle/Evidence/2026-10-1[234]-*` (ignored by git).

| Pass | Evidence | exitReason | Result |
| --- | --- | --- | --- |
| RIMMSQOL 08, English | 2026-10-12-rimmsqol-08-en | passed | 4 of 4 |
| Without Anomaly 01, English | 2026-10-12-sans-anomaly-en | passed | 3 of 3 |
| Feature 18, English | 2026-10-13-log-gender-en | passed | 3 of 3 (2026-10-12: 0 of 3, suite read `LogEntry.randSeed`, now `logID`) |
| Feature 18, French | 2026-10-14-log-gender-fr | passed | 3 of 3 (2026-10-13: 0 of 3, suite tested the folder name `French`, the game reports `French (Francais)`) |
| Gifts and Birthdays 12, English | 2026-10-13-gifts-en, 2026-10-14-gifts-en | passed | 6 of 6 (5 of 6 on 2026-10-13, the sixth replayed after the suite brought the wisher back next to the celebrant) |
| RIMMSQOL restart chain 09, 10, 11 | 2026-10-14-rimmsqol-09-11-en | passed | 1 of 1 per launch, three launches (2026-10-12 and -13: chain ran in one launch, restart not tested) |

Every red before these was a suite defect, none of the mod.
