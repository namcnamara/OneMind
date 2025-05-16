using Godot;
using System;

public partial class Player : Movable
{
	// Inherits from Movable, which defines objects with a position, velocity, and serves as a bridge that can be used to uniformly affect objects in the game.
	// This class is used to define the singleton player object in the game.
	public override void _Ready()
	{
		base._Ready();
		lastPosition = GlobalTransform.Origin;
		CurrentPosition = lastPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._Ready();
	}
}
