using Godot;
using System;

public partial class RandomTree : Node3D
{
	public override void _Ready()
	{
	// This loads a random resource sceen: bushes, rocks, trees, or materials
	PackedScene[] TreeScenes = new PackedScene[]
		{
			GD.Load<PackedScene>("res://scenes/environment/tree_curl.tscn"),
			GD.Load<PackedScene>("res://scenes/environment/tree_poof.tscn"),
			GD.Load<PackedScene>("res://scenes/environment/rock_blue.tscn"),
			GD.Load<PackedScene>("res://scenes/environment/bush_berry.tscn"),
			GD.Load<PackedScene>("res://scenes/environment/bush_goop.tscn"),
		};

		var random = new Random();
		int index = random.Next(TreeScenes.Length);

		Node3D treeInstance = TreeScenes[index].Instantiate<Node3D>();
		AddChild(treeInstance);
		
		//Add a little difference to the objects for added variety
		treeInstance.Scale = Vector3.One * (float)GD.RandRange(0.8f, 1.2f);
	}
}
