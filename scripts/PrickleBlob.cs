using Godot;
using System;

public partial class PrickleBlob : RigidBody3D
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
	private AnimatedSprite3D animatedSprite;
	private CollisionShape3D collisionShape;
	private Vector3 direction;
	public float health = 100;
	

	public override void _Ready()
	{
		// Initialize the animated sprite and collision shape
		animatedSprite = GetNode<AnimatedSprite3D>("blob");
		collisionShape = GetNode<CollisionShape3D>("p_blob_collide");
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
		}
		else
		{
			random_move();
		}

		// Update position manually based on the movement speed
		ApplyCentralForce(direction * moveSpeed);

		// Optionally change direction randomly
		animatedSprite.Play("rest_2");
	}

	private void random_move()
	{
		// Randomly change the direction vector in x and z
		direction = new Vector3((float)(random.NextDouble() * 2 - 1), 0, (float)(random.NextDouble() * 2 - 1)).Normalized();
	}
	
	
	private void blob_attack()
	{
		//Decide which side hugo is on
		// use left or right attack depending
		//move away
	}
}
