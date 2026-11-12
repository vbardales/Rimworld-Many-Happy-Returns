# Targeted pass, English, 2026-09-25 (the four failures of the 2026-09-24 replay)

- **Command**: `Submit-PickleRun.ps1 -Mod ManyHappyReturns -Language English -Filter "::a psychopath,::a Downed celebrant is not judged,::a save and reload during the birthday" -EvidenceDir ManyHappyReturns/Tests/Pickle/Evidence/2026-09-24-cibles-en` (request `20260924-210440-656-33d1`)
- **Suite state**: commit `32999c6` (the fixes of the review of the replay)
- **exitReason**: `passed`. Pickle exit 0, one attempt
- **Scenarios**: 5 discovered, 5 played, 5 passed. The two `a psychopath` ones, `a Downed celebrant is not judged` unwished (already green, kept as the control) and wished, and the save and reload in the middle of the birthday.
- **Evidence**: on disk, ignored by git (`Tests/Pickle/Evidence/2026-09-24-cibles-en/`): `junit.xml`, `summary.json`, `summary.md`, `Player.log`. The 8 KB `report.html` and `messages.ndjson` were deleted.

## What it settles

The psychopath exemption holds as a nullified thought with a zero mood offset. The three scenarios whose wish exchange had been refused
now pass, in one attempt each.

That is one sample, not a proof of the cause. The refusal was intermittent (three of nine scenarios, plus one French), so five
green scenarios do not say that clearing the initiator's chat cooldown is what fixed it. The failure message now names every condition
of the refusal: if it comes back in the full pass it will say which.

## Player.log

The only `[ERROR]` line is the test companion's `did not load any content`, present in every log of this suite. Nothing from the mod.
