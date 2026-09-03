# Changelog

## 1.0.0 — 2026-09-03

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
