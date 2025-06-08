using Godot;
using System;

public partial class GloopMinion : Friend
{
	public float WanderTimer = 0f;
	public float WanderCooldown = 1f;
	public float DetectionRadius = 20f;
	public float ExplodeRadius = 1f;
	public bool hasExploded = false;

	// Explodeing variables
	public bool IsExplodeing = false;
	public float ExplodeTimer = 0f;
	public float ExplodeCooldown = 1f;
	public float bumpAnim;
	public float ExplodeChargeRadius = 2f;

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
		var damageArea = GetNodeOrNull<Area3D>("gloop_head_rigid/Area3D");
		if (damageArea != null)
		{
			damageArea.BodyEntered += OnDamageAreaBodyEntered;
		}
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
		AnimateDirection(CurrentDirection);
		if (Health < 0)
		{
			Die();
		}
	}

	public void AnimateDirection(Vector3 direction)
	{
		if (!hasExploded && !IsExplodeing)
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
	
	private void OnDamageAreaBodyEntered(Node3D body)
	{
		Enemy enemy = FindEnemyParent(body);

		if (enemy != null)
		{
			animatedSprite.Play("Explode");
			//Damage player and enemy
			enemy.TakeDamage(100);
			TakeDamage(100);
		}
	}

	private Enemy FindEnemyParent(Node currentNode)
	{
		while (currentNode != null)
		{
			if (currentNode is Enemy enemyNode)
				return enemyNode;

			currentNode = currentNode.GetParent();
		}
		return null;
	}
}
