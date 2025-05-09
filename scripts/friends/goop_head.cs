using Godot;
using System;
using System.Threading.Tasks;

public partial class goop_head : RigidBody3D
{
	[Export] public float explodeRadius = .5f;
	[Export] public float detectionRadius = 10f;
	private bool hasExploded = false;
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
	private Vector3 lastDirection;
	private float health = 100;
	private int damage = 10;

	// Wandering timer 
	private float wanderTimer = 0f;
	private float wanderCooldown = 2f;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite3D>("goop_head_anim");
		animatedSprite.AnimationFinished += OnAnimationFinished;collisionShape = GetNode<CollisionShape3D>("goop_head_collide");
		collisionShape.Shape.Margin = 0.05f;
		player =  (HugoBody3d)GetTree().Root.FindChild("hugo_char", true, false);
		direction = new Vector3(1, 0, 0);
		lastDirection = direction;
	}

	public override void _PhysicsProcess(double delta)
	{
		AxisLockAngularY = true;
		Vector3 direction = choose_direction(delta);
		ExplodeWithDelay();
		if (health > 0)
		{
			//animate based on direction
			animate_direction(direction);
		}
		lastDirection = direction;
	}
	
	private Vector3 choose_direction(double delta)
	{
		float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
		bool in_chasing_distance = distanceToPlayer < detectionRadius;
		if (in_chasing_distance && player.state != "head")
		{
			direction = 1f * (player.GlobalPosition - GlobalPosition).Normalized();
		}
		else
		{
			wanderTimer -= (float)delta;  
			if (wanderTimer <= 0)
			{
				random_move();
				wanderTimer = wanderCooldown; 
			}
		}
		//Apply momementum in chosen direction
		if (direction.Length() > velocityThreshold) 
			{ApplyCentralForce(direction * moveSpeed);}
		else 	
			{LinearVelocity = LinearVelocity * dampeningFactor;}
		// Clamp velocity to max speed
		if (LinearVelocity.Length() > maxSpeed)
		{
			LinearVelocity = LinearVelocity.Normalized() * maxSpeed;
		}
		return direction;
	}
	
	public async Task ExplodeWithDelay()
	{
		float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
		if (distanceToPlayer < explodeRadius && player.state != "head")
		{
			health -= 100;
			animatedSprite.Play("explode");
			if (hasExploded == false)
				{
					hasExploded = true;
					player.take_damage(damage);
				}
			await Task.Delay(500);
			QueueFree();
		}
	}
	
	private void die()
	{	
		bool a = false;
		/*
		animatedSprite.Play("explode");
		await Task.Delay(500);
		QueueFree();
		*/
	}
	
	private void animate_direction(Vector3 direction)
	{
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
	
	private void random_move()
	{
		// Randomly change the direction vector in x and z (no movement in y)
		direction = new Vector3((float)(random.NextDouble() * 2 - 1), 0, (float)(random.NextDouble() * 2 - 1)).Normalized();
	}
	
	private void OnAnimationFinished()
	{
		if (animatedSprite.Animation == "explode")
		{
			QueueFree();
		}
	}
}
