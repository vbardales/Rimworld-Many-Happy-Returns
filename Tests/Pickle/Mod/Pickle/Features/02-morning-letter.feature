# The letter the base game never sends for an ordinary birthday. Matched by rebuilding the exact
# line from the mod's own translation key, never on English text, so the same scenarios hold in
# the French pass. What only a running game shows: a real letter object on the real LetterStack,
# and that a second rescan the same day does not queue it twice (TEST_SCENARIOS.md S04).
Feature: the morning letter

  Background:
    Given the save "test-colony" is loaded
    And Many Happy Returns settings are at their defaults
    And the celebrant is the map's first eligible colonist
    And the local hour at the celebrant is set to 8

  Scenario: moving the celebrant's birthday to today queues exactly one letter
    When the celebrant's birthday is moved to today
    Then a birthday letter for the celebrant is on the stack
    And exactly 1 birthday letter for the celebrant are on the stack

  # DebugRescanNow is also what the game's own hourly rescan calls (RescanIntervalTicks). A second
  # call the same day must not queue a second letter: GameComponent_Birthdays.TrySendLetter guards
  # on record.letterSent, which this proves against the real component rather than the field alone.
  Scenario: a second rescan the same day does not repeat the letter
    Given the celebrant's birthday is moved to today
    And a birthday letter for the celebrant is on the stack
    When Many Happy Returns rescans for birthdays
    And Many Happy Returns rescans for birthdays
    Then exactly 1 birthday letter for the celebrant are on the stack

  # ManyHappyReturnsSettings.morningLetter, read live by TrySendLetter: switching it off before the
  # birthday is detected must leave the stack untouched, not merely suppress a UI checkbox.
  Scenario: switching the letter off before detection leaves no letter
    Given the announce-birthdays setting is switched off
    When the celebrant's birthday is moved to today
    Then exactly 0 birthday letter for the celebrant are on the stack
