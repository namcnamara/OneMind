using Godot;
using System;
using System.Linq;

public partial class Destroyable : Movable 
{
	//Destroyable characteristics
	private static string TYPE = "destroyable";
	public string FullName = "destroyable-plain";
	//destroyable basic stuff
	public int MaxHealth = 100;
	public int Health = 100;
	public bool IsDead = false;
	public float DetectionRadius = 20f;
	
	//Destroyable scene parts
	public AnimatedSprite3D animatedSprite;
	public RigidBody3D RigidBody;  
	public CollisionShape3D collider;
	
	//Destroyable healthbar
	public PackedScene HealthBar { get; set; }
	public HealthBar healthBarInstance;
	
	//location distance, and direction 
	public Vector3 CurrentLocation {get; set;} = Vector3.Zero;
	public float CurrentDistance { get; set; } = 100f; 
	
	public Random random { get; set; } = new Random();

	public override void _Ready()
	{
		base._Ready();
	}
	
	public virtual RigidBody3D GetRigidBody()
	{
		GD.Print("Need to overwrite for this destroyable");
		return null;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}
	
	public void Die(string animName = "die")
	{
		IsDead = true;
		animatedSprite.Play(animName);
	}
	
	private void AddHealthBar()
	{
		HealthBar = GD.Load<PackedScene>("res://scenes/controllers/health_bar.tscn");
		healthBarInstance = (HealthBar)HealthBar.Instantiate();
		collider.AddChild(healthBarInstance);
		healthBarInstance.Translate(new Vector3(0, 1.5f, 0));
		healthBarInstance.Visit(FullName, Health, MaxHealth); 
	}
	
	public void TakeDamage(int damageAmount)
	{
		Health -= damageAmount;
		if (Health < 0) Health = 0; 
		if (healthBarInstance != null)
		{
			healthBarInstance.Visit(FullName, Health, MaxHealth); 
		}
		if (Health <= 0)
		{
			IsDead = true;
		}
	}
	
	public void OnAnimationFinished()
	{
		if (animatedSprite.Animation == "die" || animatedSprite.Animation =="Explode")
		{ 
				GameManager.Instance.UnregisterMovable(this, TYPE);
				QueueFree();
				GD.Print(TYPE + "***************die*****************");
		}
	}
}
