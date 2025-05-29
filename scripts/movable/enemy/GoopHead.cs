using Godot;
using System;
using System.Threading.Tasks;

public partial class GoopHead : Enemy
{
	public float WanderTimer = 0f;
	public float WanderCooldown = 1f;
	public float DetectionRadius = 20f;
	public float ExplodeRadius = 1f;
	public float BumpRadius = .3f;
	public bool hasExploded = false;
	
	// Bumping variables
	public bool IsBumping = false;
	public float BumpTimer = 0f;
	public float BumpCooldown = 1f;
	public float bumpAnim;
	public float BumpChargeRadius = 2f;

	public override void _Ready()
	{
		TYPE = "goop";
		base._Ready();
		// Ensure we get the RigidBody3D from the right node
		RigidBody = GetNode<RigidBody3D>("goop_head_rigid");
		animatedSprite = GetNode<AnimatedSprite3D>("goop_head_rigid/goop_head_anim");
		collider = GetNode<CollisionShape3D>("goop_head_rigid/goop_head_collide");
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
	}
	
	public override void define_strategy()
	{
		int decider = random.Next(0, 10);
		//Update strategies for movement
		if (decider % 2 == 0)
			movement = "chase";
				
		else
			movement = "random";
		_movementStrategy = EnemyMovementStrategyRegistry.GetStrategy(movement);
		//Add update Action
		action = "Explode";
		if (decider % 3 == 0)
		{
			action = "bump";
		}
		_actionStrategy = EnemyActionStrategyRegistry.GetStrategy(action);
	}
	
	public override RigidBody3D GetRigidBody()
	{
		if (RigidBody == null)
		{
			RigidBody = GetNode<RigidBody3D>("goop_head_rigid");
		}
		return RigidBody;
	}

	public void AnimateDirection(Vector3 direction)
	{
		if (!hasExploded && !IsBumping)
			{if (direction.X > 0){animatedSprite.Play("roll_right_head");}
			else if (direction.X < 0){animatedSprite.Play("roll_left_head");}
			else if (direction.Z < 0){animatedSprite.Play("roll_front_head");}
			else if (direction.Z > 0){animatedSprite.Play("roll_back_head");}
			else
			{
				if (LastDirection.X > 0) {animatedSprite.Play("idle_right_head");}
				else if (LastDirection.X < 0) {animatedSprite.Play("idle_left_head");}
				else if (LastDirection.Z < 0) {animatedSprite.Play("idle_front_head");}
				else if (LastDirection.Z > 0) {animatedSprite.Play("idle_back_head");}
				else {animatedSprite.Play("idle_front_head");}
			}
		}
	}
}
