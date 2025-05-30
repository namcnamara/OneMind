using Godot;
using System;

public partial class GloopMinion : Friend
{
	public float WanderTimer = 0f;
	public float WanderCooldown = 1f;
	public float DetectionRadius = 20f;
	public float ExplodeRadius = 1f;
	public float BumpRadius = 0.3f;
	public bool hasExploded = false;

	// Bumping variables
	public bool IsBumping = false;
	public float BumpTimer = 0f;
	public float BumpCooldown = 1f;
	public float bumpAnim;
	public float BumpChargeRadius = 2f;

	public override void _Ready()
	{
		TYPE = "Minion";
		base._Ready();

		// Set up node references
		RigidBody = GetNode<RigidBody3D>("gloop_head_rigid");
		animatedSprite = GetNode<AnimatedSprite3D>("gloop_head_rigid/gloop_head_anim");
		collider = GetNode<CollisionShape3D>("gloop_head_rigid/gloop_head_collide");
		collider.Shape.Margin = 0.05f;

		// Set player references
		PlayerNode = GetTree().Root.FindChild("hugo", true, false) as Player;
		PlayerBody = GetTree().Root.FindChild("hugo_char", true, false) as HugoBody3d;
		animatedSprite.AnimationFinished += OnAnimationFinished;
		define_strategy();
	}

	public override void define_strategy()
	{
		TYPE = "gloop minion";
		movement = "follow";
		_movementStrategy = FriendMovementStrategyRegistry.GetStrategy(movement);
		action = "bump";
		_actionStrategy = FriendActionStrategyRegistry.GetStrategy(action);
		FullName = TYPE + " " + movement + " "+ action;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Vector3 direction = CurrentDirection;
		AnimateDirection(direction);
	}

	public void AnimateDirection(Vector3 direction)
	{
		if (!hasExploded && !IsBumping)
		{
			if (direction.X > 0) animatedSprite.Play("roll_right_head");
			else if (direction.X < 0) animatedSprite.Play("roll_left_head");
			else if (direction.Z < 0) animatedSprite.Play("roll_front_head");
			else if (direction.Z > 0) animatedSprite.Play("roll_back_head");
			else
			{
				if (LastDirection.X > 0) animatedSprite.Play("idle_right_head");
				else if (LastDirection.X < 0) animatedSprite.Play("idle_left_head");
				else if (LastDirection.Z < 0) animatedSprite.Play("idle_front_head");
				else if (LastDirection.Z > 0) animatedSprite.Play("idle_back_head");
				else animatedSprite.Play("idle_front_head");
			}
		}
	}
}
