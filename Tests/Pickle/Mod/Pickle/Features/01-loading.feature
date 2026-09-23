# What only a running game shows about loading: that MHRDefOf's static constructor actually
# resolved every def it names against the real def database, and that the optional lookup of
# Gifts and Birthdays' congratulation thought agrees with whether that mod is really loaded in
# THIS pass. XML validity, def references and DefInjected paths are proven offline by
# Check-DefRefs.ps1, Check-XmlFields.ps1 and Check-DefInjected.ps1, and not repeated here.
Feature: Many Happy Returns loads cleanly

  Scenario: the mod's own defs resolve and no errors are logged
    Given the save "test-colony" is loaded
    Then Many Happy Returns's own defs are all resolved
    And no errors were logged

  # Correct in every pass, whether Gifts and Birthdays is staged (wsl-deps.avec-gifts-and-birthdays.map)
  # or not (the minimal pass): the claim is a correspondence, not a fixed answer.
  Scenario: the optional Gifts and Birthdays lookup matches what this pass loaded
    Given the save "test-colony" is loaded
    Then Many Happy Returns's optional Gifts and Birthdays lookup matches what is loaded

  # TEST_SCENARIOS.md S14, the half that belongs to this mod: the MayRequire guard on the Inhumanized entry
  # follows the DLC. Correct in every pass, so the default one (Anomaly staged) reads it listed and
  # wsl-deps.sans-anomaly.map (Anomaly left out) reads it absent, with no unresolved reference and no error
  # in either. The engine's own refusal of a memory for an inhumanized pawn is not this mod's to prove.
  Scenario: the Anomaly guard on the birthday thoughts follows whether the DLC is active
    Given the save "test-colony" is loaded
    Then the birthday thoughts list the Inhumanized hediff exactly when Anomaly is active
    And no errors were logged
