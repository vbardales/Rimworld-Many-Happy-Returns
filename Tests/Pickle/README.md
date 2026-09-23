# The Pickle suite for Many Happy Returns

Fourteen feature files, written and **not yet run**. They hold what only a running game can show;
everything provable outside one is proven outside one, in `Tests/ManyHappyReturns.Tests.csproj`
(15 passing cases: defaults/reset, NaN/Infinity/out-of-range clamping, real Scribe callbacks,
score thresholds, wisher-count weighting, invalid interaction inputs, native shortcut inheritance,
memory-factor snapshots — see `Tests/RESULTS.md`).

A run takes the whole machine for tens of minutes, against seconds for the executable suite. That
is the reason for the line drawn below, and for keeping this suite small: this mod is 975 lines of
C# with no Harmony patches, not a framework with dozens of moving parts.

## What lives here and what does not

`Tests/ManyHappyReturns.Tests.csproj` already answers the pure decision layer: which grade a score
maps to, how many colonists' wishes are worth, the settings' clamping, and a headless Scribe round
trip through the mod's own `LoadingVars`/`PostLoadInit` callbacks. **None of that is repeated
here.**

What is left needs a game, and needs it for a concrete reason:

| Feature | Why a running game |
| --- | --- |
| 01 loading | `MHRDefOf`'s static constructor actually resolved every def against the real def database (it throws otherwise); the optional Gifts and Birthdays lookup against what this pass really loaded |
| 02 morning letter | A real `Find.LetterStack`, and that a second real rescan the same day does not queue the letter twice |
| 03 settings shortcut | What the main bar's own worker does with a real `MainButtonDef`, and which mod a real `Dialog_ModSettings` was built for |
| 04 settings persistence | That closing a real `Dialog_ModSettings` actually calls `Mod.WriteSettings` (`Window.PreClose`), and that a freshly re-read `GetSettings<T>()` agrees with the file |
| 05 wishes and duplicate protection | `InteractionWorker_BirthdayWish.RandomSelectionWeight` and `Pawn_InteractionsTracker.TryInteractWith` against real `Pawn` objects — memory handlers, relations, developmental stage, all wired together the way only the game wires them |
| 06 verdict and forgotten | `MemoryThoughtHandler.TryGainMemory`, where `nullifyingTraits` and the settings' mood factor are actually enforced, plus a real save/reload |
| 07 midnight and year boundary | The day closing on its own: `GameComponent_Birthdays` judging a birthday when the absolute day changes, across the year wrap, and a save/reload mid-day that must not repeat the letter (S11) |
| 08 to 11 RIMMSQOL | The shortcut revealed through RIMMSQOL's own settings instance and PickleTools' `RimmsqolSteps`, opening this mod's settings, and its visibility surviving a real restart over three launches (S12) |
| 12 Gifts and Birthdays | Its real congratulation counted once, its real `LordJob_BirthdayParty` recognised as a party by `DayQualityBonus`, and the resulting stage against the day's own tally (S13) |
| 13 exemptions | `Evaluate`'s early returns and `TryGiveForgotten`'s witness floor against real pawns: a celebrant Downed, away in a real caravan, and the floor of two witnesses played on both sides (S10) |
| 14 newborn | A baby generated the way the game generates one, born today, on the calendar and refused by `CanCelebrate`; needs Biotech (S10) |

## Roles, not names

Scenarios refer to **the celebrant** and **the wisher**, never to a name from the fixture: the
exact colonist roster of whichever save is staged is not something this suite controls.
`RoleSteps.cs` picks the map's first and second eligible free colonist; every later step resolves
the role fresh by the colonist's persistent in-game name, not by holding on to the C# `Pawn`
reference — a save/reload replaces every object in the game, and a stale reference would assert
against a pawn nothing draws from any more (see the [authoring guide](../../../PickleTools/Authoring/README.md)).

`the wisher is brought next to the celebrant` moves the wisher to a walkable cell within
line-of-sight range before an exchange: `TryInteractWith` and `RandomSelectionWeight` both need
that (`SocialInteractionUtility.IsGoodPositionForInteraction`, six tiles), and a played fixture is
not guaranteed to start its colonists near each other.

## Mutations, and what reverts them

`VerdictSteps.cs` forces two things onto the celebrant to reach exemptions TEST_SCENARIOS.md
documents (S10): a one-day tenure (`Pawn_RecordsTracker`'s private `records` `DefMap`, set
directly — its own indexer is public) and the Psychopath trait (`TraitSet.GainTrait`, looked up by
`defName` exactly as `Mod/Defs/Birthday.xml`'s own `nullifyingTraits` entry does; `TraitDefOf`
carries no constant for it). Both are reverted in `[AfterScenario]`, on the same live `Pawn`
objects: no scenario in this suite combines these mutations with a save/reload, so the teardown
never needs to survive one.

`SettingsSandbox.cs` follows the collection's established pattern (SkillIcons, ArchitectStudio,
WorkStudio, Housebroken, ContentedLivestock, FieldworkCompanions): back up the real settings file
before a scenario, restore it after, and restore from an orphaned backup first if a previous run
died mid-scenario.

## Left manual, and why

Nothing below is a defect. It is work this suite does not perform, matching what
`TEST_SCENARIOS.md` already tracks as its own manual scenarios (S14, second half only: everything else is written).

