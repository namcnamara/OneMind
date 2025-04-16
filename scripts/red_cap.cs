using Godot;
using System;

public partial class red_cap : RigidBody3D
{
	//Physics properties
	[Export] public float bumpDistance = 1.0f;
	[Export] public float bumpStrength = 3.0f;
	[Export] public float detectionRadius = 5f;
	[Export] public float moveSpeed = 1f;
	[Export] public float velocityThreshold = 0.01f;
	
	// Flags and Placeholders
	private Node3D player;
	private bool chasing = false;
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
		animatedSprite = GetNode<AnimatedSprite3D>("red_cap_anim");
		collisionShape = GetNode<CollisionShape3D>("red_cap_collide");
		collisionShape.Shape.Margin = 0.05f;
		
		player =  (Node3D)GetTree().Root.FindChild("hugo", true, false);

		// Initial direction of the blob
		direction = new Vector3(1, 0, 0);
	}

	public override void _PhysicsProcess(double delta)
	{
		float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
		bool in_chasing_distance = distanceToPlayer < detectionRadius;
		if (in_chasing_distance)
		{
			direction = (player.GlobalPosition - GlobalPosition).Normalized();
			animate_red_cap(direction);
			lastDirection = direction;
		}
		else
		{
			random_move();
		}

		// Update position manually based on the movement speed
		ApplyCentralForce(direction * moveSpeed);

		if (direction == Vector3.Zero && lastDirection == Vector3.Zero)
		{animatedSprite.Play("sleep_1");}
		
	}

	private void random_move()
	{
		// Randomly change the direction vector in x OR z
		// 
		// decider = random.nextDouble()
		direction = new Vector3((float)(random.NextDouble() * 2 - 1), 0, (float)(random.NextDouble() * 2 - 1)).Normalized();

	}
	
	public void animate_red_cap(Vector3 direction)
	{
		if (direction.X > 0){animatedSprite.Play("walk_right");}
		else if (direction.X < 0){animatedSprite.Play("walk_left");}
		else if (direction.Z > 0){animatedSprite.Play("walk_right");}
		else if (direction.Z < 0){animatedSprite.Play("walk_left");}
		else
		// Idle depending on size
		{
			if (lastDirection.X > 0) {animatedSprite.Play("idle_right");}
			else if (lastDirection.X < 0) {animatedSprite.Play("idle_left");}
			else {animatedSprite.Play("sleep_2");}
		}
	}
}
