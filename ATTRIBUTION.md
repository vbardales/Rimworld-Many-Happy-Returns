# Attribution

Everything in this mod is written from scratch: defs, C#, textures, both language packs.
No third-party file, def or line of code is reused, adapted or ported.

## About "Garam, Happy Birthday"

The idea of treating a birthday as something a colonist *feels* rather than something the
colony *organises* was suggested by **Garam, Happy Birthday** by Dalrae
(https://steamcommunity.com/sharedfiles/filedetails/?id=1995828620), RimWorld 1.0 only.

That mod cannot be ported, forked or reused in any form. Its `About.xml` carries an explicit
prohibition: all rights are reserved to its author, and it may not be distributed by anyone
else. **Nothing from it is present here** — not a def, not a texture, not a string, not a
value. The mod was opened once, long enough to establish that it was locked, and never
again. Only the general idea is shared, and ideas are not what a licence covers.

The two mods do not do the same thing either. Garam graded the birthday across a family of
separate thought defs; this one uses a single graded def whose stage is chosen at runtime from
what actually happened during the day, adds a social interaction that fires on its own, and
announces the day to the player.

## Neighbouring mods

- **Gifts and Birthdays** by KrukuCoB (`KrukuCoB.rout`), 1.6, alive and maintained. It owns the
  logistics: the party, the guests, the wrapped presents. This mod deliberately does none of
  that, and reads nothing from its assembly. The one point of contact is a lookup of its
  `BirthdayCongratulationReceived` thought def by name, so that congratulations given at its
  party count towards the verdict here instead of being invisible. No reference, no patch, and
  the mod behaves identically when it is absent.
- **Birthday Variety** by Axolki (`Axolki.BirthdayVariety`), 1.6, randomises birth dates. Nothing
  to do: this mod reads `Pawn_AgeTracker.BirthDayOfYear`, whatever set it.
- **Wing's Meaningful Parties** by winggar, 1.5/1.6, gives the vanilla party event a reason,
  birthdays included, without changing how or how often it fires: its own source patches only the
  letter text (`GatheringWorker_SendLetter.cs`), not the party mechanism. No reference, no patch;
  a party it causes still grants vanilla `AttendedParty` and counts in `DayQualityBonus` the same
  as any other party.

## Dependencies

RimWorld 1.6 and nothing else. No Harmony: the mod patches nothing.

**Licence:** MIT (`LICENSE`).