- **The engine's own refusal of a memory for an inhumanized pawn** (the second half of S14). The
  mod declares `nullifyingHediffs` on both thoughts; feature 01 proves the declaration follows the DLC
  in both passes. That the game then declines the memory is `MemoryThoughtHandler`'s own behaviour,
  which AUDIT.md's "on ne teste pas le jeu" leaves to the game. Not applicable, with that reason.
- **Every `@review` screenshot**: a green run says the trip happened, not that the image shows
  anything correct. That review is a human step, not a scenario.

## How many passes a verdict needs

**1. Without the optional mods** — the default staging: Core, Harmony (not actually used by this
mod, but part of the default set `scripts/stage-pickle-wsl.sh` stages), Pickle, and this mod's own
suite. No hard dependency to add.

```powershell
powershell.exe -ExecutionPolicy Bypass -File scripts/Run-PickleWsl.ps1 -Mod ManyHappyReturns
```

Proves the mod stands alone. Feature 01's Gifts and Birthdays scenario is green here specifically
*because* that mod is absent — read the correspondence, not a fixed pass/fail.

**2. With Gifts and Birthdays** — `wsl-deps.avec-gifts-and-birthdays.map`, staging Workshop id
`3625791734`. Confirmed 2026-09-23 by reading that item's own downloaded `About.xml`
(`steamapps/workshop/content/294100/3625791734/About/About.xml`): `packageId` `KrukuCoB.rout`,
matching this mod's own `<loadAfter>`, and its `Defs/ThoughtDefs/Thoughts_Birthday.xml` does define
`BirthdayCongratulationReceived`. (An earlier Steam search under that id had returned the display
name "KrikiCoB" rather than "KrukuCoB" — that is the Steam account's display name, not the
`About.xml` author field, and the two differ.)

```powershell
powershell.exe -ExecutionPolicy Bypass -File scripts/Run-PickleWsl.ps1 -Mod ManyHappyReturns -DepMap wsl-deps.avec-gifts-and-birthdays.map
```

**3. Each language, in its own pass.** No scenario here spells an English string — the letter and
every debug confirmation are rebuilt from the mod's own translation key — so the same suite is the
French check, and the `@review` capture in feature 03 is where a missing key would show as
accented gibberish rather than clean English.

```powershell
powershell.exe -ExecutionPolicy Bypass -File scripts/Run-PickleWsl.ps1 -Mod ManyHappyReturns -Language French
```

**4. With RIMMSQOL** — `wsl-deps.avec-rimmsqol.map`, staging RIMMSQOL (Workshop id 1084452457) and
PickleTools' `RimmsqolSteps`. Features 08 to 11 need it and skip without it. 08 is one process; 09, 10
and 11 are the three launches of a restart chain under one hold of the lock, and 10 and 11 refuse to
pass in the process that wrote. From PowerShell, with a real array (see the authoring guide):

```powershell
& ./scripts/Run-PickleWsl.ps1 -Mod ManyHappyReturns -DepMap wsl-deps.avec-rimmsqol.map -Language English -Filter 08-rimmsqol-shortcut.feature
& ./scripts/Run-PickleWsl.ps1 -Mod ManyHappyReturns -DepMap wsl-deps.avec-rimmsqol.map -Language English -Filter 09-rimmsqol-restart-write.feature -Then @('10-rimmsqol-restart-read-revealed.feature', '11-rimmsqol-restart-read-hidden.feature')
```

An unfiltered run of this pass is not valid: 10 and 11 would run in the writer's process.

**5. Without Anomaly** — `wsl-deps.sans-anomaly.map` (`!ludeon.rimworld.anomaly`). The default pass
stages Anomaly, so feature 01's guard scenario reads the Inhumanized entry listed there and absent here,
with no unresolved reference and no error in either: together they are S14's first half. The fixture is a
played colony that may carry Anomaly content; errors of its own on load are read before anything is
blamed on this mod.

```powershell
powershell.exe -ExecutionPolicy Bypass -File scripts/Run-PickleWsl.ps1 -Mod ManyHappyReturns -DepMap wsl-deps.sans-anomaly.map -Language English
```

No `incompatibleWith` is declared, so there is no incompatibility pass to write.

## Building the steps

```powershell
dotnet build Tests/Pickle/Source/ManyHappyReturns.PickleSteps.csproj -c Release
```

The output lands in `Mod/Pickle/Assemblies/`, which is what the staging copies. Built and
verified offline on 2026-09-23: 0 warnings, 0 errors, against the real `Assembly-CSharp.dll` and
`RimWorks.Pickle.Ref`. Step DLLs are loaded at game start, so a rebuild needs a new run: a report
produced without one did not test the fix.

## Checking the steps before a run

```powershell
powershell.exe -ExecutionPolicy Bypass -File Tests/Pickle/Check-Steps.ps1
```

Run offline on 2026-09-23: 59 patterns declared, all 59 compile, compared against 746 other
expressions (205 from Pickle itself, 541 from 29 other step sources in the collection) — none
ambiguous, and all 267 step lines across the fourteen features resolve to a declared expression. Every
step text carries "Many Happy Returns" or names the neighbour it drives.

## Reading a report

`exitReason` first, before any number. A run killed in flight leaves a report that looks like a
result. Then the count of scenarios played against the count discovered: this suite has no
`@requires:` gating and no `@wip` scenarios, so a clean run should play all of it in both passes.
