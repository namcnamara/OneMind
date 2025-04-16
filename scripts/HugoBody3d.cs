using Godot;
using System;

public partial class HugoBody3d: CharacterBody3D
{
	// VARIABLES###############################################################
	[Export]
	public float Speed = 5.0f;
	[Export]
	public float GRAVITY = 9.8f;
	[Export]
	public float HEALTH = 100;
	[Export]
	public float GOOP_MASS = 5;
	[Export]
	public bool ALIVE = true;
	public bool HEAD = false;
	public f
	//Accesses the aniamtion object
	private AnimatedSprite3D animatedSprite;
	
	//Tracks last direction looked, default front
	private Vector3 lastDirection = Vector3.Zero;
	
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite3D>("hugo_anim");
	}
	
	
	public override void _PhysicsProcess(double delta)
	{
		//Apply constant gravity
		Vector3 direction = move_basic();
		animate_basic(direction);
	}
	
	
	public Vector3 move_basic()
	{
		Vector3 direction = Vector3.Zero;
		if (Input.IsActionPressed("move_left"))
				direction.X -= 1;
		if (Input.IsActionPressed("move_right"))
			direction.X += 1;
		if (Input.IsActionPressed("move_away"))
			direction.Z -= 1;
		if (Input.IsActionPressed("move_toward"))
			direction.Z += 1;
			
		// Create velocity from movement
		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			Velocity = direction * Speed;
			lastDirection = direction;
		}
		else
		{
			Velocity = Vector3.Zero;
		}
		MoveAndSlide();	
		return direction;
	}
	
	
	public void animate_basic(Vector3 direction)
	{
		if (direction.X > 0){animatedSprite.Play("walk_right");}
		else if (direction.X < 0){animatedSprite.Play("walk_left");}
		else if (direction.Z > 0){animatedSprite.Play("walk_front");}
		else if (direction.Z < 0){animatedSprite.Play("walk_back");}
		else
		// Idle depending on size
		{
			if (lastDirection.X > 0) {animatedSprite.Play("idle_right");}
			else if (lastDirection.X < 0) {animatedSprite.Play("idle_left");}
			else if (lastDirection.Z > 0) {animatedSprite.Play("idle_front");}
			else if (lastDirection.Z < 0) {animatedSprite.Play("idle_back");}
			else {animatedSprite.Play("idle_front");}
		}
	}
}
