using Godot;
using System;

public interface LevelBuilderInterface
{
	void Build(Node3D parent, Vector3 floorCenter, Vector3 floorSize);
}
