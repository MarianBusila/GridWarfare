# GridWarfare: Turn-Based Strategy Game

GridWarfare is a turn-based strategy game developed using the Unity game engine, inspired by the principles taught in the comprehensive [Udemy course](https://www.udemy.com/course/unity-turn-based-strategy/). This engaging game offers players the opportunity to strategically control their units in a grid-based environment, where tactical decisions and resource management play a crucial role.

## Actions and Gameplay

In GridWarfare, each unit possesses a distinct set of actions that contribute to the depth of gameplay. Action points serve as a valuable resource, determining the number of actions a unit can perform in a turn. Efficient allocation of these points is essential, as once depleted, a unit cannot execute additional actions until the next turn. Units also feature health bars, and various actions inflict differing amounts of damage.

### Move Action

Units can navigate the grid in any direction within a specified range. The game employs the A* pathfinding algorithm to calculate the most efficient route from the unit to its designated destination. This strategic movement allows players to position their units optimally.

![Move Action GIF](images/move_action.gif)

### Sword Action

Close-quarters combat becomes a pivotal aspect of gameplay, with units capable of executing a powerful sword attack when in proximity to an enemy. This action inflicts maximum damage, offering players a formidable option for dispatching adversaries up close.

![Sword Action GIF](images/sword_action.gif)

### Shoot Action

Ranged combat is a vital element in GridWarfare. Units equipped with ranged weapons can target enemies from a distance, offering strategic advantages and diversifying the player's approach to engagements.

![Shoot Action GIF](images/shoot_action.gif)

### Grenade Action

Introducing an explosive element to the game, the grenade action delivers damage to multiple units within a specified range. Additionally, destructible objects, such as crates, are obliterated by the grenade blast, further influencing the battlefield dynamics.

![Grenade Action GIF](images/grenade_action.gif)

### Interact Action

Beyond combat, players can interact with various objects, such as opening doors, adding an exploratory and immersive dimension to the gameplay experience.

![Interact Action GIF](images/interact_action.gif)

## Turn-Based Mechanics

Once a player exhausts all action points for their units, their turn concludes, allowing the enemy AI to execute its actions. This alternating turn structure enhances the strategic depth of GridWarfare, requiring players to anticipate and react to their opponent's moves.

GridWarfare offers a compelling and immersive turn-based strategy experience, combining thoughtful planning, tactical decision-making, and engaging combat scenarios.