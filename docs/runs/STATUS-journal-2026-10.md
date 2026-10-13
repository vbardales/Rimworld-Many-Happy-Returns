# STATUS journal, October 2026

Folded out of STATUS.md on 2026-10-13 (size over 20 KB). Kept as written.

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
  revision). The French sign-off by Virginie (`translation_fr: complete`) is a separate open item.

