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
		int max_enemies = 10;
		for (int i = 0; i < max_enemies; i++)
		{
			GameManager.Instance.FloorManager.currentEnemyCount += 1;
			var goop = goopScene.Instantiate<Node3D>();
			Vector3 pos = new Vector3(
				rand.RandfRange(floorCenter.X - floorSize.X / 2f, floorCenter.X + floorSize.X / 2f),
				floorCenter.Y,
				rand.RandfRange(floorCenter.Z - floorSize.Z / 2f, floorCenter.Z + floorSize.Z / 2f)
			);
			parent.AddChild(goop);
			goop.GlobalPosition = pos;
		}
		GameManager.Instance.FloorManager.maxEnemyCount = max_enemies;
		GameManager.Instance.FloorManager.EnemiesDefeated = false;
		
	}
}
