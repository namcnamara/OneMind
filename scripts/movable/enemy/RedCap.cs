using Godot;
using System;
using System.Threading.Tasks;

public partial class RedCap : Enemy
{
	public float WanderTimer = 0f;
	public float WanderCooldown = .5f;
	public float DetectionRadius = 5f;
	public bool IsSleeping = false;
	
	// Bubbleing variables
	public bool IsBubbling = false;
	public int BubbleDamage = 1;
	public float BubbleTimer = 0f;
	public float BubbleDmgTimer = 0f;
	public float BubbleDmgCooldown = .2f;
	public float BubbleCooldown = 2f;
	public float BubbleAnim;
	public float BubbleRadius = 4f;
	public float BubbleDamageRadius = 2f;
	public Node3D bubbler;
	
	public override void _Ready()
	{
		TYPE = "red_cap";
		base._Ready();
		// Ensure we get the RigidBody3D from the right node
		RigidBody = GetNode<RigidBody3D>("red_cap_rigid");
		animatedSprite = GetNode<AnimatedSprite3D>("red_cap_rigid/red_cap_anim");
		collider = GetNode<CollisionShape3D>("red_cap_rigid/red_cap_collide");
		collider.Shape.Margin = 0.05f;
		PlayerNode = GetTree().Root.FindChild("hugo", true, false) as Player;
		PlayerBody = GetTree().Root.FindChild("hugo_char", true, false) as HugoBody3d;
		animatedSprite.AnimationFinished += OnAnimationFinished;
		bubbler = GetNode<Node3D>("red_cap_rigid/Bubbler"); 
		bubbler.Visible = false;
		
		define_strategy();
		FullName = TYPE + " " + movement + " " + action;
	}

	public override void _PhysicsProcess(double delta)
	{
		//Updates current direction
		base._PhysicsProcess(delta);
		Vector3 direction = CurrentDirection;
		AnimateDirection(direction);
		if (IsBubbling)
			{
				bubbler.Visible = true;
				TakeDamage(BubbleDamage * 1);
			}
	}
	
	public override void define_strategy()
	{
		movement = "sleepy";
		_movementStrategy = EnemyMovementStrategyRegistry.GetStrategy(movement);
		//Add update Action
		action = "bubble";
		_actionStrategy = EnemyActionStrategyRegistry.GetStrategy(action);
	}
	
	public override RigidBody3D GetRigidBody()
	{
		if (RigidBody == null)
		{
			RigidBody = GetNode<RigidBody3D>("red_cap_rigid");
		}
		return RigidBody;
	}

	public void AnimateDirection(Vector3 direction)
	{
		if (!IsBubbling)
			{if (direction.X > 0){animatedSprite.Play("walk_right");}
			else if (direction.X < 0){animatedSprite.Play("walk_left");}
			else
			{
				if (IsSleeping)
				{
					if (LastDirection.X > 0) {animatedSprite.Play("sleep_right");}
					else if (LastDirection.X < 0) {animatedSprite.Play("sleep_left");}
					else {animatedSprite.Play("idle_front_head");}
				}
				else
				{   
					if (LastDirection.X > 0) {animatedSprite.Play("idle_right");}
					else if (LastDirection.X < 0) {animatedSprite.Play("idle_left");}
					else {animatedSprite.Play("idle_front_head");}
				}
			}
		}
	}
}
