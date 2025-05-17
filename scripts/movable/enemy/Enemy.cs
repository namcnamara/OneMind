using Godot;
using System;

public partial class Enemy : Movable 
{
	//enemy basic stuff
	public virtual string EnemyName { get; set; } = "generic";
	public int health = 100;
	public int damage = 10;
	public bool isDead = false;
	
	//Pointer to the player 
	public Player PlayerNode { get; set; }
	public HugoBody3d PlayerBody { get; set; }
	
	//location distance, and direction 
	public Vector3 CurrentDirection { get; set; } = Vector3.Right;
	public Vector3 LastDirection { get; set; }
	public Vector3 CurrentLocation {get; set;} = Vector3.Zero;
	public float CurrentDistance { get; set; } = 100f; //tracks distance to player
	
	//strategies
	public MovementStrategy _movementStrategy { get; set; }
	public ActionStrategy _actionStrategy { get; set; }
	
	//random generation needs to be centarlized but we failed hard the first time
	public Random random { get; set; } = new Random();

	public override void _Ready()
	{
		//variable defaults are updated in child class define_strategy()
		base._Ready();
		_movementStrategy = MovementStrategyRegistry.GetStrategy(EnemyName);
		_actionStrategy = ActionStrategyRegistry.GetStrategy(EnemyName);
	}
	
	//Assign strategy must be defined in the child
	public virtual void define_strategy()
	{
		EnemyName = EnemyName;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (!isPaused)
		{
			CurrentLocation = this.GlobalPosition;
			if (PlayerNode != null)
			{
				CurrentDistance = CurrentLocation.DistanceTo(GameManager.Instance.Player_location);
			}
			UpdateMovement(delta); 
			UpdateAction(delta);
		}
	}

	protected void UpdateMovement(double delta)
	{
		_movementStrategy.Move(this, EnemyName, delta);
	}
	
	protected void UpdateAnimation(double delta)
	{
		GD.Print("add anmation registry");
	}
	protected void UpdateAction(double delta)
	{
		_actionStrategy.Act(this, EnemyName, delta);
	}
}
