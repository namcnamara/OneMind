using Godot;
using System;
using System.Threading.Tasks;

public partial class PricklyBlob : Enemy
{
	public float WanderTimer = 0f;
	public float WanderCooldown = 2f;
	public float DetectionRadius = 20f;
	public float SpikeRadius = .3f;
	
	// Spikeing variables
	public bool IsSpikeing = false;
	public float SpikeTimer = 0f;
	public float SpikeCooldown = 1f;
	public float SpikeAnim;
	public float SpikeChargeRadius = 2f;

	public override void _Ready()
	{
		TYPE = "prickly blob";
		base._Ready();
		// Ensure we get the RigidBody3D from the right node
		RigidBody = GetNode<RigidBody3D>("prickly_blob_rigid");
		RigidBody.BodyEntered += OnBodyEntered;
		animatedSprite = GetNode<AnimatedSprite3D>("prickly_blob_rigid/prickly_blob_anim");
		collider = GetNode<CollisionShape3D>("prickly_blob_rigid/prickly_blob_collide");
		collider.Shape.Margin = 0.05f;
		PlayerNode = GetTree().Root.FindChild("hugo", true, false) as Player;
		PlayerBody = GetTree().Root.FindChild("hugo_char", true, false) as HugoBody3d;
		animatedSprite.AnimationFinished += OnAnimationFinished;
		define_strategy();
		FullName = TYPE + " " + movement + " " + action;
	}

	public override void _PhysicsProcess(double delta)
	{
		//Updates current direction
		base._PhysicsProcess(delta);
		Vector3 direction = CurrentDirection;
		AnimateDirection(direction);
		if (IsDead)
			Die();
		if (CurrentDistanceToClosestFriend < SpikeRadius)
		{
			TakeDamage(Damage);
			closestFriend.TakeDamage(Damage);
		}
	}
	
	public override void define_strategy()
	{
		//Update strategies for movement
		movement = "chase";
		_movementStrategy = EnemyMovementStrategyRegistry.GetStrategy(movement);
		action = "spike";
		_actionStrategy = EnemyActionStrategyRegistry.GetStrategy(action);
	}
	
	public override RigidBody3D GetRigidBody()
	{
		if (RigidBody == null)
		{
			RigidBody = GetNode<RigidBody3D>("prickly_blob_rigid");
		}
		return RigidBody;
	}

	public void AnimateDirection(Vector3 direction)
	{
		animatedSprite.Play("default");
	}
}
