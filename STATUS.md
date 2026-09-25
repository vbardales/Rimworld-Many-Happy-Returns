---
localization: complete
translation_en: complete
translation_fr: complete
settings_audit: complete
mod:          Many Happy Returns
packageId:    nelim.manyhappyreturns
repo:         Rimworld-Many-Happy-Returns
visibility:   public
detached:     yes
stage:        done
licence:      original
licence_at:   written from scratch, MIT; only the general idea is shared
dependencies: none
showcase:     complete
tested_on:
workshop:     3806762201
remaining:
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
    images 1 (the whole letter window), 2 (the wish in the Log tab) 3 (the day's memory in the Needs tab) and 4 (the settings, cropped above the empty half) validated 2026-09-25; image 5 to present; the two memory scenarios are
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
    manual tests done, then the final commit (documented mode, CI/CD commit 5c77b94, no `feat:` commit needed) adds
    `## [1.0.0] - <date>` to CHANGELOG.md and, in PUBLICATION.md, a `### 1.0.0` heading followed by a fenced block that is
    the Steam change note (BBCode, 8000 bytes at most); push, tell the CI/CD session the SHA for the dry-run, no push to
    main after it. Not done: no tag, no green dry-run. New colony (S16): a session cannot start one, Nelim's Pickle Tools
    was asked on 2026-09-25 for a step that starts a new colony in a headless pass; no answer yet.
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
---

# Many Happy Returns — status

## The Pickle suite — 2026-09-23

Requested after the audit below identified its absence as the sole blocker to `done`. Six feature
files under `Tests/Pickle/`, scope and design in [Tests/Pickle/README.md](Tests/Pickle/README.md),
the pass matrix in [TESTING.md](TESTING.md). Written and checked offline only, **on explicit
instruction not to take the Pickle run lock**: no game was launched, no WSL run attempted.

What was actually run, both read-only or local-compile, no ticket involved:
- `dotnet build Tests/Pickle/Source/ManyHappyReturns.PickleSteps.csproj -c Release` against the
  real installed `Assembly-CSharp.dll` and `RimWorks.Pickle.Ref`: 0 warnings, 0 errors.
- `Tests/Pickle/Check-Steps.ps1` against the installed Pickle's own assemblies and every other
  suite in the collection: 35 patterns declared, all 35 compile, none ambiguous against 732 other
  expressions (205 from Pickle itself, 527 from 29 other step sources), all 92 step lines across
  the six features resolve to a declared expression.

Neither check starts a game or requires the machine-wide Pickle lock; both are the same kind of
offline verification the authoring guide asks for before ever queuing a run.

Design choices worth recording: scenarios name **the celebrant** and **the wisher** rather than a
fixture pawn name, resolved fresh each call by persistent in-game name rather than a held `Pawn`
reference, so the same steps survive a mid-scenario save/reload without a separate reacquisition
step. The duplicate-protection scenario reads `InteractionWorker_BirthdayWish.RandomSelectionWeight`
directly rather than exchanging the wish twice, because `Pawn_InteractionsTracker.TryInteractWith`
does not consult that weight at all — exchanging it twice would prove nothing about the guard the
mod actually relies on. Two mutations needed for the forgotten-birthday exemptions (a forced short
tenure, the Psychopath trait) are reverted in `[AfterScenario]`, following the collection's
`SettingsSandbox` pattern for the settings file itself.

The optional pass with Gifts and Birthdays is stageable: its Workshop id (3625791734, `KrukuCoB.rout`)
was confirmed later the same day from the downloaded item's own `About.xml`, and
`wsl-deps.avec-gifts-and-birthdays.map` carries it.

This restores `stage: done`: every criterion of `preTest -> done` the prior audit checked still
holds (re-verified nothing changed under it), and the missing Pickle criterion it found is now
met — written, with a justified account of what stays manual and why. `done -> tested` remains
exactly as unverified as before; writing a suite is not running one.

## Audit — 2026-09-23

Applied the current `AUDIT.md` workflow: files, artifacts and re-run results, not the status
this document already declared. Audited revision: `933fe40a19d5eeb6c35358263fed121b475a2113`
(HEAD of `origin/main`, working tree clean before the audit; the only writes made here are
this section, the front matter and the refreshed `Tests/Results/*`). Read `PUBLISHING.md`,
`STYLE_RIMWORLD.md`, `MOD_SETTINGS.md` and `TRANSLATIONS.md` in full. RimWorld was not
launched, on Windows or in the WSL; all checks below are headless (compilation, a managed-DLL
reference, static analysis) or direct file inspection.

### What changed since the last recorded evidence

The 2026-09-13 test manifest was pinned to commit `68154e0`. HEAD had since moved one commit
further, `933fe40` ("write the maintainer name as Nelim"), which only recases `nelim` to
`Nelim` in `About.xml`'s `<author>`, both `LICENSE` copies and `README.md` — no source, Def,
language resource or image changed. Not a pertinent modification to behavior, settings or
translation evidence; re-run anyway rather than trusting a five-day-old manifest, since the
point of this audit is not to take the declared state on faith.

### Gates re-verified, with fresh commands

| Gate | Result |
| --- | --- |
| horsMonoRepo | **Validated.** Own `.git`, `origin` at `https://github.com/vbardales/Rimworld-Many-Happy-Returns.git`, working tree clean, HEAD tracks `origin/main`. `packageId`/folder/repo name coherent; no literal-identity requirement applies to an original title. `LICENSE`, `ATTRIBUTION.md`, `README.md`, `CHANGELOG.md` present and in English. `diff` confirms `Mod/ATTRIBUTION.md` and `Mod/LICENSE` are byte-identical to their root copies. |
| ModIcon generated | **Validated.** `Mod/About/ModIcon.png` is 128x128, 23,544 bytes (within the 20-30 KB guidance), directly viewed. It carries a text ribbon ("MANY HAPPY RETURNS"), against the general no-text guideline in `STYLE_RIMWORLD.md` — but this is an explicit, already-recorded user preference (see "Icon preference — 2026-09-13" below) that the workflow itself says takes precedence and that no audit may override or regenerate. Not re-opened. |
| Preview generated | **Validated.** `Mod/About/Preview.png` is 896x504, 693,827 bytes, under both the 900 KB guidance and the 1 MB hard limit. Directly viewed: high oblique camera, tiled floor, warm lamp pool against a cool ambient, faceless silhouettes, readable title/tagline/1.6 badge, no clipping or overlap with the scene. |
| preOptions | **Validated.** `<description>` ends, after the credits, with `[url=https://github.com/vbardales/Rimworld-Many-Happy-Returns]Source code on GitHub[/url]`, matching `<url>` and the `origin` remote. No prefix/suffix applies to this original title. **Recommendation, not a defect:** PUBLISHING.md's 2026-09-22 addition asks every named mod with its own Workshop page to carry `[url=...]name[/url]` on each citation; "Gifts and Birthdays" and "Birthday Variety" are currently named without it. The item is not yet on Steam (no `PublishedFileId.txt` anywhere in the tree), so `SetItemDescription`'s one-shot rule has not fired yet and this is still free to fix. Not applied here: a Steam search surfaced a "Gifts and Birthdays" listing at `id=3625791734`, but two live listings share that title and the fetch needed to confirm KrukuCoB's own item hit Steam's rate limit twice: recorded as unverified rather than guessed into the file. |
| options | **Validated**, re-run. `Source/ManyHappyReturnsMod.cs` and `ManyHappyReturnsSettings.cs` implement the primary `Mod options -> Many Happy Returns` route: three useful controls (letter toggle, forgotten-thought toggle, 50-200% mood slider), scope/timing text, reset, NaN/Infinity/out-of-range recovery. `Mod/Defs/MainButton.xml` declares `Nelim_ManyHappyReturnsSettings` with `buttonVisible=false`, `workerClass=ManyHappyReturns.MainButtonWorker_Settings`, which opens the same `Dialog_ModSettings` over the same loaded `Mod` instance — a hidden-by-default shortcut, not a forced-invisible one. |
| l10n | **Validated.** `Source/DebugActions_Birthday.cs` now routes every displayed label, tool cursor name and confirmation through `.Translate()` (`ManyHappyReturns.Debug.*` keys); the one remaining hardcoded string is the technical `Log.Message` tally, correctly left in English. `ran ../scripts/Check-DefInjected.ps1 -TransMod <Mod>`: 29 patch operations, 11,592 defs indexed, 18 keys checked, 0 errors. `ran ../scripts/Check-XmlFields.ps1 -ModPath <Mod> -ExtraAssemblies <dll>`: 3 files checked, no unknown field. |
| preTest | **Validated.** `About.xml` declares no `modDependencies`; `loadAfter` names only `Ludeon.RimWorld` and the optional `KrukuCoB.rout`. `BirthdayUtility`'s lookup of `GetNamedSilentFail("BirthdayCongratulationReceived")` is null-safe and unconditional in code, matching the optional declaration. The two `MayRequire="Ludeon.RimWorld.Anomaly"` entries in `Mod/Defs/Birthday.xml` guard the only DLC-specific content (`Inhumanized`). No Harmony, no `LoadFolders.xml` (single-version mod). `ran ../scripts/Check-DefRefs.ps1 -ModPath <Mod>`: 6 mod defs, well-formed XML, every def reference resolved to the right type, every `ParentName` resolved. |
| **preTest -> done** | **Defect, blocking.** Re-ran the automated suite fresh (`powershell -File Tests/Run.ps1`, no RimWorld process running before or during): 15/15 behavior cases pass, 8 XML documents parse, 16 Keyed keys and 18 DefInjected paths check out, and the tested DLL's SHA-256 matches the distributed `Mod/Assemblies/ManyHappyReturns.dll` — this satisfies "tests automatisés écrits, exécutés et au vert" and "tests XML écrits, exécutés et au vert." `TEST_SCENARIOS.md` satisfies "scénarios de tests fonctionnels écrits." But **no Pickle (Gherkin) suite exists**: no `Tests/Pickle/`, no `*.feature` file, no `TESTING.md`, no mention of Pickle anywhere in the repository (`grep -ril pickle .` finds nothing). AUDIT.md requires these *written*, not executed, at this gate, scoped to "ce que seul un jeu qui tourne peut montrer" — and this mod has exactly that kind of surface: the letter actually rendering, the hidden-by-default MainButton, a RIMMSQOL click reaching through another mod's window (the literal example in AUDIT.md's own Pickle guidance), the translated settings dialog layout, and memory state that only proves itself across a save/reload. `TEST_SCENARIOS.md` was written as the human-run substitute for exactly this surface, which is evidence the surface was recognized, not that the Pickle requirement is inapplicable to it. No `not_applicable` justification for skipping Pickle is recorded anywhere. This is a real gap, not an unperformed convenience check. |
| done -> tested | **Not established** (gate above not reached). Historically unverified regardless: no scenario in `TEST_SCENARIOS.md` has been run in a live game, `tested_on` is empty, and no Pickle suite exists to run. |

### Last cumulatively justified state: **preTest**

Every gate through `l10n -> preTest` is independently validated above, including a fresh
re-run of all automated evidence at current HEAD. The single blocker to `done` is the missing
Pickle scenario suite required by `preTest -> done`; nothing else at that gate is in question.
This is a step back from the `done` this file declared after the 2026-09-13 corrections — that
audit's own gate table did not check for a Pickle artifact at this transition, and none was
ever written. Preserving that section below as history rather than rewriting it: it was correct
about everything it checked.

### What closes `done`

Write a `Tests/Pickle/` Gherkin suite (and the `TESTING.md` describing its passes, per
AUDIT.md's Pickle section) scoped to what a running game alone can show for this mod: the
morning letter rendering and its translated text, the settings dialog opening through both the
primary route and the revealed MainButtons shortcut, a RIMMSQOL-revealed shortcut actually
being clickable through its window, and a memory surviving a save/reload. Everything else this
mod does — scoring, clamping, Scribe round-trips, interaction weights — is already proven
headless and does not belong in Gherkin per AUDIT.md's own "on ne teste pas le jeu" and
unit-test-priority rules. This does not require running the suite to reach `done`; only writing
it and stating its scope.

This audit does not write that suite: `AUDIT.md` is explicit that an audit does not complete
development to improve its own verdict, and Pickle suite design belongs to the session holding
the mod, per [[rimworld-tests-hors-jeu]] and [[rimworld-status-md-a-maintenir]].

## Delivery — 2026-09-13

The user requested committing and pushing the completed corrections to the standalone
origin/main. This supersedes the earlier uncommitted-for-review note. The original icon
is retained. Code and DLL hashes still match the recorded successful tests; the documented
icon-only change does not invalidate them. Stage remains done, with in-game checks pending.

## Icon preference — 2026-09-13

The user preferred the original ModIcon style. Restored the exact original PNG from
standalone HEAD, including ribbon, gift and confetti, and preserved it as
`Art/ModIcon-selected.png`. The artwork build now copies that selection unchanged.
This explicit user preference takes precedence over the general no-text icon guideline;
it does not downgrade `done`. The text-free alternative remains available in Art but unused.
The Preview and all code/localization changes remain unchanged. The earlier test manifest
describes the pre-restoration icon; its code/DLL evidence is unaffected. Icon format and
XML checks were repeated after restoration; no new in-game test is claimed.

## Corrections and revalidation — 2026-09-13

The user authorized fixes after the audit. **dansMonoRepo -> done**; `done` is the literal
workflow state meaning ready for final functional validation in game, not already `tested`.
The audit below is preserved as historical evidence and is superseded by this section and
the current front matter. The user's explicit clarification governs the options gate:
source/definition analysis plus applicable automated checks, with interactive checks at tested.

### Repository

Autonomous root: `C:/Users/nelim/Documents/rimworld/ManyHappyReturns`, with its own `.git`.
Existing public origin: `https://github.com/vbardales/Rimworld-Many-Happy-Returns.git`.
Fetched main and attached it to the existing pushed revision
`68154e0474d7a456eb5b3dd1ff7d0f81a1e860d2`; local main tracks origin/main.
No published history was rewritten, and no new remote repository was created.
Local files were preserved when populating the autonomous index from the remote tree.
The README difference, artwork relocation from `_tools/art/` to `Art/`, and audited STATUS
were already local relative to the older remote tree; they were retained.

Only ManyHappyReturns entries were removed from the parent index with `git rm --cached`;
the folder was ignored there and the obsolete parent remote removed. Other staged/unstaged
mod work was preserved. Those parent index removals remain staged for a scoped future commit.
The autonomous fixes remain uncommitted for review; no new commit, push or Workshop upload
was performed. This does not negate the verified existing first pushed commit.
Build intermediates now stay inside this repository's ignored `.build`, outside `Mod/`.

Public/original/MIT and the existing attribution decision remain unchanged. LICENSE and
ATTRIBUTION copies in the distribution are identical. The stable name/packageId are unchanged.

### Gates and evidence

| Gate | Current result |
| --- | --- |
| horsMonoRepo | Validated: autonomous metadata, existing GitHub origin/pushed history, initialized English documents and coherent public/original/MIT decision. |
| ModIcon generated | Validated: corrected text-free icon installed at 128 x 128; code corrections complete and distributed DLL rebuilt successfully. |
| Preview generated | Validated: installed PNG 896 x 504, 693,827 bytes; directly inspected at native size and 268 pixels. |
| preOptions | Validated: English description ends with the exact source link, matching `<url>` and origin; title has no applicable prefix/suffix/linking-word treatment; 1.6 badge and distinct cool accent over warm material palette. |
| options | Validated under the user's technical gate: useful primary settings, optional native hidden shortcut, source/definition review and applicable passing automated tests. Runtime GUI and customization checks remain tracked for tested. |
| l10n | Validated after settings checks: 16 Keyed keys EN/FR, 18 French injections and native English Def values; debug actions and confirmations localized; no owned displayed English sentence remains hardcoded. |
| preTest | Validated: Core-only mandatory runtime dependency; optional Gifts and Birthdays lookup/loadAfter, guarded Anomaly references, no external framework or Harmony. Def-reference validation passes. |
| done | Validated: written functional scenarios, 15 passing automated cases, passing XML/localization/schema/reference checks, and tested DLL identical to the delivered DLL. |
| tested | Not verified: TEST_SCENARIOS.md has not been executed in a running game; no in-game logs or translated runtime UI review claimed. |

Results and scope: [Tests/RESULTS.md](Tests/RESULTS.md), command output in
`Tests/Results/`, timestamp and exact working-file hashes in
`Tests/Results/manifest.json`. Base revision is the fetched commit above plus local changes;
the manifest binds the evidence to those actual files instead of implying HEAD alone was tested.
Commands: `Tests/Run.ps1`, plus parent `Check-DefInjected.ps1`, `Check-XmlFields.ps1`
with the delivered DLL, and `Check-DefRefs.ps1`. Build: SDK 8.0.424, pinned reference
Krafs.Rimworld.Ref 1.6.4871, zero warnings/errors. No game DLL is redistributed.

### Settings audit — complete technical gate

Existing on/on/100% defaults and global scope are retained. The native primary page now
states application timing: letter switch at first detection, memory settings at day closeout,
existing memories unchanged. Reset, slider limits (50–200%, 5% steps) and missing-value
defaults remain. Non-finite stored factors now recover to 100%; out-of-range values are
clamped after loading and before rendering.

`MainButtonWorker_Settings` uses the game's `Dialog_ModSettings` and the same loaded mod
instance as the primary route. `Nelim_ManyHappyReturnsSettings.buttonVisible=false` keeps
the shortcut neither visible nor greyed out by default. The worker does not override Visible
or reset visibility, so tools may reveal it through the native field. Installed 1.6
MainButtonDef, MainButtonWorker, Dialog_ModSettings and debug-node APIs were inspected directly.
No customization mod is mandatory; **no RIMMSQOL or other customization version was tested**.

The 15 passing cases exercise defaults/reset, bounds, NaN/infinity, actual Scribe value
callbacks, missing/old settings, scoring thresholds, null interaction inputs, native worker
inheritance and actual memory factor snapshots. Boolean behavior is traced in TrySendLetter
and TryGiveForgotten; complete letter/memory simulation is an in-game scenario.
The first Scribe harness used full FinalizeLoading and encountered Unity-only native calls;
Tests/RESULTS.md preserves that outcome and explains the callback-scoped correction.
No headless serialization result is presented as full game/save-load validation.

### Translation audit — complete resource gate

Native DebugActionYielder nodes provide translated action and tool names, avoiding constant
attribute labels without Harmony. Their allowed game state remains PlayingOnMap. Both
displayed debug confirmations use parameterized EN/FR keys; technical tally logs stay English.
Settings scope text and shortcut description are covered. The French tooltip now uses colon.
The proper name Many Happy Returns stays identical in both languages.

Checks: 8 XML documents parse; 16 used keys resolve in both languages with matching parameter
sets, no duplicates/empty entries; 18 French DefInjected paths pass the installed-game checker
(11,592 defs indexed, zero errors, no unverified target). XML field and Def reference checks
pass. UI wrapping, game grammar resolution and layout in both languages remain pending S15/S16.

### Visual revalidation

Built-in imagegen edited the icon; original art was preserved. Exact prompt and artifact
provenance: [Art/README.md](Art/README.md). Selected edit is
`Art/ModIcon-text-free-source.png`; installed icon is 15,099 bytes, readable at 32 pixels.
Preview uses the preserved source scene with reproducible native composition in
`Art/Preview.html` and a single palette in `Art/preview-palette.json`, rendered by
`_tools/build-art.cjs`. Segoe UI is available. Warm materials guide veil/secondary ink;
the accent amplifies the cool floor family. No secondary tag was invented for this title.
Minimum measured contrasts: title 7.55:1, summary 8.46:1, badge 10.62:1.
Both final PNGs and thumbnails were directly viewed: no text on the icon, no clipped title
or badge, no overlap with the scene subject, and no concrete camera concern.

### Remaining transition

Run and record the scenarios in [TEST_SCENARIOS.md](TEST_SCENARIOS.md), including full
settings persistence, native/customized access, dependency cases, EN/FR interface/logs and
new/existing saves. Fix any observed failures and rerun their affected regressions before
marking `tested`. These are missing runtime verifications, not known outstanding defects.

## Audit — 2026-09-13

The user's supplied workflow is authoritative, including its clarification that the
options gate does not require in-game testing. `stage: dansMonoRepo` uses the literal
workflow name, not the legacy `port`/`showcase` codes. Previous stage: empty;
previous showcase: `complete`; previous localization fields: `unchecked`.

Scope: `C:/Users/nelim/Documents/rimworld/ManyHappyReturns`, distributed content `Mod/`.
`git rev-parse --show-toplevel` returns `C:/Users/nelim/Documents/rimworld`;
there is no local `.git` at the mod root, and `git ls-files -- .` lists the mod in
the parent repository. No autonomous checkout was identified for this folder.
Audited monorepo HEAD: `75c3000e1833d325cc7626a402981e4aa881d47a`.
Last commit affecting this mod: `bc303d9ced55f8e2282615ac0632889f861f6e09`.
At audit start, the only local change within this mod was the addition of the three
unchecked localization fields to STATUS.md. Those fields were retained and updated.
Other mods' staged/unstaged work was excluded and left untouched.
This audit changes only STATUS.md and creates ignored build output in `.build/audit/`.
No source, distributed binary, image, Git history or remote was modified or published.

Protocols read: parent PUBLISHING.md, STYLE_RIMWORLD.md, MOD_SETTINGS.md and
TRANSLATIONS.md, plus AGENTS.md and the full attached user request.

### Ordered transition results

| Transition | Result and evidence |
| --- | --- |
| dansMonoRepo -> horsMonoRepo | **Defect:** the audited folder is still part of the monorepo, not an autonomous repository. **Validated independently:** GitHub repository exists and is PUBLIC; remote `many-happy-returns` points to it; remote HEAD is `68154e0474d7a456eb5b3dd1ff7d0f81a1e860d2` on main. Its location in the monorepo does not substitute for an autonomous remote. STATUS, English README, ATTRIBUTION, MIT LICENSE and CHANGELOG exist. LICENSE and ATTRIBUTION copies in Mod are byte-identical. Title, packageId, folder and repository names are coherent without literal identity. |
| horsMonoRepo -> ModIcon generated | **Validated independently:** build passes and delivered DLL is current (identical SHA-256). PNG is 128 x 128, 23,544 bytes. **Defect:** icon contains a text ribbon, contrary to STYLE_RIMWORLD's no-text rule. **Not established:** development completion, given the missing required settings shortcut. No generation history was required. |
| ModIcon generated -> Preview generated | **Validated independently:** directly inspected installed PNG, 896 x 504, 577,333 bytes, below both 900 KB guidance and the mandatory 1 MB limit. High oblique view, floor grid, small figures without detailed faces, readable title and coherent warm/cool composition; no concrete camera concern. |
| Preview generated -> preOptions | English description and exact title are present. No prefix, suffix, status tag or conjunction treatment is applicable to this original title. **Defect:** description ends with the adoption clause, not `[url=https://github.com/vbardales/Rimworld-Many-Happy-Returns]Source code on GitHub[/url]`; the existing raw URL and absent `<url>` do not satisfy PUBLISHING.md. **Defect:** no 1.6 version badge in the inspected Preview. Amber rule and neutral main ink are distinct; no secondary title/tag ink is displayed, so separation from a secondary ink is not applicable to this composition. |
| preOptions -> options | **Partial / defect:** three useful settings and primary settings implementation exist, but no MainButtonDef or shortcut implementation exists in Source/ or Mod/. Applicable automated behavior/persistence checks have no test suite or execution evidence. In-game checks are deferred to tested under the user's clarification, not used as an options blocker. |
| options -> l10n | **Partial / defect:** ordinary letters, settings, thoughts and interaction grammar have EN/FR resources; XML, Keyed and DefInjected checks pass below. Debug UI labels and two displayed messages remain hardcoded. Gate cannot be finalized while settings is partial. |
| l10n -> preTest | **Validated independently at source level:** RimWorld 1.6 is the only mandatory runtime dependency. No Harmony or third-party assembly reference. Optional Gifts and Birthdays lookup is null-safe and loadAfter names KrukuCoB.rout. Anomaly's Inhumanized references have MayRequire guards. No LoadFolders or patch files. Optional integration runtime behavior remains unverified. |
| preTest -> done | **Not established:** README provides dev helper instructions, not a complete set of scenarios with preconditions/actions/expected results. No Tests directory or test project is present. Audit XML checks passed, but no automated functional suite was run; compilation does not replace it. |
| done -> tested | **Unverified:** no functional in-game execution, log review, translated UI inspection, fresh game/existing save coverage or customization integration test was performed or evidenced. `tested_on` remains empty. |

The last cumulatively justified state is **dansMonoRepo**. Later independent passes
above do not advance the global state past the first failed transition.

### Repository and rights evidence

Commands: `git rev-parse --show-toplevel`, `git rev-parse HEAD`,
`git status --short -- .`, `git diff -- STATUS.md`, `git ls-files -- .`,
`gh repo view vbardales/Rimworld-Many-Happy-Returns --json nameWithOwner,visibility,url,defaultBranchRef`,
and `git ls-remote many-happy-returns HEAD`.
The first sandboxed GitHub attempts failed on configuration/network access; an authorized
read-only retry succeeded. This is not a remote defect or outstanding access limitation.

Public/original/MIT classification is retained on the documented from-scratch provenance:
ATTRIBUTION distinguishes the general inspiration from Garam's prohibited content and
expressly records no reused files, code, strings or values. This audit found no contrary
evidence; it does not claim an independent historical authorship investigation or permission
to reuse that third-party content. The older vocabulary below is a preserved historical note;
the user-supplied workflow and this explicit provenance decision govern this audit.

### Build and artifact checks

Ran an isolated build without replacing Mod/Assemblies:

```powershell
dotnet build Source/ManyHappyReturns.csproj -p:BaseIntermediateOutputPath=C:/Users/nelim/Documents/rimworld/ManyHappyReturns/.build/audit/obj/ -p:OutputPath=C:/Users/nelim/Documents/rimworld/ManyHappyReturns/.build/audit/bin/ -v:minimal
```

.NET SDK 8.0.424, restored Krafs.Rimworld.Ref 1.6.4871; exit 0,
0 warnings, 0 errors. Initial sandbox build hit MSB4184 on access to Microsoft SDKs;
authorized retry passed. Both compiled and delivered ManyHappyReturns.dll have SHA-256
`D5BACBF5D833414AA854131A1E265BD945DE2435D9E2F1C35CA4383B1283ED60`.
Mod contains only its own assembly; sources/intermediates are outside the distributed folder.
Source/Directory.Build.props currently points normal intermediates to the parent `.build`;
the audit overrides kept all new output inside this mod.

Both installed PNGs were opened and inspected directly, and System.Drawing confirmed their
PNG encoding and dimensions. Original images remain in Art/*-source.png. No historical
generation report or recorded comparison with a game screenshot was required.
Optional maintenance note: Art has no preview-palette.json or current HTML composition;
the old 640-square SVG is not the delivered 896 x 504 Preview. This is not evidence that
the inspected image was never generated, and no regeneration was attempted.

### Settings audit

Source review: ManyHappyReturnsMod.cs, ManyHappyReturnsSettings.cs and GameComponent_Birthdays.cs.
Global ModSettings defaults: morningLetter=true, forgottenThought=true, moodFactor=1.
Reset restores those values; Scribe persists them with matching missing-value defaults.
Slider bounds are 0.5 to 2, step 0.05; runtime memory scaling clamps to those bounds.
Morning letter controls letter creation; forgottenThought controls the negative memory;
the mood factor is copied into new memories and does not rewrite existing ones.
SettingsCategory/DoSettingsWindowContents implement the primary Mod options route.
These are static findings, not observations of running settings or serialization.

Search over all Source/ and Mod/ found no MainButton/MainTab implementation or definition.
The hidden, discoverable shortcut is therefore an actual missing feature required by the
workflow, not merely an unperformed integration test. No RIMMSQOL or other customization
mod version was tested. Applicable automated defaults, reset, bounds, effects and persistence
tests remain unverified; none were invented merely to fill this audit's checklist.

### Translation and XML audit

Parsed all 6 distributed XML files successfully. Compared 10 English and 10 French Keyed
entries: no missing/empty/duplicate entry and no difference in parameter token sets.
Source uses all 10 keys for the 2 birthday letter strings and 8 settings strings.
PAWN/AGE named arguments and the positional mood percentage argument match those resources.
English Def values provide native coverage; no redundant English DefInjected file is needed.
French contains the interaction label and five grammar alternatives, plus all eight thought
stage labels and six descriptions. Grammar references use INITIATOR/RECIPIENT consistently.

Ran `../scripts/Check-DefInjected.ps1 -TransMod <absolute Mod path>` against installed
game data: 29 patch operations applied, 11,591 defs indexed, **16 keys checked, 0 errors**,
exit 0, no UNVERIFIED findings. This validates the injected paths, not in-game rendering.

Defect: Source/DebugActions_Birthday.cs contains the untranslated visible action labels
`Birthday is today`, `End today's birthdays now`, `Log birthday tally`, and both
Messages.Message strings (birthday moved / birthday(s) closed out). Dev-only visibility
does not exempt displayed text under TRANSLATIONS.md. The technical Log.Message tally is
correctly English and excluded; the proper mod name is not a missing translation.
English wording exists, but language fields remain partial because the full UI inventory
is not localized and the prerequisite settings gate has not passed.
Optional wording recommendation: replace `pawn` with `colon` in the French morning-letter
tooltip. This recommendation is separate from the hardcoded UI defect.

### Next transition only

Detach this mod into an autonomous Git repository with its GitHub remote configured and
verify a pushed commit there, preserving current local work and the documented public/MIT
decision. The GitHub repository and pushed commit already exist; creating another remote
repository is unnecessary. No build, image generation or publication is needed merely to
establish horsMonoRepo. Later defects/checks above remain tracked independently.

## Historical sweep notes — preserved

Read by a sweep across every mod, rather than by asking each thread in turn. It lives at the
root, never inside `Mod/`, so Steam never receives it.

The fields above were read off the disk on 2026-09-12. Four cannot be, and wait for whoever
holds this mod:

- **`stage`** — one of `port`, `showcase`, `preTest`, `done`, `tested`, `published`. Filled in
  from the session group where one exists; confirm it.
- **`tested_on`** — the date of the last run in game. Empty means never.
- **`dependencies`** — `declared` when every mod this one needs is named in the About's
  `modDependencies`, `to check` when a non-vanilla `loadAfter` suggests a dependency that is not
  declared, `none` when the mod needs nothing. An undeclared dependency is not cosmetic: on
  2026-09-11 Reequilibrage animaux took 47 vanilla animals down with it, Muffalo included, because
  the class it injects belongs to a mod that was not declared and not loaded.
- **`remaining`** — what is left, in three kinds: `feature` for something missing from a first
  release, `defect` for a known fault left unfixed, `unverified` for what could not be checked.
  The line already there is true of nearly the whole repository; replace it once it stops being.

`licence` vocabulary: `open` an explicit licence, `silent` no licence and a dead source,
`alive` no licence but a living source, `forbidden` a written refusal, `original` owing nothing
to anyone — not a name, not an idea traceable to one mod, not a value derived from its assets.
