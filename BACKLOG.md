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
