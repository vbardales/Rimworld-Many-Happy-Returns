# Second launch of the restart chain. Nothing in this process reveals the shortcut: it must already be
# there because RIMMSQOL applied its own file at startup. The first step refuses to pass when the writer
# ran in THIS process, which would be a restart test that never restarted.
@rimmsqol @requires:MalteSchulze.RIMMSqol @requires:nelim.pickletools.rimmsqol
Feature: the shortcut's visibility survives a restart, second launch

  Background:
    Given the save "test-colony" is loaded
    And Many Happy Returns settings are at their defaults
    And I close all dialogs
    And RIMMSQOL is ready to be driven

  Scenario: the revealed shortcut is still there and still opens this mod's settings, then hide it and keep
    Given the choices RIMMSQOL kept in the previous launch are in place
    Then the main bar draws the button "Nelim_ManyHappyReturnsSettings"
    And RIMMSQOL shows the main button "Nelim_ManyHappyReturnsSettings" as visible
    When the main bar's button "Nelim_ManyHappyReturnsSettings" is activated
    Then a settings dialog is open for Many Happy Returns
    When I close all dialogs
    And RIMMSQOL hides the main button "Nelim_ManyHappyReturnsSettings"
    Then the main bar does not draw the button "Nelim_ManyHappyReturnsSettings"
    When RIMMSQOL's choices are kept for the next launch
