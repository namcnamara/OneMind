using Godot;
using System;

public partial class Enemy : Movable 
{
	//enemy basic stuff
	public virtual string EnemyName { get; set; } = "generic";
	public int MaxHealth = 100;
	public int Health = 100;
	public int Damage = 10;
	public bool IsDead = false;
	public bool IsDying = false;
	public Player PlayerNode { get; set; }
	public HugoBody3d PlayerBody { get; set; }
	
	public AnimatedSprite3D animatedSprite;
	public RigidBody3D RigidBody;  
	public CollisionShape3D collider;
	
	public PackedScene HealthBar { get; set; }
	public HealthBar healthBarInstance;
	
	//location distance, and direction 
	public Vector3 CurrentDirection { get; set; } = Vector3.Right;
	public Vector3 LastDirection { get; set; }
	public Vector3 CurrentLocation {get; set;} = Vector3.Zero;
	public float CurrentDistance { get; set; } = 100f; //tracks distance to player
	
	//strategies
	public string movement = "";
	public string action = "";
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
		GameManager.Instance.RegisterEnemy(this);
	}
	
	//Assign strategy must be defined in the child
	public virtual void define_strategy()
	{
		EnemyName = EnemyName;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (IsDying) return;
		base._PhysicsProcess(delta);
		if (Health < 0)
		{
			IsDead = true;
		}
		if (healthBarInstance == null){AddHealthBar();}
		if (!isPaused)
		{
			CurrentLocation = this.GlobalPosition;
			if (PlayerNode != null)
			{
				CurrentDistance = CurrentLocation.DistanceTo(GameManager.Instance.Player_Location);
			}
			UpdateMovement(delta); 
			UpdateAction(delta);
		}
		if (IsDead)
		{
			Die("Explode");
		}
	}
	
	public void Die(string animName = "die")
	{
		IsDying = true;
		animatedSprite.Play(animName);
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
	
	private void AddHealthBar()
	{
		HealthBar = GD.Load<PackedScene>("res://scenes/health_bar.tscn");
		healthBarInstance = (HealthBar)HealthBar.Instantiate();
		collider.AddChild(healthBarInstance);
		healthBarInstance.Translate(new Vector3(0, 1.5f, 0));
		healthBarInstance.Visit(this);
	}
	
	public void TakeDamage(int damageAmount)
	{
		Health -= damageAmount;
		if (Health < 0) Health = 0; 
		if (healthBarInstance != null)
		{
			GD.Print($"takedame {damageAmount}");
			healthBarInstance.Visit(this); 
		}
		if (Health <= 0)
		{
			IsDead = true;
		}
	}
	
	public void OnAnimationFinished()
	{
		if (animatedSprite.Animation == "Explode")
		{ 
			GameManager.Instance.UnregisterEnemy(this);
			QueueFree();
			GD.Print("***************Explode*****************");
		}
		if (animatedSprite.Animation == "die")
		{ 
				GameManager.Instance.UnregisterEnemy(this);
				QueueFree();
				GD.Print("***************die*****************");
		}
		if (animatedSprite.Animation == "bump")
		{ 
				if (Health < 0)
					QueueFree();
				GD.Print("***************die from bump*****************");
		}
	}
}
