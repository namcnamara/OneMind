using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
	//GameManager is a singleton
	public static GameManager Instance { get; private set; }
	private Node currentFloorInstance;
	//player is a singleton
	public Player Player_Movable { get; set;}
	public HugoBody3d Player_Body { get; set; }
	public Vector3 Player_Location {get; set;} = Vector3.Zero;
	public bool IsDead {get; set;} = false;
	public bool IsPaused {get; set;} = false;
	
	private PackedScene pausePanelScene;
	private Control pausePanelInstance;
	
	public bool floorUnloaded = false;
	// TODO: Hold a random generator for my classes 
	
	public int PlayerMaxHealth {get; set;} = 100;
	public int PlayerMaxGoop {get; set;} = 5;
	public List<string> PlayerTransforms = new List<string>();
	public bool GameIsPlaying = false;
	
	//Benefits
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
		Player_Location = Player_Body.GlobalPosition;
		if (Input.IsActionJustPressed("pause"))
		{
			TogglePause();
		}
	}
	
	public void LoadFloor(string buildType = "home")
	{
		var floorScene = GD.Load<PackedScene>("res://scenes/environment/levels/basic_floor.tscn");
		currentFloorInstance = floorScene.Instantiate<Node>();
		
		if (currentFloorInstance is basic_floor floor)
		{
			floor.BuildType = buildType;
		}

		// Add to Master
		var master = GetTree().Root.GetNode<Master>("Master");
		master.AddChild(currentFloorInstance);
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
}
