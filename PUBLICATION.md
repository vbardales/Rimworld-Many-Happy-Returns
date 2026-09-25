# Publishing Many Happy Returns

What the Workshop page asks for and the repository holds nowhere else. Written before the first
upload, and kept for whoever picks this mod up later.

**The first item exists, created private on 2026-09-23 as 0.1.0 (id 3806762201).** The messages below are not posted: an item has to be public first. The mod is at `done` in the workflow: the in-game gate of
`done -> tested` has not been passed, so the first upload is not due yet. This file exists so
that when it is, none of it is discovered at the form.

## The one-way parts

Three things do not get a second chance, and two of them are silent when they go wrong.

- **The description.** `Verse.Steam.Workshop.SetWorkshopItemDataFrom` calls `SetItemDescription`
  only when `creating` is true. Every later update leaves the page's text alone, so a correction
  after the first upload is made by hand on Steam and never from `About.xml`. Read it once more
  in the file before clicking — it already carries the Workshop-link convention on every citation
  of Gifts and Birthdays, Birthday Variety and Wing's Meaningful Parties, and ends with the source
  link.
- **The packageId.** `nelim.manyhappyreturns`. It is written into every subscriber's
  `ModsConfig.xml` and into other mods' `loadAfter`. Changing it after publication disables the
  mod for everyone.
- **`About/PublishedFileId.txt`.** Steam writes it into the mod folder at creation.
  **Commit it immediately.** Lost, the next upload creates a second item, and the first one stays
  up with nobody able to update it.

And one that surprises people: **Steam creates every item private.** RimWorld never calls
`SetItemVisibility`, so the item has to be switched to public by hand once it has been checked.

## Screenshots, in upload order

Steam shows the first one large. That slot goes to the most demonstrative image, not the
prettiest. **The five exist**, in `Art/Workshop/` (`01-` to `05-`, in upload order, nothing else in that
folder), cropped from the captures of the showcase pass (`Tests/Pickle/Mod/Pickle/Features/15-publication-shots.feature`,
run `docs/runs/2026-09-25-gallery-en.md`). Each was opened by the session and validated one at a time by the owner on
2026-09-25. Two differ from the table below: the settings image is cropped above the empty half of the window, and the
wish image shows the log line, not the speech bubble (the pawns are stacked and small in the full frame). They are not
the Preview — that is the header image, already made (`Mod/About/Preview.png`, 896x504). The upload is by hand.

| # | What it must show | Why this slot |
| --- | --- | --- |
| 1 | The morning letter, open, naming the pawn and their age | The whole pitch in one image: the game noticing on its own what it otherwise ignores |
| 2 | Two colonists exchanging the birthday-wish speech bubble, with the social log line visible | The interaction that carries most of the mod's day-to-day presence |
| 3 | The end-of-day memory in a pawn's Needs/Mood tab, one of the graded stages, with its description readable | Where the verdict actually lives, and proof it is graded rather than binary |
| 4 | The settings page, with the mood-scale slider mid-drag and its live percentage label | Answers "can I tune this" before anyone asks |
| 5 | A forgotten-birthday memory, to show the mod also plays the sad half honestly | Sets expectations: this is not only a feel-good mod |

Rules for every one of them: no developer tools, no debug overlay, no other mod's overlay, no
Pickle launcher panel in a corner. A window with nothing in it sells nothing. **Each image has to
be opened and looked at before it is uploaded.** A capture scenario passing green says the trip
happened, not that the picture shows anything.

## Dependencies and DLC to declare

Read from the sources on 2026-09-23, not from intent.

**No hard dependency, and no DLC, not even optionally.** `supportedVersions` is `1.6`. No Harmony:
the mod patches nothing. There is no `LoadFolders.xml`, and the only DLC-conditional content is
two `MayRequire="Ludeon.RimWorld.Anomaly"` entries in `Mod/Defs/Birthday.xml`'s
`nullifyingTraits`/`nullifyingHediffs`, guarding a field the game itself no-ops when the DLC is
absent — not worth declaring as a dependency.

**Three optional neighbours, declared as `loadAfter` (Gifts and Birthdays only) or nothing at
all, and named in the description as "works with".**

| Mod | packageId | Workshop id | What this mod does with it |
| --- | --- | --- | --- |
| Gifts and Birthdays | `KrukuCoB.rout` | 3625791734 | Looks up its `BirthdayCongratulationReceived` thought def by name; its congratulations count toward this mod's own verdict, and its party is recognised as a party. No reference, no patch; behaves identically absent. |
| Birthday Variety | `Axolki.BirthdayVariety` | 3770972092 | Nothing: this mod reads `Pawn_AgeTracker.BirthDayOfYear`, whatever set it. |
| Wing's Meaningful Parties | `winggar.meaningfulparties` | 3504909699 | Nothing: it retextures the vanilla party event rather than replacing it, so a party it causes still grants vanilla `AttendedParty`. |

