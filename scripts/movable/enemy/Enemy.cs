using Godot;
using System;

public partial class Enemy : Movable 
{
	public virtual string EnemyName => "generic";
	private MovementStrategy _movementStrategy;

	public override void _Ready()
	{
		base._Ready();

		// Use EnemyName as the key to get a strategy
		_movementStrategy = MovementStrategyRegistry.GetStrategy(EnemyName);

		if (_movementStrategy == null)
		{
			GD.PrintErr($"No strategy found for enemy: {EnemyName}");
		}
		else
		{
			GD.Print($"Strategy '{EnemyName}' assigned to {Name}");
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
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
