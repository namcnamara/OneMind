using Godot;
using System;

public partial class LichenLounge : RigidBody3D
{
	// Building for Home. Increases goop from 5 to 10 if loaded in home node.
	private Random random = new Random();
	private AnimatedSprite3D animatedSprite;
	private CollisionShape3D collisionShape;
	private bool isOccupied = false;
	
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite3D>("LichenLoungeAnim");
		collisionShape = GetNode<CollisionShape3D>("LichenLoungeCollide");
		animatedSprite.Play("idle");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (isOccupied)
			animatedSprite.Play("occupied");
	}
}
