using Godot;
using System;

public partial class HugoBody3d: CharacterBody3D
{
	// VARIABLES###############################################################
	[Export]
	public int HEALTH = 100;
	[Export]
	public int GLOOP_MASS = 5;
	private int GLOOP_MAX = 10;
	[Export]
	public bool ALIVE = true;
	public bool HEAD = false;
	private int jumpCount = 0;
	private int maxJumps = 2;
	
	// Physics stuff
	[Export]
	public float Speed = 5.0f;
	[Export]
	public float GRAVITY = -29.8f; //9.8 feels way to slow, unless we turn up hugo's mass gets turned up a bit
	[Export]
	private float jumpForce = 8.50f;
	private AnimatedSprite3D animatedSprite;
	private HudLayer hud;
	private Vector3 lastDirection = Vector3.Zero;
	
	
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite3D>("hugo_anim");
		hud = GetParent().GetNode<HudLayer>("HUDLayer");
		updateHUD();
		
		hud = GetParent().GetNode<HudLayer>("HUDLayer");
		hud.SetHealth(HEALTH);
		hud.SetGloop(GLOOP_MASS);
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
		// Move
		Vector3 velocity = move_basic(direction, gravity);
		animate_basic(direction);
	}
	
	public Vector3 get_input_direction()
	{ //This just defines how to handle  wasd movement
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
		{ //normalized diagonal movement isn't wonky
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
	{//Other animations are handled in the action specific function
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
	{//this tracks the number of jumps remaining, plays the animation for jumping, adds gravity
		if (jumpCount < maxJumps && Input.IsActionJustPressed("jump"))
		{
			gravity.Y = jumpForce;
			jumpCount++;
		}
		return gravity;
	}
	
	public void updateHUD()
	{
		hud.SetHealth(HEALTH);
		hud.SetGloop(GLOOP_MASS);
	}
	
	public void TakeDamage(int amount)
	{ // Called in reverse to heal
		int prev_health = HEALTH;
		HEALTH = Mathf.Max(0, HEALTH - amount);
		HEALTH = Mathf.Min(prev_health, HEALTH);
		updateHUD();
	}

	public void AddGloop(int amount)
	{// Called in reverse to remove gloops
		GLOOP_MASS += amount;
		if (GLOOP_MASS > GLOOP_MAX)
			GLOOP_MASS = GLOOP_MAX;
		else if (GLOOP_MASS < 0)
			GLOOP_MASS = 0;
		updateHUD();
	}
}
