using Godot;
using System;

public partial class Enemy : Movable 
{
	public virtual string EnemyName { get; set; } = "generic";
	public int health = 100;
	public int damage = 10;
	public bool isDead = false;
	public Player PlayerNode { get; set; }
	public HugoBody3d PlayerBody { get; set; }
	public Vector3 CurrentDirection { get; set; } = Vector3.Right;
	public Vector3 LastDirection { get; set; }
	public Vector3 CurrentLocation {get; set;} = Vector3.Zero;
	public float CurrentDistance { get; set; } = 100f; //tracks distance to player
	public MovementStrategy _movementStrategy { get; set; }
	public Random random { get; set; } = new Random();

	public override void _Ready()
	{
		base._Ready();
		_movementStrategy = MovementStrategyRegistry.GetStrategy(EnemyName);
	}
	
	//Assign strategy must be defined in the child
	public virtual void define_strategy()
	{
		EnemyName = EnemyName;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		CurrentLocation = this.GlobalPosition;
		if (PlayerNode != null)
		{
			CurrentDistance = CurrentLocation.DistanceTo(GameManager.Instance.Player_location);
		}
		UpdateMovement(delta); 
		//UpdateAnimation(EnemyName);
		//UpdateAction(EnemyName);
	}

	protected void UpdateMovement(double delta)
	{
		_movementStrategy.Move(this, EnemyName, delta);
	}
	
	protected void UpdateAnimation(string EnemyName)
	{
		GD.Print("add anmation registry");
	}
	protected void UpdateAction(string EnemyName)
	{
		GD.Print("add action registry");
	}
}
