using Godot;
using System;

public partial class Movable : Node3D
{
	// This class provides a bridge for pause functionality between any object type that moves. 
	// It enables the pause menu to pause all moving objects, like the player,
	// enemies, or terrain features. 
	//Shoudl also stop timers and animations. 
	[Export]
	public bool AffectedByPause { get; set; } = true;
	public Vector3 CurrentPosition { get; set; }
	public Vector3 LastVelocity { get; set; }
	public Vector3 lastPosition;
	public bool IsDead;
	public bool isPaused;
	public int Health;
	public int MaxHealth;
	public string TYPE = "generic";
	public string movement = "";
	public string action = "";

	public override void _Ready()
	{
		lastPosition = GlobalTransform.Origin;
		CurrentPosition = lastPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (GameManager.Instance.FloorManager.IsPaused)
		{
			isPaused = true;
		}
		else
			isPaused = false;
			
		CurrentPosition = GlobalTransform.Origin;
		LastVelocity = (CurrentPosition - lastPosition) / (float)delta;
		lastPosition = CurrentPosition;
	}
}
