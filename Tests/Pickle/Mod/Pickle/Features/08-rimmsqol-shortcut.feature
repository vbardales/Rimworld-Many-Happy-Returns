# TEST_SCENARIOS.md S12, the part one process can play: the shortcut revealed through RIMMSQOL's OWN
# settings instance (its own list, its own Visible field, its own WriteSettings), not by moving
# buttonVisible by hand as 03-settings-shortcut.feature does. Uses PickleTools' RimmsqolSteps, which
# makes the same typed calls RIMMSQOL's interface makes; it does not click RIMMSQOL's checkbox (the
# tool's README says so, and that stays read from RIMMSQOL's source, not shown).
#
# Needs the pass map wsl-deps.avec-rimmsqol.map. Without RIMMSQOL staged every scenario here skips, so
# a green minimal pass says nothing about them: read the skips.
@rimmsqol @requires:MalteSchulze.RIMMSqol @requires:nelim.pickletools.rimmsqol
Feature: RIMMSQOL can reveal the settings shortcut, and it opens the same settings

  Background:
    Given the save "test-colony" is loaded
    And Many Happy Returns settings are at their defaults
    And I close all dialogs
    And RIMMSQOL is ready to be driven

  Scenario: RIMMSQOL lists the shortcut, hidden, with no choice recorded
    Then RIMMSQOL's own list of main buttons offers "Nelim_ManyHappyReturnsSettings"
    And RIMMSQOL shows the main button "Nelim_ManyHappyReturnsSettings" as hidden
    And RIMMSQOL holds no choice for the main button "Nelim_ManyHappyReturnsSettings"
    And the main bar does not draw the button "Nelim_ManyHappyReturnsSettings"

  # The edit is made through the shortcut and read back through the primary route: both build their
  # dialog for the one Mod instance, so the value must agree, and the file must hold it.
  Scenario: revealed, it opens this mod's settings, and an edit is shared with Mod options
    When RIMMSQOL reveals the main button "Nelim_ManyHappyReturnsSettings"
    Then the main bar draws the button "Nelim_ManyHappyReturnsSettings"
    When the main bar's button "Nelim_ManyHappyReturnsSettings" is activated
    Then a settings dialog is open for Many Happy Returns
    When the announce-birthdays setting is switched off
    And the settings dialog is closed
    Then Many Happy Returns's settings file on disk records the announce-birthdays setting as off
    Given the primary settings page for Many Happy Returns is opened
    Then a settings dialog is open for Many Happy Returns
    And the announce-birthdays setting reads off
    And no errors were logged
    When I close all dialogs

  Scenario: hidden again it leaves the bar, and forgetting it leaves RIMMSQOL's file empty
    When RIMMSQOL reveals the main button "Nelim_ManyHappyReturnsSettings"
    And RIMMSQOL hides the main button "Nelim_ManyHappyReturnsSettings"
    Then the main bar does not draw the button "Nelim_ManyHappyReturnsSettings"
    And RIMMSQOL's settings file records the main button "Nelim_ManyHappyReturnsSettings" as hidden
    When RIMMSQOL forgets its choice for the main button "Nelim_ManyHappyReturnsSettings"
    Then RIMMSQOL's settings file records no choice for the main button "Nelim_ManyHappyReturnsSettings"

  # @review: RIMMSQOL's own edit page for the shortcut, for a person to read - its label, its defName and
  # its description as this mod translated them.
  @review
  Scenario: RIMMSQOL's edit page for the shortcut, for a person to look at
    When RIMMSQOL's own window is opened on the main button "Nelim_ManyHappyReturnsSettings"
    And I take a screenshot "RIMMSQOL edit page for the Many Happy Returns shortcut"
    And I close all dialogs
