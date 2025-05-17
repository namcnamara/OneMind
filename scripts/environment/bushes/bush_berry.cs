using Godot;
using System;

public partial class bush_berry : RigidBody3D
{
	
	private Random random = new Random();
	private Vector3 lastDirection = Vector3.Zero;
	private AnimatedSprite3D animatedSprite;
	private CollisionShape3D collisionShape;
	private Vector3 direction;
	public float health = 100;
	public bool has_berries = false;
	
	public int sleep_timer = 10;
	

	public override void _Ready()
	{
		
		// Initialize the animated sprite and collision shape
		animatedSprite = GetNode<AnimatedSprite3D>("bush_berry_anim");
		collisionShape = GetNode<CollisionShape3D>("bush_berry_collide");
		int animationChoice = random.Next(1, 19); 
		if (animationChoice > 14)
			has_berries = true;
		animationChoice = (animationChoice % 6) + 1;
		int slowdown = random.Next(1, 130);
		animatedSprite.SpeedScale = 2.0f * 0.01f * (float)slowdown;
		if (has_berries)
		{
			if (animationChoice % 2 == 0)
			{
				animatedSprite.FlipH = false;
				animatedSprite.Play("type_4");
			}
			else
			{
				animatedSprite.FlipH = true;
				animatedSprite.Play("type_4");
			}
		}
		else
		{
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
				case 5:
					animatedSprite.FlipH = false;
					animatedSprite.Play("type_3");
					break;
				case 6:
					animatedSprite.FlipH = true;
					animatedSprite.Play("type_3");
					break;
			}
		}
	}

	

	
}
