# Targeted pass, French, 2026-09-25 (the year turn)

- **Command**: `Submit-PickleRun.ps1 -Mod ManyHappyReturns -Language French -Filter "::a birthday on the last day of the year is judged" -EvidenceDir ManyHappyReturns/Tests/Pickle/Evidence/2026-09-24-cible-fr` (request `20260924-210441-198-2d11`)
- **Suite state**: commit `32999c6`
- **exitReason**: `passed`. Pickle exit 0, one attempt
- **Scenarios**: 1 discovered, 1 played, 1 passed: `a birthday on the last day of the year is judged when the year turns` (441 ticks, mean 5.7 ms)
- **Evidence**: on disk, ignored by git (`Tests/Pickle/Evidence/2026-09-24-cible-fr/`): `junit.xml`, `summary.json`, `summary.md`, `Player.log`.

## What it settles

The scenario that failed in the 2026-09-23 French pass (the wish refused before the year turn) passes. As in English, one sample: the
cause of the refusal is not proven, and the woken pawns and the cleared cooldown were both added since.
The other 38 scenarios of the French pass are not replayed: the 2026-09-23 French run remains their only proof, at an older revision.

## Player.log

Only the test companion's usual `did not load any content` line.
