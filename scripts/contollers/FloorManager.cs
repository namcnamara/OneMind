using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class FloorManager : Node
{
	public string CurrentFloor { get; set; } = "home";
	private Node currentFloorInstance;
	public bool PlayerIsDead {get; set;} = false;
	
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
		pausePanelScene = GD.Load<PackedScene>("res://scenes/controllers/Pause.tscn");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (GameIsPlaying)
		{
			check_game_processes(delta);
		}
		if (PlayerIsDead && !floorUnloaded)
		{
			UnloadFloor();
			floorUnloaded = true;
		}
	}
	
	public void check_game_processes(double delta)
	{
		//Reoccuring things to check for like non-player related input
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

		GameManager.Instance.PlayerManager.Player_Body = currentFloorInstance.GetNodeOrNull<HugoBody3d>("hugo/hugo_char");
		if (GameManager.Instance.PlayerManager.Player_Body != null)
		{
			GameManager.Instance.PlayerManager.Player_Location = GameManager.Instance.PlayerManager.Player_Body.GlobalPosition;
			GD.Print("Player_Body reference updated in FloorManager.");
		}
	}
	
	public void UnloadFloor()
	{
		GameManager.Instance.flush_registries();
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
			GD.Print("Starting Gameplay:");
			UnloadFloor(); 
			LoadFloor("home");
		}
	}
}
