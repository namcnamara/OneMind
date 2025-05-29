using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class PlayerManager : Node
{
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
		PlayerTransforms.Add("head");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (GameManager.Instance.FloorManager.GameIsPlaying)
		{
			check_game_processes(delta);
		}
	}
	
	public void check_game_processes(double delta)
	{
		if (Player_Body != null)
			Player_Location = Player_Body.GlobalPosition;
	}
}
