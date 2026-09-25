# Showcase pass replay, English, 2026-09-25 (the two memory scenarios)

- **Command**: `Submit-PickleRun.ps1 -Mod ManyHappyReturns -Language English -DepMap wsl-deps.studio.map -Filter "::the day's memory in the Needs tab,::the forgotten birthday in the Needs tab" -EvidenceDir ManyHappyReturns/Tests/Pickle/Evidence/2026-09-25-galerie-rejeu-en` (request `20260925-174900-069-6e70`, label SHA `1e256d1`)
- **Suite state**: the request carries no SHA. The tree was checked after the run: nothing under `Mod/` or `Tests/Pickle/Mod` and no step changed between `1e256d1` and the head (`git diff --stat 1e256d1..HEAD -- Mod Tests`: only `Tests/Pickle/README.md`, a documentation fix the game does not read)
- **Set name**: `studio`
- **exitReason**: `passed`. Pickle exit 0, one attempt, 20:39 to 20:41
- **Scenarios**: 2 discovered, 2 played, 2 passed
- **Evidence**: on disk, ignored by git (`Tests/Pickle/Evidence/2026-09-25-galerie-rejeu-en/`): `junit.xml`, `summary.json`, `summary.md`, `Player.log`. The report copies and the raw captures were deleted, and so were the raw captures of the first showcase run: the five validated images are in `Art/Workshop/`.

## What it settles

The two scenarios that were red in `2026-09-25-gallery-en.md` (their hover steps asked for a tooltip the Needs tab does not have) are green
once the hover steps are gone. The capture of the memory scenario was opened: the Needs tab lists "Lovely birthday +8", the same state as the
validated image 3, so the fixture reproduces it. With this run the showcase pass has no red scenario left.

## Player.log

The only `[ERROR]` lines are the four `did not load any content` lines of the companions (the three PickleTools and the suite's own): steps and features
only, no content. Nothing from the mod.
