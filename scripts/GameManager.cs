using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class GameManager : Node
{
	//GameManager is a singleton
	public static GameManager Instance { get; private set; }
	
	//Floor related stuff
	public string CurrentFloor { get; set; } = "home";
	private Node currentFloorInstance;
	
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
	
	//Game scene contollers
	public bool IsPaused {get; set;} = false;
	private PackedScene pausePanelScene;
	private Control pausePanelInstance;
	public Button ToHomeButton {get; set;}
	public bool GameIsPlaying = false;
	public bool floorUnloaded = false;
	
	//Unlocked Home Benefits for stat and transform updates.
	public bool lichenLounge = false;
	public bool BubbleHut = false;
	
	public override void _Ready()
	{
		Instance = this;
		PlayerTransforms.Add("head");
		pausePanelScene = GD.Load<PackedScene>("res://scenes/Pause.tscn");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (GameIsPlaying)
		{
			check_game_processes(delta);
		}
		if (IsDead && !floorUnloaded)
		{
			UnloadFloor();
			floorUnloaded = true;
		}
	}
	
	public void check_game_processes(double delta)
	{
		if(Player_Body == null)
		{
			GD.Print("seting player");
		}
		Player_Location = Player_Body.GlobalPosition;
		//Handle Pausing
		handle_pause(delta);
		
	}
	
	public void handle_pause(double delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			TogglePause();
		}
	}
	
	public void LoadFloor(string buildType = "home")
	{
		UnloadFloor();
		var floorScene = GD.Load<PackedScene>("res://scenes/environment/levels/basic_floor.tscn");
		currentFloorInstance = floorScene.Instantiate<Node>();

		if (currentFloorInstance is basic_floor floor)
		{
			floor.BuildType = buildType;
		}

		var master = GetTree().Root.GetNode<Master>("Master");
		master.AddChild(currentFloorInstance);

		Player_Body = currentFloorInstance.GetNodeOrNull<HugoBody3d>("hugo/hugo_char");
		if (Player_Body != null)
		{
			Player_Location = Player_Body.GlobalPosition;
			GD.Print("Player_Body reference updated in GameManager.");
		}
	}
	
	public void UnloadFloor()
	{
		if (currentFloorInstance != null && currentFloorInstance.IsInsideTree())
		{
			currentFloorInstance.QueueFree();
			currentFloorInstance = null;
			GD.Print("Floor unloaded.");
		}
	}
	
	private void TogglePause()
	{
		IsPaused = !IsPaused;
		GD.Print($"Game is {(IsPaused ? "Paused" : "Unpaused")}");

		if (IsPaused)
{
	if (pausePanelInstance == null)
	{
		pausePanelInstance = pausePanelScene.Instantiate<Control>();
		GetTree().Root.AddChild(pausePanelInstance);
		ToHomeButton = pausePanelInstance.GetNodeOrNull<Button>("ToHomeButton");
		if (CurrentFloor == "home")
		{
			ToHomeButton.Visible = false;
		}
		else
		{
			ToHomeButton.Visible = true;
			ToHomeButton.Pressed += ToHomePressed;
		}
	}
}
		else
		{
			if (pausePanelInstance != null)
			{
				pausePanelInstance.QueueFree();
				pausePanelInstance = null;
			}
		}
	}
	
	public void ToHomePressed()
	{
		{
			GD.Print("Going home");
			if (GameManager.Instance != null)
			{
				GD.Print("Starting Gameplay:");
				GameManager.Instance.UnloadFloor(); 
				GameManager.Instance.LoadFloor("home");
			}
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
