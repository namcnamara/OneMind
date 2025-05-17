using Godot;
using System;
using System.Threading.Tasks;

public partial class GoopHead : Enemy
{
	public string EnemyName = "goop_chase";
	public float WanderTimer = 0f;
	public float WanderCooldown = 1f;
	[Export] public float DetectionRadius = 20f;
	[Export] public float ExplodeRadius = .5f;
	[Export] public float jabRadius = 1f;
	private bool isJabbing = false;
	private AnimatedSprite3D animatedSprite;
	public RigidBody3D RigidBody;  // This should reference goop_head_rigid
	private CollisionShape3D collider;
	private bool hasExploded = false;

	public override void _Ready()
	{
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
	}

	public override void _PhysicsProcess(double delta)
	{
		//Updates current direction
		base._PhysicsProcess(delta);
		Explode();
		Vector3 direction = CurrentDirection;
		AnimateDirection(direction);
	}
	
	public override void define_strategy()
	{
		int decider = random.Next(0, 10);
		//Update strategies for movement
		if (decider % 2 == 0)
			EnemyName = "goop_chase";
				
		else
			EnemyName = "goop_random";
		_movementStrategy = MovementStrategyRegistry.GetStrategy(EnemyName);
		//Add update Action
		string actionType = "explode";
		if (decider % 3 == 0)
		{
			actionType = "jab";
		}
		_actionStrategy = ActionStrategyRegistry.GetStrategy(actionType);
	}
	
	public RigidBody3D GetRigidBody()
	{
		if (RigidBody == null)
		{
			RigidBody = GetNode<RigidBody3D>("goop_head_rigid");
		}
		return RigidBody;
	}

	private void AnimateDirection(Vector3 direction)
	{
		if (!hasExploded)
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

	private void OnAnimationFinished()
	{
		if (animatedSprite.Animation == "explode")
			QueueFree();
			GD.Print("***************EXPLODE*****************");
	}
	
	private void Explode()
	{
		if (hasExploded) return;

		if (CurrentDistance < ExplodeRadius && PlayerBody.state != "head")
		{
			hasExploded = true; 
			health = 0;
			GD.Print("***************startEXPLODE*****************" + EnemyName);
			animatedSprite.Play("explode");
			PlayerBody.take_damage(damage);
				
			// Delay deletion to allow animation to play
			GetTree().CreateTimer(0.5f).Timeout += () =>
			{
				GD.Print("deleted");
				QueueFree();
			};
		}
	}
}
