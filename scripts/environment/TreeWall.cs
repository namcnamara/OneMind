using Godot;
using System;

public partial class TreeWall : Node3D
{
	private PackedScene Trees = GD.Load<PackedScene>("res://scenes/environment/random_tree.tscn");
	private Random random = new Random();
	
	
	public override void _Ready()
	{
		Node3D tree = (Node3D)Trees.Instantiate();
		GetParent().AddChild(tree); 
	}
}
