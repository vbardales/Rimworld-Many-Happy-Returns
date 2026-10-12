---
localization: complete
translation_en: complete
translation_fr: partial
settings_audit: complete
mod:          Many Happy Returns
packageId:    nelim.manyhappyreturns
repo:         Rimworld-Many-Happy-Returns
visibility:   public
detached:     yes
workflow_stage: writeTests[1.0.0]
licence:      original
licence_at:   written from scratch, MIT; only the general idea is shared
upstream_mod_remotes: N/A
dependencies: none
showcase:     complete
tested_on:    2026-10-02 audit: gate 9 (done -> tested) is NOT met under the AUDIT.md of that day (the RIMMSQOL, Gifts and
              Birthdays and without-Anomaly conditional scenarios never ran; S15 interface review not cleared), so
              stage is back to done. The 1.0.0 was nevertheless sent privately on 2026-09-29 under fail fast; that is an
              act, not a state. See "Audit — 2026-10-02". Replaced claim, kept as written:
              2026-09-29, c74f940: gate 9 met — gallery images opened and validated (2026-09-25), S16 green
              on the saved colony (EN/FR) and on a new colony (feature 17, 2026-09-28), no @wip, logs
              checked FR/EN, no manual test left uncleared. RIMMSQOL, Gifts and Birthdays and without-Anomaly
              conditional passes not yet run: deferred to post-publication non-regression under fail fast
              (AUDIT.md), not a blocker for gate 9 under that policy.
              Gate 10 (tested -> prepublished) met on 2026-09-29: working tree clean (a stale, uncommitted
              .github regeneration from an older template than what's on main was reverted with
              `git checkout -- .github`, per PUBLISHING.md's shared-index rule), DLL matches sources, the
              description was reviewed and its current shape kept (no literal AI-GENERATED line, owner's
              instruction), PUBLICATION.md complete, screenshots opened and their order justified (2026-09-25),
              thank-you messages drafted, dependencies/DLC settled, Steam release notes written, adult content
              boxes answered with both images opened (2026-09-13).
workshop:     3806762201
remaining:
  - unverified: French review by Virginie. The 2026-09-30 gender-agreement rule reset
    `translation_fr` to `unchecked`; the session's own read found no French text in this mod
    that agrees with a pawn's gender (no adjective or participle describes the pawn itself — see
    "Translation audit" below), and generated FRENCH_REVIEW.md by script. Virginie then read it
    and returned eight corrections (mistranslations, imprecise wording, calques), applied the same
    day (see "Translation audit"); FRENCH_REVIEW.md regenerated on the fixed revision.
    `translation_fr` stays `partial`: no session marks its own French reviewed, so her sign-off on
    this revision is still outstanding.
  - unverified: run the Pickle suite (Tests/Pickle/, fourteen features), every pass named in
    TESTING.md (minimal, with Gifts and Birthdays, with RIMMSQOL, without Anomaly, French), and review its @review
    captures. Written and offline-verified on 2026-09-23 (builds 0 warnings/0 errors against the real
    game assembly; Check-Steps.ps1, rerun 2026-09-24: 60 patterns compiling, none ambiguous against 840 other
    expressions, all 274 step lines resolved). Minimal English pass replayed on 2026-09-24 (docs/runs/, exitReason
    failed, ran to the end): 25 passed, 4 failed, 10 skipped. The first run's fixes hold (tenure, newborn, year turn).
    The four failures are the suite's, none a defect of the mod: a psychopath keeps the forgotten memory with a zero
    mood offset (the game stores nullified memories; the assertion was wrong, now rewritten) and three refused wish
    exchanges of unproven cause (diagnostic and cooldown reset added). Replayed on 2026-09-25 as two targeted
    tickets, both exitReason passed: the four scenarios plus one control, 5 of 5 in English, and the year turn in French
    (docs/runs/). The wish refusal was intermittent, so one green sample does not prove its cause. The French pass
    of 2026-09-23 (24 passed, 5 failed, 10 skipped) remains the proof of the other French scenarios and predates the fixes.
    Not run: the RIMMSQOL, Gifts and Birthdays and Anomaly-less passes, and the full English and French passes that
    `tested` needs on the final revision.
  - unverified: publication of the 1.0.0 by CI. Decided by Virginie on 2026-09-25: semantic-release
    (`bootstrap-release.sh`), knowing that the page description is overwritten from `Mod/README.template.md`, that no
    commit is pinned (the publish takes the head of main when she approves) and that `dispatch-publish.sh` does not
    apply to it; rollback target: the item switched back to private by her (the repository has no tag and the 0.1.0
    only created the item). Before any publish, indispensable and not done: the Workshop gallery (five images in
    `Art/Workshop/`, cropped from the run of 2026-09-25, judged by the session and validated one at a time by the owner:
    images 1 (the whole letter window), 2 (the wish in the Log tab) 3 (the day's memory in the Needs tab) 4 (the settings, cropped above the empty half) and 5 (the forgotten birthday) validated 2026-09-25, so the five images are validated by the owner; the two memory scenarios are
    replayed by request 6e70, not read yet) and the manual tests (S15 interface and log review in English and French, S16 regressions
    on a new colony and on an existing save, the `@review` captures opened). Environments release-dry-run and
    steam-production (reviewer vbardales) and secrets STEAM_USERNAME, STEAM_CONFIG_VDF_B64 are present on the
    repository since 2026-09-23 (bulk scripts of Rimworld-Release-Admin, checked read-only by the CI/CD session on
    2026-09-25); approval of steam-production is Virginie's. The workflow is in place (CI/CD session, commits 7c3cd0c and
    82b9bb8 on main, `.github/workflows/release.yml`, inputs ref, version, mode): publish needs the full 40-character SHA
    and refuses if main has moved since, so `dispatch-publish.sh vbardales/Rimworld-Many-Happy-Returns release.yml <SHA> 1.0.0`
    works with it. Two dry-runs, nothing published (runs 36129326221 and 36129589535): both end red by design, no
    `feat:`/`fix:` commit yet. The runner's DLL differs from the tracked one (runner c99622ed..., tracked and local
    build 96e46073...); the workflow puts the tracked files back, so the committed, tested DLL is what ships and it must
    stay committed. The description is 3693 bytes; the straight quotes reach Steam as typographic ones. To go: gallery and
    manual tests done, then the last commit, tell the CI/CD session its SHA for the dry-run, no push to main after it
    (documented mode, CI/CD commit 5c77b94, no `feat:` commit needed). Done on 2026-09-25 (commit ce53f13): the
    `## [1.0.0] - 2026-09-25` section of CHANGELOG.md (re-date it if the publication slips) and the `### 1.0.0` change
    note of PUBLICATION.md. Replay 6e70 read on 2026-09-25 (docs/runs/2026-09-25-gallery-replay-en.md): 2 of 2 green, so the showcase pass has no red
    scenario left. The THANKS (Pickle, RimLogging, PickleTools, RIMMSQOL, agreed by the owner) are in the description, its
    Markdown source and the README. Still to do: the shared thank-you register (owner said wait), and the
    owner's own validations. Not done: no tag. `publish-tag.yml` is now the only workflow (semantic-release
    removed in 6abe743 after a green dry-run proved the page would not change). Dry-run of dd8bb1f (before that
    removal) was green (runs 36549813159, 36549969053, description 4175 bytes, sha256 ab5a3d81..., same
    renderer/source as the semantic-release path); a further commit changes the SHA and needs its own dry-run.
    **The SHA to publish is now 6abe7432a4009fd0ce1d8649f5086b078c29bcf9, version 1.0.0, dry-run green (run
    36550364220, 2026-09-29): same description, 4175 bytes, sha256 ab5a3d81...; live page unreadable while the
    item stays private, comparison skipped by design, not a defect.** No commit since; this SHA is ready for
    `dispatch-publish.sh`. **Published 2026-09-29**, approved by Virginie: run 36560128134, `uploaded to Workshop
    item 3806762201`, Steam upload confirmed ("Committing update... Success."), tag `v1.0.0` and the GitHub
    release created on 6abe7432a4009fd0ce1d8649f5086b078c29bcf9 (`gh release view` confirms `targetCommitish`).
    Still to verify: the public Steam page itself (gallery, visibility switch — the item stays
    private until Virginie switches it, and no `SetItemVisibility` call ever runs), and `PublishedFileId.txt`
    already committed from the earlier 0.1.0 prepublication, so nothing new to commit there.

    **Post-publish description and art fixes (2026-09-29), owner's own edits and instructions.** The description
    published on 2026-09-29 was missing the literal section headers `AUDIT.md` asks for: fixed in `Mod/README.template.md`
    (`## IF I GO QUIET`, `## AI-GENERATED`, `## THANKS`, in that order, then the ATTRIBUTION/licence line, matching
    DrumBathHygiene and JoyRescue's own descriptions), reversing an earlier "keep the current format" call. `AI-GENERATED`
    now also names DALL-E (Preview and ModIcon), confirmed directly by the owner; not found named anywhere in the commit
    tree, which only ever recorded the art's result, never the tool. The owner edited the live Steam description by hand
    to match; the source is kept in step for the next `update_description` publish, not re-sent automatically.
    The Preview gained the cut-out mascot icon (`scripts/cutout-icon.cjs`, flood-filled from the border, the outline
    untouched), 240x240, bottom-left (the scene's darkest corner), tilted +15deg, bleeding off the left and bottom
    edges — validated by the owner 2026-09-29. `Art/Workshop/01-preview.png` kept as its byte-identical copy (the new
    shared rule, `PUBLISHING.md`: every gallery opens on the Preview; captures renumbered `02-` to `06-`, two more
    wanted to reach eight). Fixed in passing: `scripts/build-art.cjs` still copied the retired `Art/ModIcon-selected.png`
    (removed 083f4f9); it now downscales `ModIcon-text-free-source.png` with sharp. S16 is now fully green: the saved colony in English and
    French (docs/runs/2026-09-25-s16-en.md, -s16-fr.md), and the new-colony half (feature 17) on 2026-09-28
    (docs/runs/2026-09-28-s16-new-colony-5-en.md), after four red attempts of 2026-09-26/27 — the first three for
    set-up reasons in the PickleTools NewColony steps (colonists still in their pods behind the Crashlanded intro
    dialog, a Dialog_NodeTree the accept step did not handle), fixed on their side; the fourth (df29) found a real
    defect of the mod: `BirthdayUtility.IsCloseTo` used `DirectRelationExists` on the implied relations Child and
    Sibling (a game warning, and children and siblings never counted as close), fixed in 93bb86b with
    `Pawn.GetRelations`. Not reproducible; not replayed again. The ModIcon was
    downscaled from `Art/ModIcon-text-free-source.png` on 2026-09-28 at the owner's instruction (083f4f9), with `Art/ModIcon.ico`.
    The CI/CD session pushed the manual publish workflow (1702347) and About.xml's description became plain text, by design;
    the semantic-release files are still there, migration undecided by the owner.
    `Mod/README.template.md` and `Mod/.steamignore` are committed.
  - not applicable, by rule: the engine's own refusal of a memory for an inhumanized pawn (S14, second
    half). The mod declares `nullifyingHediffs`; feature 01 proves the declaration follows the DLC in
    the default and the Anomaly-less passes; the enforcement is the game's, which AUDIT.md leaves to it.
  - unverified: execute TEST_SCENARIOS.md in game, including EN/FR interface and logs
  - unverified: native settings opening and persistence across full game restart and save loading
  - unverified: RIMMSQOL reveal/open/hide persistence and optional dependency combinations, recording exact versions
  - unverified: new colony and existing save regressions
  - unverified: Wing's Meaningful Parties (`winggar`, Workshop id 3504909699), now named in the
    description, README and ATTRIBUTION.md at the user's request. Read-only reasoning from its
    source, not tested in game: it retextures the vanilla party event without replacing its
    mechanism (GatheringWorker_SendLetter.cs patches the letter text; PartyCauseDef carries no
    separate lord job), so a party it causes should still grant vanilla AttendedParty and count
    toward DayQualityBonus like any other party. Not confirmed live.
session:      2026-09-23, first Workshop item created private by the owner (0.1.0)
updated:      2026-09-23, workshop id 3806762201 recorded; stage left at done pending the in-game gate
protocols_read_sha: 7e2994b43fb5b05d704694cbedc2e08ce4ea08e2
---

# Many Happy Returns — status

## Audit — 2026-10-02

Revision audited: `3dc4e3a` (main, working tree clean at the start). Previous `stage`: `prepublished`; retained:
`done` (`workflow_stage: done`). No game launched, nothing published, no Pickle request filed.

- **Why it falls back.** Gate 9 (`done -> tested`) in the AUDIT.md of 2026-10-02 requires that every conditional
  scenario ran. Written and never played: features 08 to 11 (`@requires:MalteSchulze.RIMMSqol`), feature 12
  (`@requires:KrukuCoB.rout`), and the Anomaly-less pass of feature 01 (S14, first half). The earlier line "deferred
  to post-publication non-regression" no longer holds for them: a scenario that never ran is a new scenario, which
  plays first. S15 (interface and log review, EN and FR) is also still listed as manual and uncleared. `@wip`: none
  (grep over `Tests/Pickle`). Gates up to `preTest -> done` are untouched: this audit re-verified nothing in them and
  keeps the 2026-09-23 / 2026-09-30 results.
- **What stays true.** `1.0.0` was sent to Steam on 2026-09-29 (item 3806762201, private, `v1.0.0` released on
  `6abe743`) under the fail fast policy. That is an act; the state is `done`. `CHANGELOG.md` already opens with `1.0.0`
  above `0.1.0` (the file ID, `Mod/About/PublishedFileId.txt`, is committed).
- **Repository checks.** `.dds`: none tracked (`git ls-files`), `*.dds` in `.gitignore`. Evidence: none tracked.
  Upstream: original mod, no source repository (`upstream_mod_remotes: N/A`).
- **Evidence trimmed (list before delete).** Deleted, superseded: `2026-09-25-s16-en` (red, replayed green as
  `2026-09-25-s16-rejeu-en`), `2026-09-26-s16-nouvelle-colonie-en`, `-2-en`, `2026-09-27-s16-nouvelle-colonie-3-en`, `-4-en`
  (red attempts, replaced by `2026-09-28-s16-nouvelle-colonie-5-en`); the `screenshots/` of `2026-09-24-minimale-en`
  and `2026-09-23-minimale-fr` (captures read on the day, not cited). Kept (19 MB -> 2.2 MB): the two full minimal
  passes (sole proof of the scenarios not replayed), the targeted EN and FR replays, both gallery runs, S16 EN replay
  and FR, new colony 5. Nothing in this file points to a deleted report. Which proofs to keep: `TESTING.md`,
  "Evidence to keep".
- **Work needed for `tested`.** Play the RIMMSQOL (08, then 09 with 10 and 11), Gifts and Birthdays (12) and Anomaly-less
  passes; clear S15 or list it not applicable; the final full EN and FR passes (non-regression, last, on the final
  revision). The French sign-off by Virginie (`translation_fr: partial`) is a separate open item.

## Translation audit — 2026-09-30

French lives in four files: `Mod/Languages/French/Keyed/ManyHappyReturns.xml` (16 keys: letter,
settings, debug actions), `Mod/Languages/French/DefInjected/ThoughtDef/Birthday.xml` (labels and
descriptions of the five `Nelim_BirthdayRemembered` grades, the one `Nelim_BirthdayForgotten`
grade, and the two wish thoughts), `.../DefInjected/InteractionDef/Birthday.xml` (the wish's label
and its five `logRulesInitiator` grammar-rule strings), `.../DefInjected/MainButtonDef/Settings.xml`
(the settings shortcut's label and description). Read in full, every file, on 2026-09-30, against
the current revision.

**French gender-agreement rule (TRANSLATIONS.md, 2026-09-30): no text in this mod needs the switch.**
None of the four files contains an adjective or past participle that agrees with a pawn's own
gender. The birthday letter's `{PAWN_pronoun}` uses the game's own pronoun grammar resolver, not a
hand-written switch. The `Nelim_BirthdayRemembered`/`Forgotten` thought descriptions are written in
the first person about the day itself ("C'était mon anniversaire...", "Discret, mais bien.") —
"Discret" agrees with the masculine noun "anniversaire", not with the pawn. The InteractionDef's
rule strings all use the compound past with "avoir" and no reflexive pronoun, so the participle
never agrees with the subject (documented in the file's own comment, checked here against the game's
grammar rules, not merely repeated). This is a read of every line, not a pattern search: no session
can certify it grep-clean, and this one did not try to.

`FRENCH_REVIEW.md` generated by `scripts/Make-FrenchReview.ps1` (shared) (script, not by hand; original =
English throughout, the mod is written from scratch, not ported — stated once at the top of that
file rather than per row).

**Virginie's review (2026-09-30), eight corrections, all applied the same day:**

| Key | Was | Now | Why |
|---|---|---|---|
| InteractionDef `rulesStrings[1]` | "a rappelé à [RECIPIENT] que c'était son anniversaire" | "s'est souvenu que c'était l'anniversaire de [RECIPIENT]" | "remembered it was [RECIPIENT]'s birthday" is not "reminded [RECIPIENT]" |
| InteractionDef `rulesStrings[3]` | "encore beaucoup d'années comme celle-ci" | "encore de nombreux heureux anniversaires" | poor rendering of "many happy returns" |
| Keyed `MorningLetterTip` | "a récolté une séquelle liée à l'âge" | "a subi une blessure liée à l'âge" | clumsy, imprecise |
| Keyed `ForgottenThoughtTip` | "debout et présent" | "éveillé et présent" | "awake" mistranslated |
| Keyed `MoodFactorTip` | "Multiplie l'humeur des souvenirs" | "Multiplie l'effet d'humeur des souvenirs" | it is the memories' mood effect that scales, not "mood" itself |
| Keyed `MoodFactorTip` | "souvenirs déjà en cours" | "souvenirs déjà acquis" | wrong word for a memory already held |
| ThoughtDef `lovely_birthday.description` | "une vraie journée" | "une journée digne de ce nom" | calque of "a real day" |
| ThoughtDef `unforgettable_birthday.description` | "Et ici, en plus" | "Et ici, de tous les endroits" | weakens "Out here, of all places" |

The session's own flag on `quiet_birthday.description` ("Discret, mais bien." — terser than the
other four grades) was confirmed as a real weakness: now "C'était discret, mais agréable."

FRENCH_REVIEW.md regenerated on the fixed revision; no row flagged.

**Not applied, the reviewer's own note, left to Virginie:** the settings text uses generic
masculine ("un colon", "il", "autres colons"). Not a gender-switch omission — these are plain
nouns/pronouns, not adjectives or participles agreeing with a specific pawn's gender — but a
rewording toward inclusive phrasing is possible if she wants that tone applied generally. Not done
here without her decision.

**Re-checked against the updated TRANSLATIONS.md (2026-09-30, o-series neutral grammar and
"Player choice for gendered French").** Both `not_applicable` for this mod: no French text here
agrees with a pawn's own gender (established above, and confirmed by Virginie's review turning up
no gender-switch defect), so there is no `{PAWN_gender ? ...}` text whose neutral segment would
need the o-series morphology, and section "Player choice for gendered French" itself says not to
create a settings page for a mod with no such text — none is added.

