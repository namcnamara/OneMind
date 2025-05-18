using Godot;
using System;

public partial class Master : Node
{
	 public override void _Ready()
	{
		var TitleAnim = GetNode<AnimatedSprite2D>("Title/BlinkHippo");
		var playButton = GetNode<Button>("Title/Play");
		TitleAnim.Play("default");
		playButton.Pressed += OnPlayPressed;
	}

	private void OnPlayPressed()
	{
		GD.Print("Play");

		if (GameManager.Instance != null)
		{
			GD.Print("Starting Gameplay:");
			GameManager.Instance.LoadFloor("home"); // Call it explicitly here
			
			//Drop the title scene and load home level
			GetNode("Title").QueueFree();
			
			// Open physics process in game manager
			GameManager.Instance.GameIsPlaying = true;
		}
		else
		{
			GD.PushError("GameManager is not initialized.");
		}
	}
}
