using Godot;
using System;

public partial class EnemyLevelBuilder : LevelBuilderInterface
{
	private PackedScene goopScene = GD.Load<PackedScene>("res://scenes/enemies/goop_head.tscn");
	private PackedScene pBlobScene = GD.Load<PackedScene>("res://scenes/enemies/prickly_blob.tscn");
	private PackedScene capScene = GD.Load<PackedScene>("res://scenes/enemies/red_cap.tscn");

	public void Build(Node3D parent, Vector3 floorCenter, Vector3 floorSize)
	{
		var rand = new RandomNumberGenerator();
		rand.Randomize();
		for (int i = 0; i < 10; i++)
		{
			var pBlob = pBlobScene.Instantiate<Node3D>();
			var cap = capScene.Instantiate<Node3D>();
			Vector3 pos = new Vector3(
				rand.RandfRange(floorCenter.X - floorSize.X / 2f, floorCenter.X + floorSize.X / 2f),
				floorCenter.Y,
				rand.RandfRange(floorCenter.Z - floorSize.Z / 2f, floorCenter.Z + floorSize.Z / 2f)
			);
			Vector3 pos2 = new Vector3(
				rand.RandfRange(floorCenter.X - floorSize.X / 2f, floorCenter.X + floorSize.X / 2f),
				floorCenter.Y,
				rand.RandfRange(floorCenter.Z - floorSize.Z / 2f, floorCenter.Z + floorSize.Z / 2f)
			);

			parent.AddChild(pBlob);
			parent.AddChild(cap);
			cap.GlobalPosition = pos;
			pBlob.GlobalPosition = pos2;
			
		}
	}
}
