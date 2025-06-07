using Godot;
using System;
using System.Linq;

public partial class Item : Movable 
{
	//Item characteristics
	private static string TYPE = "item";
	public string FullName = "item-plain";
	//item basic stuff
	public int MaxHealth = 100;
	public int Health = 100;
	public float DetectionRadius = 1f;
	public float spawnTimer = 2f;
	
	//Item scene parts
	public AnimatedSprite3D animatedSprite;
	public RigidBody3D RigidBody;  
	public CollisionShape3D collider;
	
	//Item healthbar
	public PackedScene HealthBar { get; set; }
	public HealthBar healthBarInstance;
	
	//location distance, and direction 
	public Vector3 CurrentLocation {get; set;} = Vector3.Zero;
	public Random random { get; set; } = new Random();
	private HugoBody3d playerBody;

	public override void _Ready()
	{
		base._Ready();
		RigidBody = GetNode<RigidBody3D>("RigidBody3D");
		animatedSprite = RigidBody.GetNode<AnimatedSprite3D>("AnimatedSprite3D");
		playerBody = GameManager.Instance.PlayerManager.Player_Body;
	}

	public override void _PhysicsProcess(double delta)
	{
		spawnTimer -= (float)delta;
		if (spawnTimer > 0) return;
		base._PhysicsProcess(delta);

		Vector3 direction = (playerBody.GlobalPosition - GlobalPosition).Normalized();
		float distance = GlobalPosition.DistanceTo(playerBody.GlobalPosition);
		float speed = 3.0f;
		RigidBody.ApplyCentralForce((speed / distance) * direction);		
		if (distance < 0.4f)
		{
			OnCollected();
		}
	}

	private void OnCollected()
	{
		if (TYPE == "gloop")
			GameManager.Instance.PlayerManager.gloop_count += 1;
		else
			GameManager.Instance.PlayerManager.gloop_count += 1;
		QueueFree();
	}

	public RigidBody3D GetRigidBody()
	{
		return RigidBody;
	}
	
}
