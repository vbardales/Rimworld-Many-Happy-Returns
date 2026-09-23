# TEST_SCENARIOS.md S10, the newborn. A baby is a free colonist with a birthday, and the mod's own rule
# (BirthdayUtility.CanCelebrate) still turns it down: nobody wishes a newborn happy birthday and a baby
# cannot be forgotten. Babies only exist with Biotech, so this is a conditional feature: it is played in
# every pass that stages the DLC, which is the default one, and skips where it is left out.
@requires:ludeon.rimworld.biotech
Feature: a newborn is never counted as a birthday

  Background:
    Given the save "test-colony" is loaded
    And Many Happy Returns settings are at their defaults
    And the celebrant is the map's first eligible colonist

  Scenario: a colonist born today is on the calendar and off the list
    Given a newborn colonist is born today next to the celebrant
    When Many Happy Returns rescans for birthdays
    Then the newborn's birthday is today but Many Happy Returns does not count it
    And Many Happy Returns does not track the newborn
    And no errors were logged
