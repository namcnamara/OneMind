using Godot;
using System;

public partial class HugoBody3d: CharacterBody3D
{
	// VARIABLES###############################################################
	[Export]
	public float Speed = 5.0f;
	[Export]
	public float GRAVITY = -29.8f;
	[Export]
	public float HEALTH = 100;
	[Export]
	public float GOOP_MASS = 5;
	[Export]
	public bool ALIVE = true;
	public bool HEAD = false;
	private int jumpCount = 0;
	private int maxJumps = 2;
	[Export]
	private float jumpForce = 8.50f;
	
	private AnimatedSprite3D animatedSprite;
	
	private Vector3 lastDirection = Vector3.Zero;
	
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite3D>("hugo_anim");
	}
	
	
	public override void _PhysicsProcess(double delta)
	{
		if (IsOnFloor())
			jumpCount = 0;
		Vector3 gravity = add_gravity(delta);
		gravity = handle_jump(gravity);
		
		Vector3 direction = get_input_direction();
		if (direction != Vector3.Zero)
		{
			lastDirection = direction;
		}
		
		Vector3 velocity = move_basic(direction, gravity);
		animate_basic(direction);
	}

	public Vector3 get_input_direction()
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
		
		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
		}
		return direction;
	}

	public Vector3 move_basic(Vector3 direction, Vector3 gravity)
	{
		Vector3 velocity = Vector3.Zero;
		velocity.X = direction.X * Speed;
		velocity.Z = direction.Z * Speed;
		velocity.Y = gravity.Y;
		Velocity = velocity;
		MoveAndSlide();
		return velocity;
	}
	
	
	public void animate_basic(Vector3 direction)
	{
		if (direction.X > 0){animatedSprite.Play("walk_right");}
		else if (direction.X < 0){animatedSprite.Play("walk_left");}
		else if (direction.Z > 0){animatedSprite.Play("walk_front");}
		else if (direction.Z < 0){animatedSprite.Play("walk_back");}
		else
		{
			if (lastDirection.X > 0) {animatedSprite.Play("idle_right");}
			else if (lastDirection.X < 0) {animatedSprite.Play("idle_left");}
			else if (lastDirection.Z > 0) {animatedSprite.Play("idle_front");}
			else if (lastDirection.Z < 0) {animatedSprite.Play("idle_back");}
			else {animatedSprite.Play("idle_front");}
		}
	}
	
	public Vector3 add_gravity(double delta)
	{
		bool isOnFloor = IsOnFloor();
		Vector3 gravity = Velocity;
		if (!isOnFloor)
		{
			gravity.Y += GRAVITY * (float)delta;
		}
		else
		{
			gravity.Y = 0;
		}
		return gravity;
	}
	
	public Vector3 handle_jump(Vector3 gravity)
	{
		if (jumpCount < maxJumps && Input.IsActionJustPressed("jump"))
		{
			gravity.Y = jumpForce;
			jumpCount++;
		}
		return gravity;
	}
}
