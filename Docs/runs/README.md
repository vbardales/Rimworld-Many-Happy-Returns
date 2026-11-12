# Pickle runs

One text summary per run of the suite in `Tests/Pickle/`: pass, language, date, launcher command,
`exitReason`, scenarios discovered against played, verdict, and what was read in the captures.

The evidence itself (`junit.xml`, `summary.md`, `messages.ndjson`, `Player.log`, captures) is written by the
launcher with `-EvidenceDir ManyHappyReturns/Tests/Pickle/Evidence/<run>`. It stays on disk and is ignored by
git; a summary here is what the repository keeps.
