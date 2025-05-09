using Godot;
using System;

public partial class red_cap : RigidBody3D
{
	// Physics properties
	[Export] public float bumpDistance = 1.0f;
	[Export] public float bumpStrength = 3.0f;
	[Export] public float detectionRadius = 5f;
	[Export] public float moveSpeed = 6f;
	[Export] public float velocityThreshold = 0.01f;
	[Export] public float maxSpeed = 2f;  
	[Export] public float dampeningFactor = 0.9f;  

	// Flags and placeholders
	private HugoBody3d player;  // Hugo's reference
	private bool chasing = false;
	private Random random = new Random();
	private Vector3 direction = Vector3.Zero;
	private Vector3 lastDirection = Vector3.Zero;

	private AnimatedSprite3D animatedSprite;
	private CollisionShape3D collisionShape;
	
	// Wandering timer
	private float wanderTimer = 0f;
	private float wanderCooldown = 5f;

	public float health = 100;
	public int damage = 10;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite3D>("red_cap_anim");
		collisionShape = GetNode<CollisionShape3D>("red_cap_collide");
		collisionShape.Shape.Margin = 0.05f;
		player =  (HugoBody3d)GetTree().Root.FindChild("hugo_char", true, false);
		direction = new Vector3(1, 0, 0);
	}

	public override void _PhysicsProcess(double delta)
	{
		
		AxisLockAngularY = true;
		float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
		bool in_chasing_distance = distanceToPlayer < detectionRadius;
		direction = control_movement(in_chasing_distance, delta);
		animate_red_cap(direction);
		lastDirection = direction;
		
	}

	private Vector3 random_move()
	{
		return new Vector3(
			(float)(random.NextDouble() * 2 - 1),
			0,
			(float)(random.NextDouble() * 2 - 1)
		).Normalized();
	}

	private void animate_red_cap(Vector3 dir)
	{
		if (LinearVelocity == Vector3.Zero)
		{
			if (lastDirection.X > 0)
				{animatedSprite.Play("sleep_right");}
			else
				{animatedSprite.Play("sleep_left");}
		}
		else if (dir.X > 0)
		{
			animatedSprite.Play("walk_right");
		}
		else if (dir.X < 0)
		{
			animatedSprite.Play("walk_left");
		}
		else
		{
			// Idle animation based on the last direction
			if (lastDirection.X > 0)
				animatedSprite.Play("idle_right");
			else
				animatedSprite.Play("idle_left");
		}
	}
	
	private Vector3 control_movement(bool in_chasing_distance, double delta)
	{
		
		if (in_chasing_distance && player.state != "hippo"){
			Mass = 1f;
			// Update the direction to move towards the current position of Hugo (not the initial one)
			direction = 3f * (player.GlobalPosition - GlobalPosition).Normalized();
		}
		else{
			Mass = 100f;
			LinearVelocity =  Vector3.Zero;
		}

		// Apply movement force
		if (direction.Length() > velocityThreshold){
			ApplyCentralForce(direction * moveSpeed);
		}
		else{
			LinearVelocity = LinearVelocity * dampeningFactor;
		}

		// Clamp velocity to max speed
		if (LinearVelocity.Length() > maxSpeed){
			LinearVelocity = LinearVelocity.Normalized() * maxSpeed;
		}
		lastDirection = direction;
		return direction;
	}
}
