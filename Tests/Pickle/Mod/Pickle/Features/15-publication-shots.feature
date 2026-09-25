# The five images of the Workshop page, in PUBLICATION.md's upload order ("Screenshots, in upload order").
# The state being photographed is asserted before every capture; composition and readability remain a visual
# review, and a green scenario says the trip happened, not that the picture sells anything.
#
# THE SCENE IS THE OWNER'S SHOWCASE COLONY, not the test fixture: the fixture "nelim-zen-meadow-studio" of
# PickleTools/ScreenshotStudio, four named colonists in a meadow, paused. It exists only in a pass that stages
# that companion (wsl-deps.studio.map); every other pass skips this feature by requirement.
#
# RUN IT WITH THE GAME IN ENGLISH: the Workshop page is English, and these images carry the mod's own labels.
#   Run-PickleWsl.ps1 -Mod ManyHappyReturns -DepMap wsl-deps.studio.map -Filter 15-publication-shots.feature
#
# Two kinds of image, as in Work Studio's: a window the mod owns (the letter, the settings) is taken
# with the interface around it hidden, through the game's own screenshot mode; a game pane the mod changes (a
# pawn's inspect pane with its log or its mood) is taken with the full interface, developer mode off.
#
# Not done here, on purpose: the wish bubble is a mote that lasts a few seconds in play. The game is paused
# for the capture, so it is still there if the motes do not age while paused; the review says whether it is.
@review @requires:nelim.pickletools.screenshotmode @requires:nelim.pickletools.screenshotstudio @requires:nelim.pickletools.inspecttabs @requires:nelim.pickletools.hoversteps
Feature: images for the Workshop page

  Background:
    Given the save "nelim-zen-meadow-studio" is loaded
    And game speed is paused
    And Many Happy Returns settings are at their defaults
    And the celebrant is the map's first eligible colonist
    And the wisher is the map's second eligible colonist
    And at least a third eligible colonist is present

  # 1. The whole pitch: the game noticing on its own what it otherwise ignores.
  @timeout:30
  Scenario: the morning letter, open
    Given the local hour at the celebrant is set to 8
    And the celebrant's birthday is moved to today
    And Many Happy Returns rescans for birthdays
    And a birthday letter for the celebrant is on the stack
    When I open the birthday letter for the celebrant
    Then the letter window is open
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "Workshop page, 1 the morning letter"
    And Nelim's Pickle Tools: screenshot mode is disabled

  # 2. The interaction that carries most of the mod's day-to-day presence: the wish and its social log line.
  @timeout:30
  Scenario: two colonists exchanging the birthday wish
    Given the celebrant's birthday is moved to today
    And the wisher is brought next to the celebrant
    When the wisher exchanges the birthday wish with the celebrant
    And the on-screen messages are cleared
    And the camera is centred on the celebrant
    And I select the celebrant
    And Nelim's Pickle Tools: I open the "Log" inspect tab
    Then the celebrant holds a birthday wish received from the wisher
    And Nelim's Pickle Tools: the "Log" inspect tab is open
    And Nelim's Pickle Tools: developer mode is turned off for the capture
    And I take a screenshot "Workshop page, 2 the wish"

  # 3. Where the verdict lives, graded rather than binary. Three colonists wish the fourth: three points, the
  # second of five grades. The mood tooltip is what lists the memory, so the pointer is put on it.
  @timeout:30
  Scenario: the day's memory in the mood tooltip
    Given the celebrant's birthday is moved to today
    When every other colonist wishes the celebrant a happy birthday
    And Many Happy Returns closes out today's birthdays
    And the on-screen messages are cleared
    And the camera is centred on the celebrant
    And I select the celebrant
    And Nelim's Pickle Tools: I open the "Needs" inspect tab
    Then the celebrant holds a birthday-remembered memory
    And Nelim's Pickle Tools: the "Needs" inspect tab is open
    And Nelim's Pickle Tools: developer mode is turned off for the capture
    When Nelim's Pickle Tools: I hover over the tooltip containing "Quiet birthday"
    Then Nelim's Pickle Tools: the tooltip containing "Quiet birthday" is drawn
    And I take a screenshot "Workshop page, 3 the memory"

  # 4. "Can I tune this": the settings page with the slider away from its default.
  @timeout:30
  Scenario: the settings page, with the mood scale changed
    Given the mood scale is set to 150 percent
    And the primary settings page for Many Happy Returns is opened
    Then a settings dialog is open for Many Happy Returns
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "Workshop page, 4 the settings"
    And Nelim's Pickle Tools: screenshot mode is disabled

  # 5. The sad half, so the page is honest about it: nobody said a word all day.
  @timeout:30
  Scenario: the forgotten birthday in the mood tooltip
    Given the celebrant has been a colonist for 20 days
    And the celebrant's birthday is moved to today
    When Many Happy Returns closes out today's birthdays
    And the on-screen messages are cleared
    And the camera is centred on the celebrant
    And I select the celebrant
    And Nelim's Pickle Tools: I open the "Needs" inspect tab
    Then the celebrant holds a birthday-forgotten memory
    And Nelim's Pickle Tools: the "Needs" inspect tab is open
    And Nelim's Pickle Tools: developer mode is turned off for the capture
    When Nelim's Pickle Tools: I hover over the tooltip containing "Nobody remembered my birthday"
    Then Nelim's Pickle Tools: the tooltip containing "Nobody remembered my birthday" is drawn
    And I take a screenshot "Workshop page, 5 the forgotten birthday"
