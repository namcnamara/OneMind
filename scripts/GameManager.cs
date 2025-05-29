using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class GameManager : Node
{
	//GameManager is a singleton
	public static GameManager Instance { get; private set; }
	// Floor manager
	public FloorManager FloorManager { get; private set; }
	
	//Add a player manager next
	//Registries for enemy and friendly units
	public Dictionary<string, Enemy> EnemiesByID { get; private set; } = new();
	public Dictionary<Enemy, string> EnemyIDsByRef { get; private set; } = new();
	public IEnumerable<Enemy> GetAllEnemies() => EnemiesByID.Values;
	public Dictionary<string, Friend> FriendsByID { get; private set; } = new();
	public Dictionary<Friend, string> FriendIDsByRef { get; private set; } = new();
	public IEnumerable<Friend> GetAllFriends() => FriendsByID.Values;

	//Player related stuff
	public Player Player_Movable { get; set;}
	public HugoBody3d Player_Body { get; set; }
	public Vector3 Player_Location {get; set;} = Vector3.Zero;
	public int PlayerMaxHealth {get; set;} = 100;
	public int PlayerMaxGoop {get; set;} = 5;
	public List<string> PlayerTransforms = new List<string>();
	public bool IsDead {get; set;} = false;
	
	
	public override void _Ready()
	{
		Instance = this;
		PlayerTransforms.Add("head");
		FloorManager = FloorManager = GetNode<FloorManager>("FloorManager");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (FloorManager.GameIsPlaying)
		{
			check_game_processes(delta);
		}
		if (IsDead && !FloorManager.floorUnloaded)
		{
			FloorManager.UnloadFloor();
			FloorManager.floorUnloaded = true;
		}
	}
	
	public void check_game_processes(double delta)
	{
		if(Player_Body == null)
		{
			GD.Print("seting player");
		}
		Player_Location = Player_Body.GlobalPosition;
	}
	
	public Movable GetClosestEntity(Vector3 toPosition, string type = "enemy")
	{
		if (type == "enemy")
		{
			return EnemiesByID.Values
			.OrderBy(e => e.GlobalPosition.DistanceTo(toPosition))
			.FirstOrDefault();
		}
		
		else 
		{
			return FriendsByID.Values
			.OrderBy(e => e.GlobalPosition.DistanceTo(toPosition))
			.FirstOrDefault();
		}
	}
	
	public void RegisterMovable(Movable entity, string type = "enemy")
	{
		string id = Guid.NewGuid().ToString();
		if (type == "enemy")
		{	
			Enemy enemy = entity as Enemy;
			if (!EnemiesByID.ContainsKey(id))
			{
				EnemiesByID[id] = enemy;
				EnemyIDsByRef[enemy] = id;
				GD.Print($"Registered{enemy} at {id}");
			}
		}
		if (type == "friend")
			{
			if (!FriendsByID.ContainsKey(id))
			{
				Friend friend = entity as Friend;
				FriendsByID[id] = friend;
				FriendIDsByRef[friend] = id;
				GD.Print($"Registered{entity} at {id}");
			}
		}
	}

	public void UnregisterMovable(Movable entity, string type = "enemy")
	{
		if (type == "enemy")
		{
			Enemy enemy = entity as Enemy;
			if (EnemyIDsByRef.TryGetValue(enemy, out var id))
			{
				EnemiesByID.Remove(id);
				EnemyIDsByRef.Remove(enemy);
				GD.Print($"Unregistered{entity} at {id}");
			}
		}
		if (type == "friend")
		{
			Friend friend = entity as Friend;
			if (FriendIDsByRef.TryGetValue(friend, out var id))
			{
				FriendsByID.Remove(id);
				FriendIDsByRef.Remove(friend);
				GD.Print($"Unregistered{entity} at {id}");
			}
		}
	}
}
