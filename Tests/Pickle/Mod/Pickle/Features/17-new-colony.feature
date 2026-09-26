# TEST_SCENARIOS.md S16, the half that says "a new colony": the mod in a colony that has just been made, from the first
# minutes, played through and read by the game's log. The other half, an existing save, is feature 16.
#
# A NEW COLONY IS NOT REPRODUCIBLE, so this is played once, in the final pass, in a ticket of its own, never for a fix or an
# exploration, and never chained with other features. The step is Nelim's Pickle Tools' NewColony: it plays what the game's own
# new-colony pages do (Crashlanded, three colonists, paused). The assertions below hold for ANY three colonists: no name, no
# trait, no tenure is assumed. The ticket passes -Extra "-pickle-scenario-timeout=400" (Pickle's watchdog kills at 120 s).
#
# Not proven: a game without Ideology, another scenario, the same seed twice, a key never displayed.
@requires:nelim.pickletools.newcolony @requires:nelim.pickletools.loadaudit
Feature: the mod in a new colony, from its first minutes, leaves a clean log

  @timeout:60
  Scenario: a birthday played through in a colony made a moment ago, then the settings opened and closed
    Given the main menu is open
    And Nelim's Pickle Tools: the new colony's seed is "many-happy-returns-s16"
    When Nelim's Pickle Tools: a new colony is started
    Then Many Happy Returns's own defs are all resolved
    Given Many Happy Returns settings are at their defaults
    And the celebrant is the map's first eligible colonist
    And the wisher is the map's second eligible colonist
    And at least a third eligible colonist is present
    And the local hour at the celebrant is set to 8
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
