using Godot;
using System;

public partial class GameManager : Node
{
	//GameManager is a singleton
	public static GameManager Instance { get; private set; }
	//player is a singleton
	public Player Player_Movable { get; set;}
	public HugoBody3d Player_body { get; set; }
	public Vector3 Player_location {get; set;} = Vector3.Zero;
	public bool isPaused {get; set;} = false;
	// TODO: Hold a random generator for my classes 
	
	

	public override void _Ready()
	{
		Instance = this;
	}
	public override void _PhysicsProcess(double delta)
	{
		Player_location = Player_body.GlobalPosition;
		if (Input.IsActionJustPressed("pause"))
		{
			isPaused = !isPaused; 
			GD.Print($"Game is {(isPaused ? "Paused" : "Unpaused")}");
		}
	}
	
	public void LoadFloor(string buildType = "home")
{
	GD.Print("Loading floor...");

	var floorScene = GD.Load<PackedScene>("res://scenes/environment/levels/basic_floor.tscn");
	var floorInstance = floorScene.Instantiate<Node>();

	if (floorInstance is basic_floor floor)
	{
		floor.BuildType = buildType;
	}

	GetTree().Root.AddChild(floorInstance);
}
}