None of the three belongs in the required items list: a hard dependency forces a download on
someone who does not want it, and all three are genuinely optional here.

## Adult content boxes

**No to all of them.** Both images were opened and looked at on 2026-09-13:

- `Mod/About/Preview.png` — an overhead dining-room scene, warm lamp light over a table set for a
  small gathering, cool ambient elsewhere, small rear-facing colonists with no readable faces.
  Title, tagline and a `1.6` corner badge.
- `Mod/About/ModIcon.png` — a stylised mascot head with a ribbon, gift box and confetti (the
  original art style, kept by explicit preference over the general no-text guideline; see
  `STATUS.md`, "Icon preference").

Nothing sexual, nothing graphic, no nudity, no gore. The mod adds no art beyond the icon and
preview, no text beyond its own interface strings, and touches no body, health or combat system.

## Messages to post, one per mod

Written for the three mods this one reaches into or names. **Posted after the item is public** —
a link to a private item opens for nobody. One per recipient and personalised. BBCode works in
Steam comments, and pasting a bare Workshop URL makes a thumbnail, so the link to this mod goes
on its own line. Steam's comment limit is 1000 characters; each of these is well under it.

### Gifts and Birthdays (`KrukuCoB.rout`, id 3625791734)

> Hi! I've just published a small companion mod called Many Happy Returns: colonists notice their
> own birthday and remember how the day went, all day, wherever two of them cross paths — no
> party required. I built it to stay entirely out of your way: it never patches or reads your
> assembly, only looks up your BirthdayCongratulationReceived thought by name, so a congratulation
> handed out at your party counts toward my mod's verdict too, and your party is recognised as a
> party. If your mod is absent, mine behaves exactly the same. Thank you for Gifts and Birthdays —
> it's the mod that made a colonist's birthday feel like an occasion in the first place, and I
> wanted the quiet, everyday half of it to exist alongside your party.
>
> https://steamcommunity.com/sharedfiles/filedetails/?id=PUBLISHED_FILE_ID

### Birthday Variety (`Axolki.BirthdayVariety`, id 3770972092)

> Hi! I've published Many Happy Returns, a small mod about colonists noticing their own birthday
> and each other's. It needed nothing special to work with Birthday Variety — it just reads
> whatever birth date a pawn actually has — but I wanted to say thank you anyway: randomising
> birthdays is exactly the kind of small, honest utility that makes a mod like mine worth writing
> in the first place. Without it every starting colonist shares the same day, and the whole point
> of my mod is that the day should feel like theirs.
>
> https://steamcommunity.com/sharedfiles/filedetails/?id=PUBLISHED_FILE_ID

### Wing's Meaningful Parties (`winggar.meaningfulparties`, id 3504909699)

> Hi! I've published Many Happy Returns, a mod about the quiet, everyday half of a colonist's
> birthday — the letter, the wishes, the memory of the day. I read through your source before
> saying this works together, and it's a genuinely nice piece of compatibility: you never replace
> the vanilla party, only give it a reason, so a birthday party your mod causes is still a real
> party as far as my mod (and the base game) is concerned. I love the idea of a party finally
> explaining itself — thank you for it.
>
> https://steamcommunity.com/sharedfiles/filedetails/?id=PUBLISHED_FILE_ID

Claude (Anthropic) is credited in the description under `AI-GENERATED`, not by comment.

## Steam release notes

Written at the moment of upload, in a tab nothing prompts until the form is open â€” the easiest
thing to forget. Unlike the description they can be corrected afterwards and start again at every
update. With the release workflow of `.github/workflows/release.yml` in documented mode, the change note
is the fenced block under the `### 1.0.0` heading below, sent to Steam as written (BBCode, 8000 bytes at most,
plain text here), and the GitHub release notes are the `## [1.0.0]` section of `CHANGELOG.md`.

### 1.0.0

```
First release. RimWorld 1.6.

Colonists notice their own birthday, and whether anyone said anything about it. A letter in the morning, a birthday wish that colonists give each other on their own, and a single graded memory formed at the end of the day, from "someone thought of me" to "an unforgettable birthday", built from who wished them well, whether someone close was among them, a party, the meal, and the room it was eaten in. And if nobody said a word all day, that is noticed too, hard to earn and easy to avoid.

Works with Gifts and Birthdays, Birthday Variety and Wing's Meaningful Parties. None required. Mod settings for the letter, the forgotten-birthday memory, and a mood scale, under Mod options. English and French. No Harmony.
```
## Right after the upload, in this order

1. Commit and push `Mod/About/PublishedFileId.txt`. Before anything else.
2. Subscribe to your own item and load it, as a subscriber sees it.
3. Switch the item to public by hand.
4. Post the three messages above, with the real item id in the links.
5. Write the Workshop id into `STATUS.md` under `workshop:`.
