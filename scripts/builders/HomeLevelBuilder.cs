using Godot;
using System;

public partial class HomeLevelBuilder : LevelBuilderInterface
{
	public bool hasBubbleHut = true;
	private PackedScene BubbleHutScene = GD.Load<PackedScene>("res://scenes/environment/buildings/BubbleHut.tscn");
	public bool hasLichenLounge = true;
	private PackedScene LichenLoungeScene = GD.Load<PackedScene>("res://scenes/environment/buildings/LichenLounge.tscn");
	public RandomNumberGenerator rand = new RandomNumberGenerator();

	public void Build(Node3D parent, Vector3 floorCenter, Vector3 floorSize)
	{
		rand.Randomize();
		GD.Print("Building home level");
		//Build 
		if (hasBubbleHut)
		{
			GD.Print("Building Bubblehut");
			// Adds 5 gloop (handled in building
			var resource = BubbleHutScene.Instantiate<Node3D>();
			Vector3 pos = new Vector3(floorCenter.X + 4f,floorCenter.Y ,floorCenter.Z + 4f);
			spawn_resource(resource, parent, pos);
			GD.Print("Built Bubblehut");
		}
		
		if (hasLichenLounge)
		{
			// Adds 25 health
			var resource = LichenLoungeScene.Instantiate<Node3D>();
			Vector3 pos = new Vector3(floorCenter.X,floorCenter.Y,floorCenter.Z - 3);
			spawn_resource(resource, parent, pos);
			GD.Print("Building LichenLounge");
		}
		GD.Print("Building home level complete");
	}
	
	public void spawn_resource(Node3D resource, Node3D parent, Vector3 pos)
	{
		resource.GlobalPosition = pos;
		parent.AddChild(resource);
		GD.Print("Spawn successs");
	}
}
