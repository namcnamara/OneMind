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
	public string state = "head";
	private bool isStuck = false;
	private float transform_timer = 0.0f;
	
	// Physics stuff
	[Export]
	public float Speed = 6.0f;
	[Export]
	public float GRAVITY = -29.8f; //9.8 feels way to slow, unless we turn up hugo's mass gets turned up a bit
	[Export]
	private float jumpForce = 8.50f;
	private AnimatedSprite3D animatedSprite;
	private HudLayer hud;
	private Vector3 lastDirection = Vector3.Zero;
	
	private ShaderMaterial damageShader;
	private float flashTimer = 0.0f;
	private float flashDuration = 0.2f;
	

	public override void _Ready()
	{
		// Call a method after a short delay to ensure GameManager is initialized
		CallDeferred("RegisterPlayerInGameManager");
		animatedSprite = GetNode<AnimatedSprite3D>("hugo_anim");
		damageShader = (ShaderMaterial)animatedSprite.MaterialOverride;
		hud = GetParent().GetNode<HudLayer>("HUDLayer");
		updateHUD();
		isStuck = false;
	}

	private void RegisterPlayerInGameManager()
	{
		if (GameManager.Instance == null)
		{
			GD.PrintErr("GameManager.Instance is null!");
			return;
		}
		GameManager.Instance.Player_body = this;
		GD.Print("Player registered in GameManager.");
	}
	
	
	public override void _PhysicsProcess(double delta)
	{
		if (HEALTH <= 0)
		{
			die();
		}
		else
		{
			handle_shaders(delta);
			handle_transform();

			// Add other time-taking actions like spawning goops
			if (isStuck)
			{
				transform_timer -= (float)delta;
				if (transform_timer > 0)
					return;
				else
					isStuck = false;
			}

			if (IsOnFloor())
				jumpCount = 0;

			Vector3 gravity = add_gravity(delta);
			gravity = handle_jump(gravity);

			Vector3 direction = get_input_direction();
			if (direction != Vector3.Zero)
				lastDirection = direction;

			Vector3 velocity = move_basic(direction, gravity);

			if (state == "head")
				animate_head(direction);
			else if (state == "hugo")
				animate_hugo(direction);
			else if (state == "hippo")
				animate_hippo(direction);
		}

		updateHUD();
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
	
	public void handle_shaders(double delta)
	{
		if (flashTimer > 0.0f)
			{
				flashTimer -= (float)delta;
				if (flashTimer <= 0.0f)
				{
					damageShader.SetShaderParameter("flash_strength", 4.0f);
				}
			}
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
	
	public void animate_hugo(Vector3 direction)
	{//Facings for hugo
		//If there is some minimum velocity:
		if (Math.Abs(direction.X) + Math.Abs(direction.Z) > 0.3f)
		{
			if (direction.X > 0){animatedSprite.Play("walk_right_hugo");}
			else if (direction.X < 0){animatedSprite.Play("walk_left_hugo");}
			else if (direction.Z > 0){animatedSprite.Play("walk_front_hugo");}
			else if (direction.Z < 0){animatedSprite.Play("walk_back_hugo");}
		}
		else
		{
			if (lastDirection.X > 0) {animatedSprite.Play("idle_right_hugo");}
			else if (lastDirection.X < 0) {animatedSprite.Play("idle_left_hugo");}
			else if (lastDirection.Z > 0) {animatedSprite.Play("idle_front_hugo");}
			else if (lastDirection.Z < 0) {animatedSprite.Play("idle_back_hugo");}
			else {animatedSprite.Play("idle_front_hugo");}
		}
	}
	
	public void animate_head(Vector3 direction)
	{//Other animations are handled in the action specific function
		if (direction.X > 0){animatedSprite.Play("roll_right_head");}
		else if (direction.X < 0){animatedSprite.Play("roll_left_head");}
		else if (direction.Z < 0){animatedSprite.Play("roll_front_head");}
		else if (direction.Z > 0){animatedSprite.Play("roll_back_head");}
		else
		{
			if (lastDirection.X > 0) {animatedSprite.Play("idle_right_head");}
			else if (lastDirection.X < 0) {animatedSprite.Play("idle_left_head");}
			else if (lastDirection.Z < 0) {animatedSprite.Play("idle_front_head");}
			else if (lastDirection.Z > 0) {animatedSprite.Play("idle_back_head");}
			else {animatedSprite.Play("idle_front_head");}
		}
	}
	
	public void animate_hippo(Vector3 direction)
	{//Other animations are handled in the action specific function
		//If there is some minimum velocity:
		if (Math.Abs(direction.X) + Math.Abs(direction.Z) > 0.3f)
		{
			if (direction.X > 0){
				animatedSprite.FlipH = true;
				animatedSprite.Play("walk_left_hippo");}
			else if (direction.X < 0){
				animatedSprite.FlipH = false;
				animatedSprite.Play("walk_left_hippo");}
			else if (direction.Z > 0){animatedSprite.Play("walk_front_hippo");}
			else if (direction.Z < 0){animatedSprite.Play("walk_back_hippo");}
		}
		else
		{
			if (lastDirection.X > 0) {
				animatedSprite.FlipH = true;
				animatedSprite.Play("idle_left_hippo");}
			else if (lastDirection.X < 0) {
				animatedSprite.FlipH = false;
				animatedSprite.Play("idle_left_hippo");}
			else if (lastDirection.Z > 0) {animatedSprite.Play("idle_front_hippo");}
			else if (lastDirection.Z < 0) {animatedSprite.Play("idle_back_hippo");}
			else {animatedSprite.Play("idle_front_hippo");}
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
		hud.UpdateHealth(HEALTH);
		hud.UpdateGloop(GLOOP_MASS);
	}
	
	public void handle_transform()
	{
		if (state == "head" && Input.IsActionJustPressed("transform_hugo") && GLOOP_MASS >= 1)
		{
			state = "hugo";
			HEALTH -= 10;
			GLOOP_MASS -= 1;
			Speed = 5.0f;
			animatedSprite.Play("head_to_hugo");
			transform_timer = 1.0f;
			isStuck = true;
		}
		else if (state == "head" && Input.IsActionJustPressed("transform_hippo") && GLOOP_MASS >= 2)
		{
			state = "hippo";
			HEALTH -= 15;
			GLOOP_MASS -= 2;
			Speed = 3.0f;
			animatedSprite.Play("head_to_hippo");
			transform_timer = 2.0f;
			isStuck = true;
		}
		else if (state == "hugo" && Input.IsActionJustPressed("transform_hugo"))
		{
			state = "head";
			HEALTH += 5;
			Speed = 6.0f;
			animatedSprite.Play("hugo_to_head");
			transform_timer = 1.0f;
			isStuck = true;
		}
		else if (state == "hippo" && Input.IsActionJustPressed("transform_hippo"))
		{
			state = "head";
			HEALTH += 5;
			Speed = 6.0f;
			animatedSprite.Play("hippo_to_head");
			transform_timer = 1.0f;
			isStuck = true;
		}
	}
	
	public void take_damage(int damage)
	{
		HEALTH -= damage;
		if (HEALTH < 0)
			die();
		else
		{
			hud.UpdateHealth(HEALTH);
			// Flash red to show damage using dmg shader
			flashTimer = flashDuration;
			damageShader.SetShaderParameter("flash_strength", 2.0f);
		}
	}
	
	public void heal(int heal)
	{
		// Flash blue to show damage using heal shader
	}
	
	public void die()
	{
		animatedSprite.Play("die");
		// Load into base
	}
}
