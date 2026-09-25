# TEST_SCENARIOS.md S16, the half that says "an existing save": the mod in a colony that was played before it was
# installed, from the morning letter to the verdict and back through the settings, with the game's log read at the end.
# The fixture "test-colony" is that existing save. The other half of S16, a NEW colony, is not here: it is not
# reproducible and is played once, in the final pass, in its own feature.
#
# The last step is Nelim's Pickle Tools' LoadAudit: no error, exception or warning attributed to this mod, no
# unresolved def, no message repeated five times or more, no Keyed key missing in the active language. It reads the whole
# process log, so it sits at the end of the scenario, after the screens were opened. Run it in English and in French:
# the language check reads the active language's data.
#
# What it does not prove (its own README): a key never displayed, a def never loaded, a path never executed.
@requires:nelim.pickletools.loadaudit
Feature: the mod in an existing save, from the letter to the verdict, leaves a clean log

  Background:
    Given the save "test-colony" is loaded
    And Many Happy Returns settings are at their defaults
    And the celebrant is the map's first eligible colonist
    And the wisher is the map's second eligible colonist
    And at least a third eligible colonist is present

  @timeout:30
  Scenario: a birthday played through on a saved colony, then the settings opened and closed
    Given the local hour at the celebrant is set to 8
    And the celebrant's birthday is moved to today
    And Many Happy Returns rescans for birthdays
    And a birthday letter for the celebrant is on the stack
    And the wisher is brought next to the celebrant
    When the wisher exchanges the birthday wish with the celebrant
    And Many Happy Returns closes out today's birthdays
    Then the celebrant holds a birthday-remembered memory
    When the primary settings page for Many Happy Returns is opened
    Then a settings dialog is open for Many Happy Returns
    When the settings dialog is closed
    Then Nelim's Pickle Tools: the load of the mod "nelim.manyhappyreturns" is clean