`translation_fr` stays `partial`: only Virginie's own reading of `FRENCH_REVIEW.md` can set it to
`complete`, per TRANSLATIONS.md's "Systematic French review" section.


## Audit — 2026-10-12 (up to `playTests`)

Revision `96b249a`. Chain re-read against AUDIT.md, gates 1 to 7. `workflow_stage` set to `writeTests[1.0.0]`: gate 7.g
(leaving `writeTests`) is not met, so `playTests` is not established.

- **Met, re-run today.** 7.b automated tests `Tests/Run.ps1`: 15 passed, 0 failed; 7.c XML tests green; tested DLL equals
  `Mod/Assemblies/ManyHappyReturns.dll` (hashes in `Tests/Results/manifest.json`). `Check-ModBasics.ps1` PASS,
  `Check-DefInjected.ps1` 18 keys, 0 errors, `Check-Status.ps1` 0 error.
- **Not met: 6.g / 7.g.** `translation_fr: partial`: Virginie's sign-off of `FRENCH_REVIEW.md` on this revision is outstanding.
- **defect (6.c).** `Check-Translations.ps1` FAILS: `ManyHappyReturns.BirthdayLetterText` uses `{PAWN_pronoun}` once in English
  and twice in French ("... se souviendra de la journée qu'{PAWN_pronoun} aura passée"). Not changed: the French text was
  read by Virginie, and a text change forces a new review (8.b). Also a WARN: French path `InteractionDef/Nelim_BirthdayWish.logRulesInitiator.rulesStrings`
  outside the text inventory.
- **Pending housekeeping.** `.build/` to `build/` (rename after the four queued Pickle tickets return);
  `Tests/Check-Xml.ps1` generic sections to replace by the shared `Check-ModBasics.ps1` / `Check-Translations.ps1` (LOCAL_SCRIPTS.md);
  `scripts/cutout-icon.cjs` obsolete (needs `modIconSource`, owner's art work); `Mod/About/About.xml` em dashes (WARN).
- **Pickle (early, not a gate).** Four tickets filed 2026-10-12: RIMMSQOL 08, RIMMSQOL 09-11, Gifts and Birthdays 12+01, without-Anomaly 01.

Older journal (Pickle suite 2026-09-23, audits 2026-09-13 and 2026-09-23, delivery, corrections, historical sweep notes): `docs/runs/STATUS-journal-2026-09.md`.
