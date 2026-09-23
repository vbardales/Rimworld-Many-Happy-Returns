# What only a running game shows about settings persistence: that closing the real
# Dialog_ModSettings actually calls Mod.WriteSettings (Window.PreClose, per WindowStack.TryRemove),
# and that a freshly re-read Mod.GetSettings<T>() - not the in-memory object this scenario just
# mutated - agrees with what was written. The values themselves (defaults, reset, NaN/Infinity/
# out-of-range clamping, a headless Scribe round trip through the mod's own LoadingVars/
# PostLoadInit callbacks) are already proven offline in Tests/Program.cs and not repeated here.
Feature: settings persist through a real Dialog_ModSettings close

  Background:
    Given the save "test-colony" is loaded
    And Many Happy Returns settings are at their defaults
    And I close all dialogs

  Scenario: a change made while the primary settings page is open survives closing it
    Given the primary settings page for Many Happy Returns is opened
    When the announce-birthdays setting is switched off
    And the settings dialog is closed
    Then Many Happy Returns's settings file on disk records the announce-birthdays setting as off
