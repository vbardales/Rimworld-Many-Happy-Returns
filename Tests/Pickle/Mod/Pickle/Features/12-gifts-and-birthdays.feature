# TEST_SCENARIOS.md S13: Gifts and Birthdays present. Its congratulation and its party are exercised
# through its own real code (its InteractionDef through the game's TryInteractWith, its
# LordJob_BirthdayParty through the vanilla ApplyOutcome), with nothing referenced at compile time.
# What is left out is only what Many Happy Returns does not depend on: its organiser search, its letter,
# its gifts, and the five to fifteen thousand ticks a party lasts.
#
# Needs wsl-deps.avec-gifts-and-birthdays.map. In the minimal pass every scenario here skips: read the
# skips. The same-pass absent half of S13 ("no dependency errors when absent") is 01-loading.feature's
# correspondence scenario, which is green in both passes.
@requires:KrukuCoB.rout
Feature: Gifts and Birthdays' congratulation and party count towards the verdict

  Background:
    Given the save "test-colony" is loaded
    And Many Happy Returns settings are at their defaults
    And the celebrant is the map's first eligible colonist
    And the wisher is the map's second eligible colonist
    And at least a third eligible colonist is present
    And the wisher is brought next to the celebrant
    And the celebrant's birthday is moved to today

  # BirthdayUtility.CountWishers counts a pawn once whichever of the two mods recorded the wish, and
  # InteractionWorker_BirthdayWish disqualifies the pair once either has: the two tallies are added,
  # never doubled.
  Scenario: its congratulation counts as one wisher, and the same colonist is not counted twice
    When Gifts and Birthdays congratulates the celebrant from the wisher
    Then the celebrant holds a Gifts and Birthdays congratulation from the wisher
    And Many Happy Returns counts the celebrant as wished by 1 colonist today
    And the wish's selection weight from the wisher to the celebrant is zero
    When I wait 130 ticks
    And the wisher exchanges the birthday wish with the celebrant
    Then Many Happy Returns counts the celebrant as wished by 1 colonist today
    And no errors were logged

  Scenario: a congratulation alone is enough for a remembered birthday, at the stage the tally gives
    When Gifts and Birthdays congratulates the celebrant from the wisher
    And Many Happy Returns closes out today's birthdays
    Then the celebrant holds a birthday-remembered memory
    And the celebrant holds no birthday-forgotten memory
    And the celebrant's remembered memory is at the stage the day's own tally gives

  # "Its party is recognised as a party": the party's own attendee thought is the vanilla one the day's
  # bonus looks for, and a party played out raises the bonus by exactly the party's two points.
  Scenario: its party raises the day's bonus by the party's two points
    Given Gifts and Birthdays congratulates the celebrant from the wisher
    And a Gifts and Birthdays party is held for the celebrant, organised by the wisher
    And I wait 2 ticks
    Then the party's attendee thought is the one Many Happy Returns reads as a party
    Given Many Happy Returns notes the celebrant's day-quality bonus
    When the Gifts and Birthdays party is played out with everyone present throughout
    Then the celebrant holds a party memory
    And the celebrant's day-quality bonus is 2 higher than noted
    When Many Happy Returns closes out today's birthdays
    Then the celebrant holds a birthday-remembered memory
    And the celebrant's remembered memory is at the stage the day's own tally gives
    And no errors were logged
