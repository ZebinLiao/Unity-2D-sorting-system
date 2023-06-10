# Unity-2D-sorting-system
A tile based y axis sorting system for characters in Unity 2D, (contains a movement system)

To summarise what this script does, when making a 2d tile based game, to make an object render on top of another object, y axis sorting has o take place.

- This script determins which tiles are to be rendered in front or behind the character. 
- After this is deterined, it puts the tile it wants to render on top in a seperate tile map on a render player in front of the player. 
- this happens every time the player walks on top of another tile.
- This makes it so that the player can be onstructed by a tile.

This script also handles collision between the player and the tiles, however it will likely be moved to another script in the future.

this script also includes a part for character movement, but it can and probably will be moved to another script in the future.
