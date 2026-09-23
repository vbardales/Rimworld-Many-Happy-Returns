# Minimal pass, English, 2026-09-23

- **Command**: `scripts/Run-PickleWsl.ps1 -Mod ManyHappyReturns -Language English -EvidenceDir ManyHappyReturns/Tests/Pickle/Evidence/2026-09-23-minimale-en`
- **Set name**: `sans-facultatifs` (Core, DLC, Harmony, RimLogging, Pickle, this mod and its test companion: 11 mods, all loaded)
- **Suite state**: the commit before `feaf85e`, i.e. eleven feature files as written that afternoon
- **exitReason**: `failed`. Pickle exit 1
- **Scenarios**: 39 discovered, 39 accounted for: 25 passed, 4 failed, 10 skipped, 0 flaky
- **Evidence**: on disk, ignored by git (`Tests/Pickle/Evidence/2026-09-23-minimale-en/`): `junit.xml`, `summary.json`, `summary.md` and `Player.log`. The heavy `report.html`, `messages.ndjson` and the four failure captures were deleted the same day (disk space; the failures are fixed and their causes are written below).
  The launcher's own copy stopped on a path over 260 characters, and the rest of the shared report folder belongs to other suites.

## The ten skips are the expected ones

Seven need RIMMSQOL (features 08 to 11) and three need Gifts and Birthdays (feature 12). They skip in this pass by design and
belong to the other passes. A green minimal pass says nothing about them.

## The four failures

None is a defect of the mod that the run can show. All are the suite.

| Scenario | Cause |
| --- | --- |
| an unwished birthday forms the forgotten memory | No forgotten memory for the celebrant. The mod requires ten days as a colonist and the test-colony fixture is a young colony. |
| an unwished birthday is judged at midnight too | Same cause. |
| with two other colonists on the map the birthday can be forgotten | Same cause. |
| a colonist born today is on the calendar and off the list | `Log.Error`: a baby colonist would include downed lifestages. `PawnGenerationRequest` needed `allowDowned: true`. |

Fixes made after the run: a step that sets the celebrant's tenure, used by every scenario that expects the forgotten memory;
the same step in feature 13's Background; `allowDowned: true` for the newborn; and a failure message that now names the
conditions (setting, days as colonist, witnesses, spawned, downed).

## What the green results are worth

Several passes are **vacuous until the tenure fix is played**: with a young colony the celebrant could not be forgotten, so
"switching the forgotten setting off suppresses the memory", "a colonist of one day's tenure is exempt", "a psychopath is
exempt on an unwished birthday", "a Downed celebrant is not judged", "away in a caravan" and "with a single other colonist"
all passed without proving their exemption. They are not evidence of the exemptions until they are rerun with a
long-established celebrant. The other passes are real: defs resolved, the Anomaly guard, the letter (once, not repeated, not sent
when switched off), the shortcut contract, settings persisting through a real dialog close, the wish and its
duplicate protection, a wished verdict, a save and reload, and a wished birthday judged at midnight and at the year turn.

## Not read

The `@review` capture of feature 03 was not identified in the shared report folder, so no image was opened. The
Player.log was read for the failures only, not for startup errors.
