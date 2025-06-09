using Godot;
using System;

public partial class HomeLevelBuilder : LevelBuilderInterface
{
	private PackedScene BubbleHutScene = GD.Load<PackedScene>("res://scenes/environment/buildings/BubbleHut.tscn");
	private PackedScene LichenLoungeScene = GD.Load<PackedScene>("res://scenes/environment/buildings/LichenLounge.tscn");
	public RandomNumberGenerator rand = new RandomNumberGenerator();
	
	
	public void Build(Node3D parent, Vector3 floorCenter, Vector3 floorSize)
	{
		rand.Randomize();
		//Build 
		if (GameManager.Instance.FloorManager.BubbleHut)
		{
			// Adds 5 gloop (handled in building
			var resource = BubbleHutScene.Instantiate<Node3D>();
			Vector3 pos = new Vector3(floorCenter.X + 4f,floorCenter.Y ,floorCenter.Z + 4f);
			spawn_resource(resource, parent, pos);
		}
		
		if (GameManager.Instance.FloorManager.LichenLounge)
		{
			// Adds 25 health
			var resource = LichenLoungeScene.Instantiate<Node3D>();
			Vector3 pos = new Vector3(floorCenter.X,floorCenter.Y,floorCenter.Z - 3);
			spawn_resource(resource, parent, pos);
		}
		GD.Print("Building home level complete");
	}
	
	public void spawn_resource(Node3D resource, Node3D parent, Vector3 pos)
	{
		parent.AddChild(resource);
		resource.GlobalPosition = pos;
	}
}
