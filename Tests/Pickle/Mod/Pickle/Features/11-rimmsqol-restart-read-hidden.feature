# Third and last launch of the restart chain: the hide survived too, and forgetting leaves RIMMSQOL's
# file with no entry for this shortcut, so nothing is left behind for the next run that stages it.
@rimmsqol @requires:MalteSchulze.RIMMSqol @requires:nelim.pickletools.rimmsqol
Feature: the shortcut's visibility survives a restart, third launch

  Background:
    Given the save "test-colony" is loaded
    And Many Happy Returns settings are at their defaults
    And I close all dialogs
    And RIMMSQOL is ready to be driven

  Scenario: the hidden shortcut is still hidden, then forget the choice
    Given the choices RIMMSQOL kept in the previous launch are in place
    Then the main bar does not draw the button "Nelim_ManyHappyReturnsSettings"
    And RIMMSQOL shows the main button "Nelim_ManyHappyReturnsSettings" as hidden
    And RIMMSQOL's settings file records the main button "Nelim_ManyHappyReturnsSettings" as hidden
    When RIMMSQOL forgets its choice for the main button "Nelim_ManyHappyReturnsSettings"
    Then RIMMSQOL's settings file records no choice for the main button "Nelim_ManyHappyReturnsSettings"
