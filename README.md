This repository holds files for my CPSC 5270 Game Project.

To run the project you need Godot 4.4 installed with C# support.
  - Open Godot, and import the file.
  - Run project.godot.

The main scene is Master.tscn. It controls the switch between loading title and game scenes. 
  - When the button Play is pressed on the title scene, the game is started.
  - The home floor is loaded.
	  - TODO: MEMENTO to load state.
	 

Patterns used:
Singleton:
  - GameManager autoloads and is accessible as a global instance for all classes in the ecosystem.

Multiton:
  - Enemy instances created by builder classes are tracked by the singleton GameManager via a registry.

Bridge:
 - Movable class extends from Node, and is used to allow pause access for all objects that have movement, or can cause damage.
	 - Player extends Movable, used for player sync with the pause command.
	 - Enemy extends Movable, defines a controller script for enemy classes.
	   - Uses Builder pattern for action and movement assignment.
	   - Uses Strategy pattern for specific assignment. 

Builders:
  - Level builders are used to create enemy/home/neutral floors.
  - Enemy builders allow flexibility in how a enemy behaves by changing its characteristics upon creation. 

Strategy:
  - Enemys use simple modulo operators to define which actions and movements are assigned to a specific

Visitor:
  - Define access to HealthBar class through a visitor interface so it can be extended to other classes/control access. Made tracking/updating health per enemy way easier.

Decorator:
   - TODO: make leader enemy. On death, change the strategy of enemies to be idle/random. 
