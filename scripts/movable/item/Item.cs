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
		var damageArea = GetNodeOrNull<Area3D>("RigidBody3D/Area3D");
		if (damageArea != null)
		{
			damageArea.BodyEntered += OnDamageAreaBodyEntered;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		spawnTimer -= (float)delta;
		if (spawnTimer > 0) return;
		base._PhysicsProcess(delta);

		Vector3 direction = (playerBody.GlobalPosition - GlobalPosition).Normalized();
		float distance = GlobalPosition.DistanceTo(playerBody.GlobalPosition);
		float speed = 3.0f;
		RigidBody.ApplyCentralForce(speed * direction);		
	}


	public RigidBody3D GetRigidBody()
	{
		return RigidBody;
	}
	
	private void OnDamageAreaBodyEntered(Node3D body)
{
	// Check if this is the player body
	if (body is HugoBody3d player)
	{
		GD.Print("Player entered item area.");

		if (TYPE == "goo")
		{
			GameManager.Instance.PlayerManager.goo_count += 1;
			GD.Print("Goo collected. Total: " + GameManager.Instance.PlayerManager.goo_count);
		}
		else if (TYPE == "mush")
		{
			GameManager.Instance.PlayerManager.mush_count += 1;
			GD.Print("Mush collected. Total: " + GameManager.Instance.PlayerManager.mush_count);
		}
		else if (TYPE == "berry")
		{
			GameManager.Instance.PlayerManager.berry_count += 1;
			GD.Print("Berry collected. Total: " + GameManager.Instance.PlayerManager.berry_count);
		}
		else
		{
			GameManager.Instance.PlayerManager.goo_count += 1;
			GD.Print("catchGoo collected. Total: " + GameManager.Instance.PlayerManager.goo_count);
		}
		QueueFree(); 
	}
}
	
}
