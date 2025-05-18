using Godot;
using System;

public partial class tree_birch : RigidBody3D
{
	private Random random = new Random();
	private Vector3 lastDirection = Vector3.Zero;
	private AnimatedSprite3D animatedSprite;
	private CollisionShape3D collisionShape;
	private Vector3 direction;
	public float health = 100;
	
	public int sleep_timer = 10;
	

	public override void _Ready()
	{
		// Initialize the animated sprite and collision shape
		animatedSprite = GetNode<AnimatedSprite3D>("tree_birch_anim");
		collisionShape = GetNode<CollisionShape3D>("tree_birch_collide");
		int animationChoice = random.Next(1, 4); 
		
		//Determines speed for this animation so its varied a bit
		int slowdown = random.Next(1, 130);
		animatedSprite.SpeedScale = 2.0f * 0.01f * (float)slowdown;

	switch (animationChoice)
	{
		case 1:
			animatedSprite.FlipH = false;
			animatedSprite.Play("type_1");
			break;
		case 2:
			animatedSprite.FlipH = false;
			animatedSprite.Play("type_2");
			break;
		case 3:
			animatedSprite.FlipH = true;
			animatedSprite.Play("type_1");
			break;
		case 4:
			animatedSprite.FlipH = true;
			animatedSprite.Play("type_2");
			break;
	}
	}
}
