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
	// TODO: Hold a random generator for my classes 

	public override void _Ready()
	{
		Instance = this;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Player_location = Player_body.GlobalPosition;
	}
}
