using Godot;
using System;

public partial class GoopHead : Enemy
{
	public override string EnemyName => "goop_chase";
	public float WanderTimer { get; set; } = 0f;
	[Export] public float DetectionRadius = 10f;
	[Export] public float ExplodeRadius = 2.5f;
	[Export] public int Damage = 10;

	public HugoBody3d Player { get; private set; }
	public Vector3 CurrentDirection { get; set; } = Vector3.Right;
	public Vector3 LastDirection { get; set; }
	public float CurrentDistance { get; set; } = 100f;

	private AnimatedSprite3D animatedSprite;
	private RigidBody3D rigidBody;  // This should reference goop_head_rigid
	private CollisionShape3D collider;
	private bool hasExploded = false;
	private float health = 100;

	public override void _Ready()
	{
		base._Ready();

		// Ensure we get the RigidBody3D from the right node
		rigidBody = GetNode<RigidBody3D>("goop_head_rigid");
		animatedSprite = GetNode<AnimatedSprite3D>("goop_head_rigid/goop_head_anim");
		collider = GetNode<CollisionShape3D>("goop_head_rigid/goop_head_collide");
		collider.Shape.Margin = 0.05f;

		Player = GetTree().Root.FindChild("hugo_char", true, false) as HugoBody3d;
		animatedSprite.AnimationFinished += OnAnimationFinished;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		AnimateDirection();
		CheckExplosion();
	}
	
	public RigidBody3D GetRigidBody()
	{
		if (rigidBody == null)
		{
			rigidBody = GetNode<RigidBody3D>("goop_head_rigid");
		}
		return rigidBody;
	}

	private void CheckExplosion()
	{
		
		if (CurrentDistance < ExplodeRadius && Player.state != "head")
		{
			if (!hasExploded)
			{
				hasExploded = true;
				health -= 100;
				animatedSprite.Play("explode");
				Player.take_damage(Damage);
				CallDeferred(nameof(QueueFree), 0.5f); // Delay removal
			}
		}
	}

	private void AnimateDirection()
	{
		if (CurrentDirection.X > 0) animatedSprite.Play("roll_right_head");
		else if (CurrentDirection.X < 0) animatedSprite.Play("roll_left_head");
		else if (CurrentDirection.Z < 0) animatedSprite.Play("roll_front_head");
		else if (CurrentDirection.Z > 0) animatedSprite.Play("roll_back_head");
		else animatedSprite.Play("idle_front_head");
	}

	private void OnAnimationFinished()
	{
		if (animatedSprite.Animation == "explode")
			QueueFree();
	}
}
