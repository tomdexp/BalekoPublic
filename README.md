# Baleko Source

## What is this repository ?

This repository is the public version of the source code of one of my Master 1's exercise (5 days) where we had to create a game with a mechanics linked to procedural generation (like Wave Function Collapse or Genetic Evolution).

It's the public version because I have trimmed all paid Plugins that should not be in the public repository.

IT WON'T COMPILE (Because there is some missing plugins)

## What is this project about ?

### The goal
For this project, we aimed to create a simplified simulation of the process of natural selection. We had 5 days.

### The rules
Each entity in the game has:

* A health bar,
* A hunger bar (which decreases over time and, when it reaches zero, causes death),
* Various "genes" that differ from one entity to another. (These genes determine multiple parameters such as movement speed, entity size, and several characteristics related to their ability to shoot.)
* When an entity dies, it drops food, which can replenish the hunger bar and is crucial for survival.

### Genes
Each gene is associated with a specific parameter of an entity, and genes can mutate, altering the value of that parameter. Most genes are paired (or opposed). For these, when one gene's value increases, the other's decreases, and vice versa. For example: the genes ProjectileSize and ProjectileSpeed are opposed — the larger a projectile is, the slower it moves, and the smaller it is, the faster it travels.

Here’s a list of the different genes and their effects:

* `GeneAttackSpeed`: Determines how fast the units can shoot. The higher the value, the faster they fire.
* `GeneHealth`: The health of the units.
* `GeneMovementSpeed`: The movement speed of the units.
* `GenePrecision`: The unit’s shooting accuracy. A higher value reduces the dispersion cone, making the unit more precise.
* `GeneProjectileCount`: The number of projectiles fired by the creature.
* `GeneProjectileLifespan`: How long a projectile lasts.
* `GeneProjectileSize`: The size of a projectile.
* `GeneProjectileSpeed`: The speed of a projectile.
* `GeneRotationSpeed`: The rotation speed of a unit.
* `GeneSize`: The size of a unit.
* `GeneVisionRange`: The distance of the unit’s vision cone.
* `GeneVisionWidth`: The width of the unit’s vision cone.

Here’s a list of opposed gene pairs:

* `GeneSize` VS `GeneHealth`
* `GeneAttackSpeed` VS `GenePrecision`
* `GeneProjectileCount` VS `GeneProjectileLifespan`
* `GeneProjectileSpeed` VS `GeneProjectileSize`
* `GeneVisionRange` VS `GeneVisionWidth`
* `GeneRotationSpeed` VS `GeneMovementSpeed`

### Gene Selection Process
The game revolves around generating a set number of entities with random parameters and observing which survive the longest. 
To avoid ending up with a single, overly specific entity profile, we introduced three fictional groups (this has no impact on gameplay — it's every entity for themselves).
After each round, the best survivors from these three groups are selected, and based on their slightly modified parameters, a new generation of each team is created. 
This process helps diversify the potential for an optimal survivor.

For example, you might see entities that shoot rapidly in all directions but only at short range, almost like flamethrowers, or profiles with a very wide field of vision, able to defend themselves and move freely in 360°.


## What did I do on this project ?
We were 3 programmers, 

I focused on the Genetics system that you can find at `Assets/_Project/Scripts/Genetics`.

I also implemented the Flyweight Factory pattern by creating object pooling for projectiles and collectibles and used XCharts to do the data visualisation.
