# Minimal pass, French, 2026-09-23

- **Command**: `scripts/Run-PickleWsl.ps1 -Mod ManyHappyReturns -Language French -EvidenceDir ManyHappyReturns/Tests/Pickle/Evidence/2026-09-23-minimale-fr`
- **Set name**: `sans-facultatifs`. The game ran in French: the failure texts read "colon" and "bébé"
- **Suite state**: staged at 19:08, when the run began, and the earliest build with any review fix is 19:11: so this ran the
  same suite as the English pass, with none of the fixes made since
- **exitReason**: `failed`. Pickle exit 1
- **Scenarios**: 39 discovered, 39 accounted for: 24 passed, 5 failed, 10 skipped, 0 flaky
- **Evidence**: on disk, ignored by git: `junit.xml`, `summary.json`, `summary.md`, `Player.log` and the `@review` capture of the settings page (not yet opened). `report.html`, `messages.ndjson` and the failure captures were deleted the same day for disk space. The launcher's own copy stopped again on a path over 260 characters,
  and no captures were kept: the shared screenshot folder cannot tell this suite's images from other suites'.

## Same result as English, plus one

The ten skips are the same RIMMSQOL and Gifts and Birthdays scenarios as in English. The four failures are the same four and have
the same causes, since neither fix was staged: no forgotten memory for a celebrant of a young colony (three scenarios), and the
newborn refused by the generator ("bébé" among the downed lifestages).

The fifth is new, and it is in the year-turn scenario: `TryInteractWith` refused the birthday wish. That scenario passed in English.
The clock is moved to just before the end of the year, whose local hour depends on the map, so the likeliest cause is a colonist asleep
at that hour, which `CanInteractNowWith` refuses. `BringTogether` now wakes both pawns, and this run predates that change,
so the cause is a probable one and not shown. It is to be watched in the next run.

## What this pass adds

The letter scenarios passed in French: the letter is rebuilt from the mod's own key and matched, so a missing French key would have
failed them. The settings shortcut, the dialog close and the wish scenarios passed as in English. The `@review` capture of the settings
page was not opened, for the reason above.

## Not read

No capture was opened. The Player.log was read for the failures only.
