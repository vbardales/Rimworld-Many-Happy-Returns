# TEST_SCENARIOS.md S12, first launch of the restart chain: RIMMSQOL's choice must outlive the process.
# Three launches under one hold of the lock, in this order (see Tests/Pickle/README.md):
#   09 reveals and keeps, 10 finds it revealed, hides and keeps, 11 finds it hidden and forgets.
# "RIMMSQOL's choices are kept for the next launch" is the LAST step: a scenario that fails before it
# leaves nothing behind. Never run this file without its two readers, and never unfiltered: see the
# chain command in TESTING.md.
@rimmsqol @requires:MalteSchulze.RIMMSqol @requires:nelim.pickletools.rimmsqol
Feature: the shortcut's visibility survives a restart, first launch

  Background:
    Given the save "test-colony" is loaded
    And Many Happy Returns settings are at their defaults
    And I close all dialogs
    And RIMMSQOL is ready to be driven

  Scenario: reveal the shortcut through RIMMSQOL and keep the choice
    When RIMMSQOL reveals the main button "Nelim_ManyHappyReturnsSettings"
    Then the main bar draws the button "Nelim_ManyHappyReturnsSettings"
    And RIMMSQOL's settings file records the main button "Nelim_ManyHappyReturnsSettings" as visible
    When RIMMSQOL's choices are kept for the next launch
