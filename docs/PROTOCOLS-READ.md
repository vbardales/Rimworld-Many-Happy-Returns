# Protocol documents read

What this session read, in which version (`git log -1 --format='%h %ad' -- <file>`; "modified, not committed"
when `git status` shows the file changed), and whether it was of use for this mod. Read on 2026-09-25. Reread a
file only when its version below is no longer the last commit that touched it. The monorepo is
`C:\Users\nelim\Documents\rimworld`; `PickleTools`, `Rimworld-Release-Admin` and `Rimworld-Ticket-Dispatcher` are
repositories of their own, cloned beside it.

## Monorepo

| File | Version read | Used? | What it gave this mod |
| --- | --- | --- | --- |
| `AGENTS.md` | 90d51374 2026-09-25 | yes | Evidence rules (latest report per revision, one text line per run in `docs/runs/`, delete with a list first); publication by CI: dry-run of the exact commit, full SHA, only Virginie approves `steam-production`, no session touches credentials |
| `AUDIT.md` | 90d51374 2026-09-25 | yes | Every gate; the Pickle rules (a request carries no SHA, the tree must stay on the revision under test, no watcher of any kind, one request per pass, small tickets); `done -> tested` criteria; the fail fast conditions before a `publish` (no red without a green replay, gallery, the owner's manual validations, rollback target) |
| `PUBLISHING.md` | 90d51374 2026-09-25 | yes | Description rules (Workshop link on every named mod, THANKS for every exercised tool and integration, adoption clause, source link last), the gallery folder rule (`01-`, `02-`, nothing else in it), the collection's thank-you register, topics and social preview, CI publishing, and what the owner does by hand when a 1.0.0 goes to production |
| `TRANSLATIONS.md` | 90d51374 2026-09-25 | partly | The gate already passed (`localization`, `translation_en`, `translation_fr` complete); nothing changed in player-facing text since. Reread if the mod's texts change |
| `STYLE_RIMWORLD.md` | 90d51374 2026-09-25 | no | Art direction for the Preview and the ModIcon, both already made and inspected; only the 32 px icon check and the file constraints could apply. Not needed again unless the Preview is regenerated |
| `scripts/SEARCHING.md` | 90d51374 2026-09-25 | no | Searching the mod corpus for defNames and classes; this mod ports nothing and declares nothing shared. Skipped after its opening section |
| `WORKSHOP_COMMENTS.md` (not on the list, named by `PUBLISHING.md`) | aba3082f 2026-09-25, **modified, not committed** (another session is editing it) | yes | The register decides whether a thank-you comment is still to be posted: Pickle and RIMMSQOL are already `posted`, the three integrations of this mod are not in it |
| `MOD_SETTINGS.md` (named by the others, not on the list) | not reread | no | The settings gate is `complete` since 2026-09-23 |

## Tool repositories

| File | Version read | Used? | What it gave this mod |
| --- | --- | --- | --- |
| `PickleTools/README.md` | 2b7b6d0 2026-09-25 | yes | The list of companions, and the rule that a pass map names them with a `path:` line |
| `PickleTools/Headless/README.md` | b2712fc 2026-09-25 | yes | Filters (`::text`, commas are OR), passes and maps, exit codes, `-EvidenceDir`, the default 5 s step timeout, Pickle's own `I select {string}` (exact label) and camera steps |
| `PickleTools/docs/steps.md` | 7268217 2026-09-25 | yes | The steps that exist, to avoid writing one twice; no step starts a new colony |
| `PickleTools/Authoring/README.md` (named by `AUDIT.md`) | b1f1abd 2026-09-25 | consulted earlier in the session, not reread today | Suite layout, `@timeout:N` valid on a `Scenario` only |
| `Rimworld-Release-Admin/docs/OPERATIONS.md` | d403592 2026-09-25 | yes | Dry-run before every publication, what a dry-run does not prove, the two publish paths, the Steam limits, credentials belong to the owner |
| `Rimworld-Ticket-Dispatcher/docs/WELCOME.md` | 79668cc 2026-09-25 | yes | Registering with `REGISTER`, filter terms, `-DepMap`, no watching of the queue, a request carries no SHA, evidence deletion trap (`robocopy /MIR`), the rule to note the versions read |
| `Rimworld-Ticket-Dispatcher/docs/SUBMIT.md` | 79668cc 2026-09-25 | yes | Every option of `Submit-PickleRun.ps1`, the launcher's exit codes, `-Extra '-pickle-scenario-timeout=N'` for slow scenarios |

## This repository

| File | Version read | Used? | Notes |
| --- | --- | --- | --- |
| `STATUS.md` | 72a10ca 2026-09-25 | yes | Mine; the oldest sections (2026-09-23) are dated history and stay as written |
| `README.md` | e463cd5 2026-09-23 | yes | No credit for the test tools |
| `CHANGELOG.md` | 2f6ea68 2026-09-23 | yes | `## 0.1.0` lacks the brackets the documented release mode expects; the 1.0.0 section is still to write |
| `ATTRIBUTION.md` | e463cd5 2026-09-23 | yes | Identical to `Mod/ATTRIBUTION.md` (compared with `cmp` on 2026-09-25) |
| `LICENSE` | 933fe40 2026-09-20 | yes | MIT, also in `Mod/` |
| `PUBLICATION.md` | 2f6ea68 2026-09-23 | yes | Three draft comments, none checked against the register yet; no `### 1.0.0` change note yet |
| `TESTING.md` | 2e28bc3 2026-09-25 | yes | Pass matrix, includes the showcase pass |
| `Mod/About/About.xml` | 5576748 2026-09-23 | yes | Description ends with the source link; no credit for Pickle, RimLogging, PickleTools or RIMMSQOL |
| `docs/runs/` | f1807d4 2026-09-25 | yes | One summary per run |
| `Tests/Pickle/` | 2e28bc3 2026-09-25 | yes | Mine |
| `BACKLOG.md`, `NOTES.md`, `BUGS.md` | absent | n/a | This repository has none; nothing to read |
