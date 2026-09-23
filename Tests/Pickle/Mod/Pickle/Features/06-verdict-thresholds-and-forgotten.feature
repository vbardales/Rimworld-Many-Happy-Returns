# What only a running game shows about the end-of-day verdict: that CloseOutDay, wired to a real
# Pawn, actually forms Nelim_BirthdayRemembered or Nelim_BirthdayForgotten through the game's own
# MemoryThoughtHandler.TryGainMemory - where nullifyingTraits and the settings' mood factor are
# both enforced by real engine code, not by this mod alone - and that a formed memory survives a
# real save and reload.
#
# The grade a score maps to (StageForScore) and the wisher-count weighting (PointsForWishers) are
# already exhaustively covered offline in Tests/Program.cs and are not re-derived here: these
# scenarios assert that the real memory formed carries the stage the mod's own function already
# computed, not a hand-picked number.
#
# Left manual, per TEST_SCENARIOS.md S10: a Downed celebrant, a caravan celebrant, and fewer than
# two witnesses. Each is reachable in principle, but forcing them means either disturbing the
# health system of a shared fixture pawn or temporarily removing other colonists from a played
# map - both riskier to a shared fixture than the tenure and trait mutations here, which touch
# only the celebrant and are reverted by VerdictSteps' own [AfterScenario] hook.
Feature: the end-of-day verdict and the forgotten-birthday exemptions

  Background:
    Given the save "test-colony" is loaded
    And Many Happy Returns settings are at their defaults
    And the celebrant is the map's first eligible colonist
    And at least a third eligible colonist is present
    And the celebrant's birthday is moved to today

  Scenario: a wished birthday forms a remembered memory scaled by the real mood setting
    Given the wisher is the map's second eligible colonist
    And the wisher is brought next to the celebrant
    And the wisher exchanges the birthday wish with the celebrant
    When Many Happy Returns closes out today's birthdays
    Then the celebrant holds a birthday-remembered memory
    And no errors were logged

  Scenario: an unwished birthday forms the forgotten memory, scaled by the real mood setting
    When Many Happy Returns closes out today's birthdays
    Then the celebrant holds a birthday-forgotten memory
    And the celebrant holds no birthday-remembered memory

  # TEST_SCENARIOS.md S09: the toggle controls formation, not merely the UI.
  Scenario: switching the forgotten-thought setting off suppresses the memory
    Given the announce-birthdays setting is switched off
    When Many Happy Returns closes out today's birthdays
    Then the celebrant holds no birthday-forgotten memory

  # TEST_SCENARIOS.md S10: a recruit from yesterday expects nothing from anyone yet.
  Scenario: a colonist of one day's tenure is exempt from the forgotten memory
    Given the celebrant has been a colonist for one day
    When Many Happy Returns closes out today's birthdays
    Then the celebrant holds no birthday-forgotten memory

  # Mod/Defs/Birthday.xml's own comment: "a psychopath must still be able to enjoy a good
  # birthday" - the nullifying trait sits only on Nelim_BirthdayForgotten, never on
  # Nelim_BirthdayRemembered, so the two halves of that sentence are two different scenarios.
  Scenario: a psychopath is exempt from the forgotten memory on an unwished birthday
    Given the celebrant is a psychopath
    When Many Happy Returns closes out today's birthdays
    Then the celebrant holds no birthday-forgotten memory

  Scenario: a psychopath still receives a positive verdict on a wished birthday
    Given the celebrant is a psychopath
    And the wisher is the map's second eligible colonist
    And the wisher is brought next to the celebrant
    And the wisher exchanges the birthday wish with the celebrant
    When Many Happy Returns closes out today's birthdays
    Then the celebrant holds a birthday-remembered memory

  Scenario: the remembered memory survives a save and reload
    Given the wisher is the map's second eligible colonist
    And the wisher is brought next to the celebrant
    And the wisher exchanges the birthday wish with the celebrant
    And Many Happy Returns closes out today's birthdays
    And the celebrant holds a birthday-remembered memory
    When I save and reload
    Then the celebrant holds a birthday-remembered memory
    And no errors were logged
