# TEST_SCENARIOS.md S06: the log line of the birthday wish agrees with the initiator in French
# ({INITIATOR_gender ? m : f : n} in the "s'est souvenu" rule). A comment in the XML is not a proof: the
# line is built by the game's own PlayLogEntry_Interaction for each of the three genders and read.
# Play it in French (the agreement) and in English (the line stays the plain "remembered it was").
Feature: the wish's log line agrees with the initiator's gender

  Background:
    Given the save "test-colony" is loaded
    And Many Happy Returns settings are at their defaults
    And the celebrant is the map's first eligible colonist
    And the wisher is the map's second eligible colonist

  Scenario: a male initiator
    Given the wisher's gender is Male
    Then the wisher's remembered-birthday log line agrees with the wisher's gender

  Scenario: a female initiator
    Given the wisher's gender is Female
    Then the wisher's remembered-birthday log line agrees with the wisher's gender

  Scenario: an initiator without gender
    Given the wisher's gender is None
    Then the wisher's remembered-birthday log line agrees with the wisher's gender
