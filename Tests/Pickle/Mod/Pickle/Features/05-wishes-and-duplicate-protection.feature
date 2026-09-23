# What only a running game shows about the wish: InteractionWorker_BirthdayWish.RandomSelectionWeight
# evaluated against real Pawn objects (real memory handlers, relations, developmental stage), and
# the real Pawn_InteractionsTracker.TryInteractWith callback the game itself uses to run any
# interaction.
#
# TryInteractWith runs the worker's Interacted() unconditionally - it never consults
# RandomSelectionWeight. So "the pair does not get picked again by the natural interaction
# roulette" (TEST_SCENARIOS.md S06) is exactly what reading the weight proves; exchanging the wish
# twice and hoping the second is refused would prove nothing, since nothing refuses it that way.
#
# The scoring arithmetic (PointsForWishers, StageForScore) is already exhaustively covered offline
# in Tests/Program.cs and is not repeated here.
Feature: the birthday wish and its duplicate protection

  Background:
    Given the save "test-colony" is loaded
    And Many Happy Returns settings are at their defaults
    And the celebrant is the map's first eligible colonist
    And the wisher is the map's second eligible colonist
    And the wisher is brought next to the celebrant
    And the celebrant's birthday is moved to today

  Scenario: an eligible wisher is strongly favoured before the wish, and disqualified after
    Then the wish's selection weight from the wisher to the celebrant is high
    When the wisher exchanges the birthday wish with the celebrant
    Then the wish's selection weight from the wisher to the celebrant is zero

  Scenario: exchanging the wish forms both social memories, and each pawn counts as one wisher
    When the wisher exchanges the birthday wish with the celebrant
    Then the celebrant holds a birthday wish received from the wisher
    And the wisher holds a birthday wish given to the celebrant
    And Many Happy Returns counts the celebrant as wished by 1 colonist today
    And no errors were logged
