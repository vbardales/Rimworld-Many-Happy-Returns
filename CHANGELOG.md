# Changelog

## [1.0.0] - 2026-09-25

First public release. RimWorld 1.6.

- A letter on the morning of a colonist's birthday. The game sends none for an ordinary birthday:
  `LetterBirthdayBiological` only fires when age-related injuries were gained.
- A birthday wish, a social interaction that fires on its own on the day, once per pair. Opinion only, no mood, so
  it never doubles Gifts and Birthdays' congratulations.
- A single graded memory at the end of the day, its stage chosen from the day's score: wishes received, whether
  someone close was among them, a party, the meal, the room it was eaten in.
- A forgotten-birthday memory when nobody said a word, guarded by ten days in the colony, two colonists who could
  have remembered, and the pawn being awake and present.
- Mod settings under Mod options: morning letter, forgotten-birthday memory, mood scale 50-200 %, with an
  explanation of their scope and when they apply; an invalid stored mood factor is recovered.
- An optional main-bar settings shortcut, hidden by default, that RIMMSQOL and compatible tools can reveal; it opens
  the same dialog.
- English and French, defs, interface and developer actions.
- Developer actions under "Many Happy Returns": move a birthday onto today, close the books without waiting for
  midnight, log the day's tally.
- Works with Gifts and Birthdays, Birthday Variety and Wing's Meaningful Parties. None required. No Harmony.

## [0.1.0] - 2026-09-23

- Create the `PublishedFileId.txt` (first Workshop item, id 3806762201, created private).

## Before the first Workshop item

Kept as written. Its "1.0.0" was the repository's own first release; it was never tagged and never uploaded to Steam.

### 1.0.0 — 2026-09-03

First release. RimWorld 1.6.

- A letter on the morning of a colonist's birthday. The game sends none for an ordinary
  birthday: `LetterBirthdayBiological` only fires when age-related injuries were gained.
- `Nelim_BirthdayWish`, a social interaction that fires on its own on the day, once per pair.
  Opinion only, no mood, so it never doubles Gifts and Birthdays' congratulations.
- `Nelim_BirthdayRemembered`, a single graded memory whose stage is chosen at midnight from
  the day's score: wishes received, whether someone close was among them, a party, the meal,
  the room it was eaten in.
- `Nelim_BirthdayForgotten` when nobody said a word, guarded by ten days in the colony, two
  colonists who could have remembered, and the pawn being awake and present.
- Mod settings: morning letter, forgotten-birthday memory, mood scale 50–200 %.
- English and French, defs and interface.
- Dev-mode test actions under "Many Happy Returns": move a birthday onto today, close the
  books without waiting for midnight, log the day's tally.
- No Harmony.
