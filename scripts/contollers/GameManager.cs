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
	// Player manager
	public PlayerManager PlayerManager { get; private set; }
	
	//Registries for enemy and friendly units
	public Dictionary<string, Enemy> EnemiesByID { get; private set; } = new();
	public Dictionary<Enemy, string> EnemyIDsByRef { get; private set; } = new();
	public IEnumerable<Enemy> GetAllEnemies() => EnemiesByID.Values;
	
	public Dictionary<string, Friend> FriendsByID { get; private set; } = new();
	public Dictionary<Friend, string> FriendIDsByRef { get; private set; } = new();
	public IEnumerable<Friend> GetAllFriends() => FriendsByID.Values;
	
	public override void _Ready()
	{
		Instance = this;
		FloorManager = FloorManager = GetNode<FloorManager>("FloorManager");
		PlayerManager = GetNode<PlayerManager>("PlayerManager");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (PlayerManager.Player_Body == null)
			return;

		Vector3 playerPosition = PlayerManager.Player_Body.GlobalPosition;
		Movable closestEnemy = GetClosestEntity(playerPosition, "enemy");

		if (closestEnemy != null)
		{
			//GD.Print($"Closest enemy to player: {closestEnemy.TYPE} at {closestEnemy.GlobalPosition}");
		}
		else
		{
			GD.Print("No enemies present.");
		}
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
				GD.Print($"Registered{friend} at {id}");
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
	
	public void flush_registries()
	{
		foreach (var friend in FriendsByID.Values)
		{
			friend.QueueFree();
		}
		foreach (var enemy in EnemiesByID.Values)
		{
			enemy.QueueFree();
		}
		FriendsByID.Clear();
		FriendIDsByRef.Clear();	
		EnemiesByID.Clear();
		EnemyIDsByRef.Clear();
	}
}
