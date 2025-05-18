using Godot;
using System;

public partial class BubbleHut : RigidBody3D
{
	// Building for Home. Increases goop from 5 to 10 if loaded in home node.
	private Random random = new Random();
	private AnimatedSprite3D animatedSprite;
	private CollisionShape3D collisionShape;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite3D>("BubbleHutAnim");
		collisionShape = GetNode<CollisionShape3D>("BubbleHutCollide");
		animatedSprite.Play("idle");
	}
}
