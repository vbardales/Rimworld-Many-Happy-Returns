# TEST_SCENARIOS.md S11: the day closing on its own. The verdict is formed by GameComponent_Birthdays
# when the absolute day changes, not by the debug action the other features use, and only the game clock
# can prove that path. The clock is positioned a few hundred ticks short of a boundary with
# TickManager.DebugSetTicksGame (the call the game's own "Increment time" makes) and everything after
# that is played tick by tick: a jumped-over day would form no wishes at all, which is why the clock is
# only ever POSITIONED here and never used to skip the birthday itself.
#
# No scenario here asserts the letter across a boundary: it is dropped past hour 20 local, and the local
# hour of a boundary depends on the map's longitude. The letter is asserted in the one scenario that
# fixes the local hour first.
Feature: the birthday is judged when the day turns, and only once

  Background:
    Given the save "test-colony" is loaded
    And Many Happy Returns settings are at their defaults
    And the celebrant is the map's first eligible colonist
    And the wisher is the map's second eligible colonist
    And at least a third eligible colonist is present

  Scenario: a wished birthday is judged at midnight without any debug action
    Given the game clock is set to 400 ticks before midnight
    And I wait 5 ticks
    And the celebrant's birthday is moved to today
    And the wisher is brought next to the celebrant
    And the wisher exchanges the birthday wish with the celebrant
    And Many Happy Returns is tracking 1 birthday today
    When I wait 450 ticks
    Then the celebrant holds a birthday-remembered memory
    And Many Happy Returns is tracking 0 birthdays today
    And it is no longer the celebrant's birthday
    And no errors were logged

  Scenario: an unwished birthday is judged at midnight too, and the next day tracks nothing
    Given the game clock is set to 400 ticks before midnight
    And I wait 5 ticks
    And the celebrant's birthday is moved to today
    When I wait 450 ticks
    Then the celebrant holds a birthday-forgotten memory
    And Many Happy Returns is tracking 0 birthdays today

  # The last day of the year has the highest DayOfYear, and the next one the lowest: the comparison with
  # BirthDayOfYear must hold across that wrap, or a December birthday is never judged.
  Scenario: a birthday on the last day of the year is judged when the year turns
    Given the game clock is set to 400 ticks before the end of the year
    And I wait 5 ticks
    And the celebrant's birthday is moved to today
    And the wisher is brought next to the celebrant
    And the wisher exchanges the birthday wish with the celebrant
    When I wait 450 ticks
    Then the celebrant holds a birthday-remembered memory
    And Many Happy Returns is tracking 0 birthdays today
    And it is no longer the celebrant's birthday

  # A save in the middle of the day must repeat neither the letter (the record carries letterSent) nor
  # cost the verdict, which is still formed when the day ends.
  Scenario: a save and reload during the birthday repeats no letter and keeps the verdict
    Given the local hour at the celebrant is set to 8
    And the celebrant's birthday is moved to today
    And the wisher is brought next to the celebrant
    And the wisher exchanges the birthday wish with the celebrant
    And exactly 1 birthday letter for the celebrant are on the stack
    When I save and reload
    And Many Happy Returns rescans for birthdays
    Then exactly 1 birthday letter for the celebrant are on the stack
    And Many Happy Returns is tracking 1 birthday today
    Given the game clock is set to 400 ticks before midnight
    When I wait 450 ticks
    Then the celebrant holds a birthday-remembered memory
    And exactly 1 birthday letter for the celebrant are on the stack
    And Many Happy Returns is tracking 0 birthdays today
