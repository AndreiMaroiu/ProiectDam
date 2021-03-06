# Nera: Lost Among Dimensions

A mobile game made in Unity with C# for a college project.<br/>
Contributors: 
* Gociman Marius: GUI and user options
* Iancu Norbert: art, animation and sound
* Maroiu Andrei: level procedural generation, core gameplay and enemy logic

# About the game
The game is a rouge-like with turn based elements.

### The basics:
* The player must traverse multiple rooms in a random generated labyrinth in order to find the exits. 
* To make traversing easier, the player is aided by a map that reveals all explored rooms.
* Each room can have one or three dimensions which the player can switch between, in each dimension there are enemies or obstacles which can be killed, destroyed or avoided
* The player has a limited number of health points, energy points and bullets. If one of them reaches 0, then the game is over. 
* The player can collect different pick-ups to regenerate health, energy or bullets.
* Energy points are used when the player move or attacks.

### About Dimensions:
There are three in-game dimensions:
* Grass dimension
* Fire dimension
* Dungeon dimension

### About the enemies:
* The enemies will move one block towards the player when their turn come.
* If an enemy is close enough to the player, it will attack.

### Combat:
* The player can use a melee attack, that can hit all enemies around him. This attack uses energy points.
* The player can use a ranged attack (a gun), that can hit a single enemy, but inflicts more damage. This attack uses bullets.

### Pick-ups and Traps:
* The player can collect pick-ups, as stated earlier, to regenerate health, energy or bullets
* The player should avoid traps, or can even lure enemies into traps

### Special rooms
Special rooms are rooms that can spawn only once in the labyrinth. These rooms have a single dimension. Such rooms are:
* Chest room: a room that contains a chest that can give the player a random pick-up
* Potions room: a room that contains some random pick-ups
* Portal room: the exit

# Screenshots:

### Main Menu:
![Screenshot 2022-02-14 105001](https://user-images.githubusercontent.com/79592738/153831467-0942ed43-96af-4796-b348-fca495273088.png)

### Grass Dimension
![Screenshot 2022-02-14 105034](https://user-images.githubusercontent.com/79592738/153831352-e4e0b105-4ee7-4388-b290-b871afddd58d.png)

### Fire Dimension
![Screenshot 2022-02-14 105106](https://user-images.githubusercontent.com/79592738/153831498-6e5c82f9-ae14-473c-914b-13849c6cc694.png)

### Dungeon Dimension
![Screenshot 2022-02-14 105219](https://user-images.githubusercontent.com/79592738/153831529-fb38c2e2-aea5-4474-8298-f9fb79853efc.png)

### Navigation Map
![Screenshot 2022-02-14 105235](https://user-images.githubusercontent.com/79592738/153831710-7b62e2d6-dc85-4be1-9779-329219463345.png)

---

We still have ideas for the game, when the time will let us, we will add new features.
