# Many Happy Returns

*Did anyone remember?*

RimWorld 1.6. Colonists notice their own birthday, and they notice whether anyone else did.

The base game knows every pawn's birth date and does almost nothing with it: no letter, no
reaction, only the occasional age-related injury. This mod adds the missing half — the felt
experience of the day, not the logistics of a party.

## What it does

**A letter in the morning.** Today is someone's birthday. That is the only thing the mod asks
of you.

**Colonists wish each other happy birthday.** An ordinary social interaction: it fires by
itself when two colonists meet, once per pair per birthday, and leaves both thinking a little
better of each other.

**A verdict at the end of the day.** One memory, five grades, from *someone thought of me* to
*an unforgettable birthday*. What moves it:

| Signal | Points |
|---|---|
| Colonists who wished them well | 1 → **1** · 2 → **2** · 3–4 → **3** · 5+ → **4** |
| A spouse, partner, parent, child, sibling or close friend among them | **+2** |
| A party that day | **+2** |
| A fine meal **+1**, a lavish meal | **+2** |
| Eaten in an impressive dining room | **+1** |

| Score | Grade | Mood, 5 days |
|---|---|---|
| 1 | someone thought of me | +2 |
| 2–3 | quiet birthday | +4 |
| 4–5 | nice birthday | +6 |
| 6–8 | lovely birthday | +8 |
| 9+ | unforgettable birthday | +10 |

**And when nobody said a word.** A separate memory, −6 for five days. It is deliberately hard
to earn: the pawn must have lived in the colony for at least ten days, must have had at least
two other colonists around who could have remembered, and must have been awake and present.
A recruit from yesterday expects nothing from anyone; a colonist who spent the day unconscious
in the hospital is not being snubbed. Psychopaths never form it.

## Settings

Three: the morning letter on or off, the forgotten-birthday memory on or off, and a mood scale
from 50 % to 200 %. The scale is written into each memory as it forms, so moving the slider
never rewrites memories a pawn already holds.

## Compatibility

**Gifts and Birthdays** (`KrukuCoB.rout`) owns the party, the guests and the presents; this mod
does none of that on purpose. Its congratulations only fire during its party, the wishes here
happen all day wherever two colonists cross paths, and the two tallies are added together, so
congratulations given at its party count towards the verdict here. The wishes here carry no
mood of their own — only opinion — so nothing is ever counted twice. The mod is found by def
name, not by packageId, and everything works identically when it is absent.

**Birthday Variety** randomises birth dates. Nothing to do.

## How it works

No Harmony. Nothing is patched.

- The day is detected in a `GameComponent`, by comparing
  `GenDate.DayOfYear(TicksAbs, 0f)` with `Pawn_AgeTracker.BirthDayOfYear`. Deliberately *not*
  the biological birthday: `Pawn_AgeTracker` fires that one when the biological year rolls
  over, which drifts from the calendar as soon as the aging rate is not exactly one.
- The wishes are drawn by the game's own interaction roulette: an `InteractionWorker` that
  returns a weight only on the right day, for a pair that has not exchanged one yet.
- Everything counted is read back from the pawn's own memories — this mod's wishes, Gifts and
  Birthdays' congratulations, and the vanilla party / meal / dining-room thoughts.

## Building

```bash
dotnet build ManyHappyReturns/Source
```

The textures are regenerated from SVG with `_tools/build.sh` (Chrome headless as rasteriser).

## Testing it

Dev mode, `Debug actions → Many Happy Returns`:

- **Birthday is today** (click a pawn) — moves their birth date onto today, keeping their
  chronological age whole. The morning letter fires at once.
- **End today's birthdays now** — runs the midnight verdict immediately.
- **Log birthday tally** (click a pawn) — prints the score breakdown to the log.

Do **not** use the game's own *Increment time*: it calls `DebugSetTicksGame`, which jumps the
tick counter instead of playing the ticks. No colonist gets the chance to say anything, so a
jumped-over birthday yields a forgotten one, or nothing at all.

## Licence

MIT. See `LICENSE` and `ATTRIBUTION.md`.
