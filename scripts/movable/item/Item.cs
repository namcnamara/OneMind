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

	public override void _Ready()
	{
		base._Ready();
	}
	
	public virtual RigidBody3D GetRigidBody()
	{
		GD.Print("Need to overwrite for this item");
		return null;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}
}
