# Showcase pass, English, 2026-09-25 (the Workshop gallery)

- **Command**: `Submit-PickleRun.ps1 -Mod ManyHappyReturns -Language English -DepMap wsl-deps.studio.map -Filter 15-publication-shots.feature -EvidenceDir ManyHappyReturns/Tests/Pickle/Evidence/2026-09-25-galerie-en` (request `20260925-131655-535-ae65`)
- **Suite state**: commit `2e28bc3` (the request carried no SHA; the run staged the tree of that moment, which held that commit and the CI files added by the CI/CD session, none of them read by the game)
- **Set name**: `studio` (the showcase colony `nelim-zen-meadow-studio`)
- **exitReason**: `failed`. The run went to the end
- **Scenarios**: 5 discovered, 5 played: 3 passed, 2 failed, 0 skipped
- **Evidence**: on disk, ignored by git (`Tests/Pickle/Evidence/2026-09-25-galerie-en/`): `junit.xml`, `summary.json`, `summary.md`, `Player.log` and the five raw captures. `report.html` and `messages.ndjson` were deleted.

## The two failures are the suite's, and the captures they left are the ones needed

`the day's memory` and `the forgotten birthday` asked to hover a tooltip containing "Quiet birthday" and "Nobody remembered my birthday".
The Needs tab of this game version already lists the memories in its mood section, so there was no tooltip region to find, and the
grade was wrong anyway (three wishers with a lover among them give "Lovely birthday", +8, not "Quiet birthday"). Both scenarios had
already built and asserted the state, and the capture of the step 17 frame shows it. Fix: the hover steps and the HoverSteps companion
were removed and the two scenarios renamed (`... in the Needs tab`). Replay requested: `20260925-174900-069-6e70`, not read yet.

## The five images, judged (every capture was opened)

Cropped from these captures into `Art/Workshop/`, in upload order:

| # | Image | Verdict |
| --- | --- | --- |
| 1 | The letter text: "Jet is 35 years old today. Nobody out here will make anything of it unless you do..." | Legible, states the pitch. Reserve: a text strip without the dialog's buttons; the game's own letter window is two thirds empty, so the crop keeps the text only |
| 2 | The Log tab: "Larson wished Jet a happy birthday." first among ordinary social lines, with its cake icon | Good: shows the wish is a normal interaction. Reserve: a fragment of map at the bottom right of the crop. The speech bubble is not visible at this zoom; the pawns are stacked in the full frame |
| 3 | The Needs tab, Mood list with "Lovely birthday +8" among the other thoughts | Good: shows the graded memory where a player finds it |
| 4 | The settings page with the mood scale at 150% and both switches on | Good: no debug text, slider away from its default |
| 5 | The Needs tab with "Nobody remembered my birthday -6" | Good: the sad half, honestly shown |

No developer tool, no Pickle panel and no raw key in any of the five crops. The full frames carry Anomaly letters of the fixture ("Area
revealed", "Fallen monolith") and alerts on the right, which is why the images are cropped to the window or pane that carries the subject.

## Player.log

Only the test companion's usual `did not load any content` line, and the two failure lines. Nothing from the mod.
