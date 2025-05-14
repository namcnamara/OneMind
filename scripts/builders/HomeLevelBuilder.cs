using Godot;
using System;

public partial class HomeLevelBuilder : LevelBuilderInterface
{
	private PackedScene enemyScene = GD.Load<PackedScene>("res://scenes/enemies/red_cap.tscn");

	public void Build(Node3D parent, Vector3 floorCenter, Vector3 floorSize)
	{
		var rand = new RandomNumberGenerator();
		rand.Randomize();

		for (int i = 0; i < 15; i++)
		{
			var enemy = enemyScene.Instantiate<Node3D>();
			Vector3 pos = new Vector3(
				rand.RandfRange(floorCenter.X - floorSize.X / 2f, floorCenter.X + floorSize.X / 2f),
				floorCenter.Y,
				rand.RandfRange(floorCenter.Z - floorSize.Z / 2f, floorCenter.Z + floorSize.Z / 2f)
			);

			enemy.GlobalPosition = pos;
			parent.AddChild(enemy);
		}
	}
}
