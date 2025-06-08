using Godot;
using System;

//This is the first level. Only a few basic goop heads to show enemy movement to the player. Shows the transform ability. 

public partial class EnemyLevel0Builder : LevelBuilderInterface
{
	private PackedScene goopScene = GD.Load<PackedScene>("res://scenes/enemies/goop_head.tscn");

	public void Build(Node3D parent, Vector3 floorCenter, Vector3 floorSize)
	{
		var rand = new RandomNumberGenerator();
		rand.Randomize();
		for (int i = 0; i < 5; i++)
		{
			var goop = goopScene.Instantiate<Node3D>();
			Vector3 pos = new Vector3(
				rand.RandfRange(floorCenter.X - floorSize.X / 2f, floorCenter.X + floorSize.X / 2f),
				floorCenter.Y,
				rand.RandfRange(floorCenter.Z - floorSize.Z / 2f, floorCenter.Z + floorSize.Z / 2f)
			);
			parent.AddChild(goop);
			goop.GlobalPosition = pos;
			
		}
	}
}
