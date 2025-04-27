using Godot;
using System;

public partial class goop_head : RigidBody3D
{
	// Physics properties
	[Export] public float bumpDistance = 1.0f;
	[Export] public float bumpStrength = 3.0f;
	[Export] public float detectionRadius = 3f;
	[Export] public float moveSpeed = 6f;
	[Export] public float velocityThreshold = 0.01f;
	[Export] public float maxSpeed = 2f;  
	[Export] public float dampen_factor = 0.01f;  

	// Flags and placeholders
	private Node3D player;  // Hugo's reference
	private bool following = false;
	private Random random = new Random();
	private Vector3 direction = Vector3.Zero;
	private Vector3 lastDirection = Vector3.Zero;

	private AnimatedSprite3D animatedSprite;
	private CollisionShape3D collisionShape;
	
	// Wandering timer
	private float wanderTimer = 0f;
	private float wanderCooldown = 5f;

	public float health = 100;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite3D>("goop_head_anim");
		collisionShape = GetNode<CollisionShape3D>("goop_head_collide");
		collisionShape.Shape.Margin = 0.05f;
		player =  (Node3D)GetTree().Root.FindChild("hugo_char", true, false);
		direction = new Vector3(1, 0, 0);
	}

	public override void _PhysicsProcess(double delta)
{
	AxisLockAngularY = true;
	float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
	bool in_following_distance = distanceToPlayer < detectionRadius;
	direction = control_movement(in_following_distance, delta);
	animate_goop_head(direction);
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

	private void animate_goop_head(Vector3 dir)
	{	
		float min_force = 0.5f;
		if (Math.Abs(LinearVelocity.Z) > Math.Abs(LinearVelocity.X))
		{	
			if (Math.Abs(LinearVelocity.Z) > min_force)
			{
				if (LinearVelocity.Z > 0){animatedSprite.Play("roll_back_head");}
				else if (LinearVelocity.Z < 0){animatedSprite.Play("roll_front_head");}
			}
		}
		// If theres a reasonable abount of movement in x
		else if (Math.Abs(LinearVelocity.X) > min_force)
		{
			if (LinearVelocity.X > 0){animatedSprite.Play("roll_right_head");}
			else if (LinearVelocity.X < 0){animatedSprite.Play("roll_left_head");}
		}
		else
		{
			if (lastDirection.X > 0) {animatedSprite.Play("idle_right_head");}
			else if (lastDirection.X < 0) {animatedSprite.Play("idle_left_head");}
			else if (lastDirection.Z > 0) {animatedSprite.Play("idle_front_head");}
			else if (lastDirection.Z < 0) {animatedSprite.Play("idle_back_head");}
			else {animatedSprite.Play("idle_front_head");}
		}
	}
	
	private Vector3 control_movement(bool in_following_distance, double delta)
	{
		
		if (in_following_distance){
			//ADD MOVE TOWARD ENEMY
			LinearVelocity *= dampen_factor;
			random_move();
		}
		else{
			Mass = 1f;
			// Update the direction to move towards the current position of Hugo (not the initial one)
			direction = 3f * (player.GlobalPosition - GlobalPosition).Normalized();
		}

		// Apply movement force
		if (direction.Length() > velocityThreshold){
			ApplyCentralForce(direction * moveSpeed);
		}
		else{
			LinearVelocity = LinearVelocity * dampen_factor;
		}

		// Clamp velocity to max speed
		if (LinearVelocity.Length() > maxSpeed){
			LinearVelocity = LinearVelocity.Normalized() * maxSpeed;
		}
		lastDirection = direction;
		return direction;
	}
}
