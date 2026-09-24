# Minimal pass, English, 2026-09-24 (replay after the review fixes)

- **Command**: `Submit-PickleRun.ps1 -Mod ManyHappyReturns -Language English -EvidenceDir ManyHappyReturns/Tests/Pickle/Evidence/2026-09-24-minimale-en` (request `20260924-163959-668-3b9e`, filed through the TicketDispatcher after two earlier tickets died: 29716 was abandoned by the launcher at 90 minutes, 47900 died with its session)
- **Set name**: `sans-facultatifs`
- **Suite state**: commit `7a45161`, fourteen feature files
- **exitReason**: `failed`. Pickle exit 1: the run went to the end
- **Scenarios**: 39 discovered, 39 accounted for: 25 passed, 4 failed, 10 skipped, 0 flaky
- **Evidence**: on disk, ignored by git (`Tests/Pickle/Evidence/2026-09-24-minimale-en/`): `junit.xml`, `summary.json`, `summary.md`, `Player.log`, the `@review` capture and the captures of the three failures whose cause is still open. Deleted
  once summarised: `report.html`, `messages.ndjson` (40 MB), the psychopath failure capture (its cause is written above) and the whole
  2026-09-23 English run, which this one supersedes scenario for scenario. The 2026-09-23 French run stays: it is the only French proof.

## What this run settles

The three fixes of the first run hold: the tenure step (every scenario that expects the forgotten memory now gets it: unwished,
midnight, two witnesses, and the exemptions that used to pass vacuously: a one-day tenure, a Downed celebrant unwished, a caravan, a single other
colonist), `allowDowned` for the newborn (feature 14 passes), and the year turn (the wish exchange that the French pass
refused passes here). Ten skips are the expected ones (RIMMSQOL, Gifts and Birthdays).

## The four failures

| Scenario | What the run showed | Verdict |
| --- | --- | --- |
| a psychopath is exempt from the forgotten memory on an unwished birthday | The celebrant holds a `Nelim_BirthdayForgotten` memory. | The suite's assumption was wrong. Decompiled: `MemoryThoughtHandler.TryGainMemory` calls `ThoughtUtility.CanGetThought` without `checkIfNullified`, so the game stores the memory; `Thought.MoodOffset` then returns 0 for a nullified thought. The exemption is real, and it is a zero mood offset, not an absent memory. The first run passed this scenario only because the young fixture formed no memory at all. Now asserted on the nullified state and the zero offset. |
| a psychopath still receives a positive verdict on a wished birthday | `TryInteractWith` refused the wish. | Cause not shown (see below). |
| a Downed celebrant is not judged, wished | Same refusal. | Same. |
| a save and reload during the birthday repeats no letter and keeps the verdict | Same refusal. | Same. |

The wish is refused in three of the nine scenarios that exchange one, and it was refused in one French scenario the day before;
in the other six it passes with the same steps. That is not deterministic. The message only said "CanInteractNowWith or a
cooldown", so it did not tell which. My working hypothesis is the initiator's 120-tick cooldown after an ambient chat during
the fixture's settling time. It is a hypothesis: the failure message now names each condition (position, awake, downed,
mental state, both directions), and the step clears the cooldown before the exchange. The replay decides.

## Fixes made after the run

`WishSteps.Exchange` (cooldown cleared, diagnostic message), a new step `the celebrant's birthday-forgotten memory, if any, has no effect on mood`
for the psychopath scenario, and the comments in the mod source, feature 06, `VerdictSteps.cs` and the README that claimed the engine refuses a
nullified memory. No behaviour of the mod changed; the source change is a comment. Not replayed.

## Player.log

Read for errors. Two lines outside the failures, both from the test companion and present in the 2026-09-23 log too: `Mod Many Happy
Returns - Pickle tests did not load any content` (it carries only steps and features) and the missing `downloadUrl` for its dependency. No
error from the mod itself, and no unresolved reference. The other errors are the four failures.

## Not read

The four failure captures and the `@review` capture of feature 03 were kept and not opened beyond one (the Downed-wished
capture: three colonists standing outside at 9h, nothing that names the refusal).
