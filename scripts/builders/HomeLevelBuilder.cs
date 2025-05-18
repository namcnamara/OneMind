using Godot;
using System;

public partial class HomeLevelBuilder : LevelBuilderInterface
{
	public bool hasBubbleHut = true;
	private PackedScene BubbleHutScene = GD.Load<PackedScene>("res://scenes/buildings/BubbleHut.tscn");
	public bool hasHippoHut = false;
	private PackedScene HippoHutScene = GD.Load<PackedScene>("res://scenes/buildings/HippoHut.tscn");
	public bool hasLichenLounge = true;
	private PackedScene LichenLoungeScene = GD.Load<PackedScene>("res://scenes/buildings/LichenLounge.tscn");
	public RandomNumberGenerator rand = new RandomNumberGenerator();
	
	private PackedScene enemyScene = GD.Load<PackedScene>("res://scenes/enemies/red_cap.tscn");

	public void Build(Node3D parent, Vector3 floorCenter, Vector3 floorSize)
	{
		rand.Randomize();
		
		//Build 
		if (hasBubbleHut)
		{
			// Adds 5 gloop (handled in building
			var resource = BubbleHutScene.Instantiate<Node3D>();
			spawn_resource(resource, parent, floorCenter, floorSize);
		}
		if (hasHippoHut)
		{
			// Adds hippo transform
			var resource = HippoHutScene.Instantiate<Node3D>();
			spawn_resource(resource, parent, floorCenter, floorSize);
		}
		if (hasLichenLounge)
		{
			// Adds 25 health
			var resource = LichenLoungeScene.Instantiate<Node3D>();
			spawn_resource(resource, parent, floorCenter, floorSize);
		}
		

	}
	
	public void spawn_resource(Node3D resource, Node3D parent, Vector3 floorCenter, Vector3 floorSize)
	{
		for (int i = 0; i < 25; i++)
		{
			Vector3 pos = new Vector3(
				rand.RandfRange(floorCenter.X - floorSize.X / 2f, floorCenter.X + floorSize.X / 2f),
				floorCenter.Y,
				rand.RandfRange(floorCenter.Z - floorSize.Z / 2f, floorCenter.Z + floorSize.Z / 2f)
			);

			parent.GlobalPosition = pos;
			parent.AddChild(resource);
		}
		GD.Print("Spawn successs");
	}

}
