# TEST_SCENARIOS.md S10, the exemptions that need a pawn put in a state the fixture does not hold. The
# two others (a recruit of one day, a psychopath) are in 06-verdict-thresholds-and-forgotten.feature.
# Every scenario loads the saved fixture again first, so a pawn made Downed or sent off in a caravan is
# discarded with its scenario.
#
# The rules under test are GameComponent_Birthdays.Evaluate's early returns and TryGiveForgotten's
# witness floor: a pawn that is away or Downed at the verdict gets none either way, and the forgotten
# memory needs at least two other colonists on the map, babies excluded.
Feature: nobody is judged who could not have been remembered or forgotten

  Background:
    Given the save "test-colony" is loaded
    And Many Happy Returns settings are at their defaults
    And the celebrant is the map's first eligible colonist
    And at least a third eligible colonist is present
    And the celebrant's birthday is moved to today
    And the celebrant has been a colonist for 20 days

  Scenario: a Downed celebrant is not judged, unwished
    Given the celebrant is downed
    When Many Happy Returns closes out today's birthdays
    Then the celebrant holds no birthday-forgotten memory
    And the celebrant holds no birthday-remembered memory
    And no errors were logged

  # "No verdict either way": being wished earlier in the day does not bring a verdict to a pawn who is
  # Downed when the day closes.
  Scenario: a Downed celebrant is not judged, wished
    Given the wisher is the map's second eligible colonist
    And the wisher is brought next to the celebrant
    And the wisher exchanges the birthday wish with the celebrant
    And the celebrant is downed
    When Many Happy Returns closes out today's birthdays
    Then the celebrant holds no birthday-remembered memory
    And the celebrant holds no birthday-forgotten memory

  Scenario: a celebrant away in a caravan is not judged
    When the celebrant leaves the map in a caravan
    And Many Happy Returns closes out today's birthdays
    Then the caravan celebrant holds no birthday-forgotten memory
    And the caravan celebrant holds no birthday-remembered memory
    And no errors were logged

  # The witness floor is two, so the boundary is played on both sides: one other colonist is too few, two
  # are enough. The second scenario is the control that says the first one is not passing by accident.
  Scenario: with a single other colonist on the map nobody was there to forget
    Given all colonists but the celebrant and 1 other have left the map in a caravan
    When Many Happy Returns closes out today's birthdays
    Then the celebrant holds no birthday-forgotten memory

  Scenario: with two other colonists on the map the birthday can be forgotten
    Given all colonists but the celebrant and 2 others have left the map in a caravan
    When Many Happy Returns closes out today's birthdays
    Then the celebrant holds a birthday-forgotten memory
