# Backlog

Ideas and follow-ups that are not part of the current release. Nothing here is promised; an item leaves this list
when it is done, dropped with its reason, or turned into a scenario in `TEST_SCENARIOS.md`.

## Enrich the morning letter

Asked by the owner on 2026-09-25, while judging the first Workshop image.

Today the letter is one template (`ManyHappyReturns.BirthdayLetterText`, English and French) and only the name, the
age and the pronouns change (`GameComponent_Birthdays.cs`, the letter is sent once per birthday and per colonist).
Every player reads the same two sentences for every colonist. To enrich it:

- decide what varies: more than one wording, or facts from the colony that the mod already reads (who is present,
  whether a close relation is around, how long the colonist has lived in the colony);
- keep every wording translatable (Keyed keys, English and French, parameters checked) and keep the letter short: it
  is a nudge, not a report;
- a new variant is a change of player-facing text: it resets `localization`, `translation_en` and `translation_fr`
  in `STATUS.md` until re-audited, and it changes the first Workshop image (`Art/Workshop/01-...`).

Not started: the owner gave the list of things that could vary (below); which wordings, and how many, is not decided.

What the owner listed as things that could change the wording (2026-09-25), kept as she gave them:

- the colonist's **mood**, the **season**;
- **grief** (a recent death in the colony, or a bereaved colonist), **sick or dying**, **pregnant**;
- **at war** (a raid or a fight in progress), **in a caravan**;
- who is around: the **best friend**, the **parents**, the **spouse**, and so on.

Notes from reading the code, not decisions: a colonist away in a caravan is not spawned, so the letter is not sent for
them today (`GameComponent_Birthdays.cs` returns early); "in a caravan" would first need a decision on whether a
caravan birthday should send anything at all. The mod already reads close relations for the verdict
(`BirthdayUtility.CloseRelations`), which is a natural source for the parents, spouse and best friend variants.

## Weigh the verdict by the gifts received

Asked by the owner on 2026-09-25. Today the day's score counts the wishers, a close relation among them, and the day
quality (party, meal, room) in `BirthdayUtility.DayQualityBonus`; it reads no gift. The idea: what the colonist
received that day weighs in the score. Open: what counts as a gift (an item handed over, or only what Gifts and
Birthdays wraps), and whether the mod reads Gifts and Birthdays' presents by name, as it already does for its
congratulation (no reference to its assembly, absent means unchanged).

## A "Dudley" mode: the verdict by traits and by the recreation obtained that day

Asked by the owner on 2026-09-25, kept as she wrote it: "mode Dudley en fonction des traits de caractère / du loisir
obtenu ce jour-là". Read as an optional mode in which the same day is judged differently depending on the colonist's
traits and on the recreation (joy) they got that day. **The reading of "Dudley" is the session's guess, to confirm with
her** (a colonist who judges a birthday by what they received, like the character), as is whether it is a setting,
a separate mode or a replacement of the current scoring. If it becomes a setting, it is a new player-facing text: it
resets `settings_audit`, `localization`, `translation_en` and `translation_fr` until re-audited.
