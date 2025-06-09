using Godot;
using System;

public partial class TriggerArea : Area3D
{
	private string FloorToLoad { get; set; } = "enemy"; 
	private CollisionShape3D collide;
	private Label3D label;

	public override void _Ready()
	{
		collide = GetNodeOrNull<CollisionShape3D>("CollisionShape3D");
		label = GetNodeOrNull<Label3D>("Label3D");
		label.Text = "Unassigned Area";
		this.BodyEntered += OnBodyEntered;
	}

	// Set the floor type this trigger switches to
	public void UpdateTrigger(string buildType)
	{
		if (buildType == "home")
		{
			label.Text = "Spread Out";
			FloorToLoad= GameManager.Instance.ProgressManager.get_progress();
			GD.Print("The trigger will load ", FloorToLoad);
		}
		else
		{
			label.Text = "Enter Home";
			FloorToLoad = "home";
		}
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body is HugoBody3d hugo)
		{
			GD.Print($"Player entered trigger area. Switching to floor: {FloorToLoad}");

			if (GameManager.Instance != null)
			{
				GameManager.Instance.FloorManager.UnloadFloor();
				GameManager.Instance.FloorManager.LoadFloor(FloorToLoad);
				GameManager.Instance.FloorManager.CurrentFloor = FloorToLoad;
			}
		}
	}
}
