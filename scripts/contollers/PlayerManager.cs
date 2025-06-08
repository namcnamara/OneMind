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
	public int PlayerMaxGloop {get; set;} = 40;
	public List<string> PlayerTransforms = new List<string>();
	public bool IsDead {get; set;} = false;
	public int GloopMinionCost {get; set;} = 5;
	
	public int mush_count = 0;
	public int goo_count = 0;
	public int berry_count = 0;
	
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
