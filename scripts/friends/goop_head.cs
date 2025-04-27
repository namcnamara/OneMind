using Godot;
using System;

public partial class goop_head : RigidBody3D
{
	// Physics stuff
	[Export] public float bumpDistance = 1.0f;
	[Export] public float bumpStrength = 3.0f;
	[Export] public float detectionRadius = 10f;
	public float followDistance = 2f;
	[Export] public float moveSpeed = 60f;
	[Export] public float velocityThreshold = 0.01f;
	[Export] public float maxSpeed = 1f; 
	[Export] public float dampeningFactor = 0.9f;

	// Flags and Placeholders
	private HugoBody3d player;
	private bool chasing = false;
	private Random random = new Random();
	private AnimatedSprite3D animatedSprite;
	private CollisionShape3D collisionShape;
	private Vector3 direction;
	public float health = 100;

	// Wandering timer 
	private float wanderTimer = 0f;
	private float wanderCooldown = 2f;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite3D>("goop_head_anim");
		collisionShape = GetNode<CollisionShape3D>("goop_head_collide");
		collisionShape.Shape.Margin = 0.05f;
		player =  (HugoBody3d)GetTree().Root.FindChild("hugo_char", true, false);
		direction = new Vector3(1, 0, 0);
	}

	public override void _PhysicsProcess(double delta)
	{
		AxisLockAngularY = true;
		float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
		bool in_chasing_distance = distanceToPlayer < detectionRadius;
		if (in_chasing_distance && player.state != "head")
		{
			// Move toward the player, but apply negative direction to "chase"
			direction = 1f * (player.GlobalPosition - GlobalPosition).Normalized();
		}
		else
		{
			// If out of range, randomly wander
			wanderTimer -= (float)delta;  

			if (wanderTimer <= 0)
			{
				random_move();
				wanderTimer = wanderCooldown; 
			}
		}
		// Apply movement force
		if (direction.Length() > velocityThreshold) 
			{ApplyCentralForce(direction * moveSpeed);}
		else 	
			{LinearVelocity = LinearVelocity * dampeningFactor;}

		// Clamp velocity to max speed
		if (LinearVelocity.Length() > maxSpeed)
		{
			LinearVelocity = LinearVelocity.Normalized() * maxSpeed;
		}

		// Play idle animation when not moving
		animatedSprite.Play("rest_2");
	}

	private void random_move()
	{
		// Randomly change the direction vector in x and z (no movement in y)
		direction = new Vector3((float)(random.NextDouble() * 2 - 1), 0, (float)(random.NextDouble() * 2 - 1)).Normalized();
	}
	
	
}
