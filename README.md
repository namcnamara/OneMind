This repository holds files for my CPSC 5270 Game Project.

To run the project you need Godot 4.4 installed with C# support.
  - Open Godot, and import the file.
  - Run project.godot.



The two main files are scenes and scripts. Each scene defines an object in godot. Most scenes have a Node3D as its root object for greater portability.
This project scripts try to make use of C# patterns:

Bridge:
 - Movable class extends from Node, and is used to allow pause access for all objects that have movement, or can cause damage.
 - TODO: This is so gameplay can be stopped/started at will.

Builders:
  - Level builders are used to create enemy/home/neutral floors.
  - Enemy builders allow flexibility in how a enemy behaves by changing its characteristics upon creation. 

Strategy:
  - Enemys use simple modulo operators to define which actions and movements are assigned to a specific

Decorator:
   - TODO: make leader enemy. On death, change the strategy of enemies to be idle/random. 


