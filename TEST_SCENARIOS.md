# Functional validation — RimWorld 1.6

Status: **not executed in game**. Automated evidence is separate in Tests/RESULTS.md.
Record date, game version, language, mod versions/load order, save name, observed result
and Player.log for every run. Back up an existing save before these scenarios.

Common setup: development mode, Core and Many Happy Returns, optional dependencies absent,
three awake adult humanlike free colonists on the same map, one with at least ten days as a
colonist. Use a disposable colony/save. Do not use Increment time: it skips simulation ticks.
Run the applicable scenarios in both English and French; examine all displayed strings for
raw keys, English fallback, incorrect grammar, clipping and exceptions.

| ID | Preconditions | Actions | Expected result |
| --- | --- | --- | --- |
| S01 First use / primary route | Back up then remove only this mod's settings file; no customization mod | Open Mod options -> Many Happy Returns | Letter on, forgotten memory on, mood 100%; three useful controls, scope text and reset; no error and no visible or greyed-out MainButton |
| S02 Editing / bounds / reset | Settings page open | Toggle both controls; move slider to each end and intermediate values; reset | Slider remains 50–200% in 5% increments; reset gives on/on/100%; translated text stays readable |
| S03 Persistence / scope | Change settings to off/off/175% and close the native dialog | Reopen; exit/restart the game; reload the save; load another save | Same global values throughout; save-specific birthday state does not overwrite mod settings |
| S04 Morning announcement | Default settings; target's birthday not already tracked today; local hour before 20 | Debug actions -> Many Happy Returns -> Birthday is today, choose target; allow an hourly rescan | Exactly one translated letter and one translated debug confirmation; no duplicate letter on rescan or save/reload |
| S05 Letter switch | Separate fresh birthday target per toggle; detect before hour 20 | Disable letters, set first target's birthday today; re-enable and do the same for another target | No letter for first; letter for second. Already tracked birthdays are not announced retroactively |
| S06 Wishes / duplicate protection | Eligible birthday target with another colonist able to interact | Let normal interactions run; inspect social log and opinion memories; log tally | Birthday wish appears naturally; +10 received/+5 given opinion, no mood on the wish; repeated meetings do not count that same wisher twice |
| S07 Grade thresholds / mood scale | Arrange distinct wishers and optional family/meal/party bonuses; use fresh target/save per case | Log tally, then End today's birthdays now for scores 1, 2, 4, 6, 9; repeat at 50% and 200% | One positive memory with base mood +2/+4/+6/+8/+10 respectively, scaled only for this mod, duration five days; translated stage text |
| S08 Existing memories | Target already has this mod's positive or negative memory at 100% | Change slider to 200%; create a later birthday memory on another pawn | Existing memory retains its original factor; new memory has doubled mood; opinion and other mods' thoughts unchanged |
| S09 Forgotten toggle | Eligible long-established target, two other colonists present, no wishes | Close day with forgotten enabled; repeat on a clean equivalent save with it disabled | Enabled yields the -6 base mood memory; disabled yields none; toggling back restores future forgotten memories |
| S10 Exemptions | Fresh independent fixtures: recruit under ten days, fewer than two other colonists, downed target at verdict, caravan target, psychopath, baby | Run birthday with no wishes and close day | No forgotten memory in each exempt case; no null errors; psychopath can still receive a positive birthday verdict |
| S11 Midnight / saves | Naturally running birthday; save during the day | Save/reload, then let simulation cross midnight without manual debug closeout | One verdict; next day clears tracked birthdays; reload does not repeat letter or verdict. Repeat around year boundary and on an existing save without prior mod state |
| S12 Optional shortcut | Install RIMMSQOL compatible with the tested game version; clean button customization | Reveal Nelim_ManyHappyReturnsSettings; activate it; edit settings; open primary route; hide shortcut and restart | Same native settings dialog, values and persistence; visibility choice survives as controlled by RIMMSQOL. Record exact version; do not claim other tools tested |
| S13 Dependency absent/present | First run Core + this mod; second run add Gifts and Birthdays, recording its version | Without dependency, run S06–S09; with it, attend its birthday party and get its congratulations plus normal wishes | No dependency errors when absent; when present, distinct wishers include its congratulations, the same pawn is counted once, and the party bonus applies |
| S14 Anomaly condition | First run without Anomaly; second with Anomaly and an inhumanized target | Load defs and complete a birthday | No unresolved Inhumanized reference without DLC; positive/negative mood memories nullified for inhumanized pawn with DLC |
| S15 Debug localization | English then French UI, playing on map | Open all three debug actions, hover/use pawn tools and trigger both confirmations | Actions and cursor names follow language; confirmations are localized; technical tally remains English in Player.log; actions unavailable outside playing-on-map state |
| S16 Final regressions | New colony and backed-up existing save | Open both settings routes, repeat relevant cases after any fix, inspect Player.log and dev error list | No mod-related exceptions, raw keys, unresolved defs or repeated warnings. Record each observed result before marking tested |

Birthday Variety compatibility is based on reading the calendar birthday; if that integration
is to be advertised as runtime-tested, record its exact version and repeat S04/S11 after date
randomization. Other MainButton customization tools require their own S12 run.

Non-applicable: no text entry widget, no per-save settings UI, no required external framework,
no Harmony patches, no restart-only controls, no Workshop upload as part of validation.
Malformed/outdated stored numeric values are covered by automated Scribe callback tests;
full game startup and save integration remain S03/S11.
