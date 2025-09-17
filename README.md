# Description:
The lower part of the screen is a horizontal scroll of 20 rectangles (hereinafter referred to as cubes) of different colors.
Any of the cubes can be moved to the upper right part and placed on top of each other in the form of a tower with the following conditions:
the first cube can be placed anywhere in the upper right part of the screen,
the rest of the cubes are placed in the tower only if they are moved on top of those already exposed, in this case they bounce with animation and are placed on top of the tower
if the player has moved the cube After missing the tower, the cube disappears or explodes, preferably animatedly.,
cubes are placed on top of the previous ones in a tower with a random horizontal offset, but no more than 50% of the face length,
the height limit is the height of the device screen, if the last cube goes beyond its border, then it becomes impossible to install more,
the cubes at the bottom are endless and do not disappear after dragging up,
physics is not used, only animations.
At any time, a cube from the tower can be thrown into a hole by transferring it to its image in the upper-left part of the screen. In this case, the ones above the tower smoothly descend with animation. It is advisable to take into account its oval borders when assessing whether a player has hit it by dragging a cube.
Each part of the screen has its own background.
Each action (installing a cube, throwing a cube, disappearing a cube, height restriction) should be commented with an inscription above the bottom of the screen.
The game configuration should contain a minimum: the number of cubes at the bottom and their colors.

# Requirements:
The code should provide for scaling for future updates (for example, you will need to transfer a new cube only to a cube of the same color in the tower).
Provide for the possibility of localization (it is possible without the system itself).
It should be noted that the source of the game configuration can be different data sources (the game may have 1 implementation - from ScriptableObject).
Drag & drop of cubes should work correctly, taking into account the scrolling (there should be no situations when the bottom bar scrolls at the beginning of dragging).
At least 1 animation must be used (Unity Animation or DOTween).
The progress is saved between sessions.
Use the minimum necessary set of graphics to evaluate it visually.
It is necessary for the mini-game to look good on the main aspect ratios of mobile devices: 19.5x9. 16x9. 4x3.
