using Godot;
using System;
using System.Linq;

public partial class Enemy : Movable 
{
	//Enemy characteristics
	private static string TYPE = "enemy";
	public string FullName = "enemy-plain";
	//enemy basic stuff
	public int MaxHealth = 100;
	public int Health = 100;
	public int Damage = 10;
	public bool IsDead = false;
	public bool IsDying = false;
	public float WanderTimer = 0f;
	public float WanderCooldown = 1f;
	public float DetectionRadius = 20f;
	
	//reference to player
	public Player PlayerNode { get; set; }
	public HugoBody3d PlayerBody { get; set; }
	
	//Enemy scene parts
	public AnimatedSprite3D animatedSprite;
	public RigidBody3D RigidBody;  
	public CollisionShape3D collider;
	
	//Enemy healthbar
	public PackedScene HealthBar { get; set; }
	public HealthBar healthBarInstance;
	
	//location distance, and direction 
	public Vector3 CurrentDirection { get; set; } = Vector3.Right;
	public Vector3 LastDirection { get; set; }
	public Vector3 CurrentLocation {get; set;} = Vector3.Zero;
	public float CurrentDistance { get; set; } = 100f; //tracks distance to player
	public float CurrentDistanceToClosestFriend = 1000f;
	public Friend closestFriend = null;
	
	//strategies
	public string movement = "";
	public string action = "";
	public EnemyMovementStrategy _movementStrategy { get; set; }
	public EnemyActionStrategy _actionStrategy { get; set; }
	public Random random { get; set; } = new Random();

	public override void _Ready()
	{
		//variable defaults are updated in child class define_strategy()
		base._Ready();
		_movementStrategy = EnemyMovementStrategyRegistry.GetStrategy(TYPE);
		_actionStrategy = EnemyActionStrategyRegistry.GetStrategy(TYPE);
		GameManager.Instance.RegisterMovable(this, TYPE);
	}
	
	public virtual RigidBody3D GetRigidBody()
	{
		GD.Print("Need to overwrite for this enemy");
		return null;
	}
	
	//Assign strategy must be defined in the child
	public virtual void define_strategy()
	{
		TYPE = TYPE;
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
				CurrentDistance = CurrentLocation.DistanceTo(GameManager.Instance.PlayerManager.Player_Location);
				if (GameManager.Instance.FriendsByID.Any())
				{
					Vector3 playerPosition = GameManager.Instance.PlayerManager.Player_Body.GlobalPosition;
					closestFriend = GameManager.Instance.GetClosestEntity(playerPosition, "friend") as Friend;
					CurrentDistanceToClosestFriend = CurrentLocation.DistanceTo(closestFriend.CurrentLocation);
				}
				else
				{
					closestFriend = null;
					CurrentDistanceToClosestFriend = 1000f;
				}
				
			}
			UpdateMovement(delta); 
			UpdateAction(delta);
		}
		if (IsDead)
		{
			Die();
		}
	}
	
	public void Die(string animName = "die")
	{
		IsDying = true;
		animatedSprite.Play(animName);
	}

	protected void UpdateMovement(double delta)
	{
		_movementStrategy.Move(this, TYPE, delta);
	}
	
	protected void UpdateAnimation(double delta)
	{
		GD.Print("add anmation registry");
	}
	
	protected void UpdateAction(double delta)
	{
		_actionStrategy.Act(this, TYPE, delta);
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
		if (animatedSprite.Animation == "bump")
		{ 
				if (Health < 0)
				//Start dying
					animatedSprite.Play("die");
		}
		GD.Print("Successful removal");
	}
	
	public void OnBodyEntered(Node body)
	{
		if (body is Friend friend)
		{
			GD.Print("Hit a friend!");
			friend.TakeDamage(Damage);  
		}
	}
}
