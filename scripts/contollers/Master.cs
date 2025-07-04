using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Master : Node
{
	public PackedScene TitleScreen { get; set;}
	public AnimatedSprite2D animatedSprite {get; set;}
	public Button playButton {get; set;}
	
	 public override void _Ready()
	{
		LoadTitle();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (GameManager.Instance.PlayerManager.IsDead)
		{
			LoadTitle();
			GameManager.Instance.PlayerManager.IsDead = false;
		}
	}

	private void OnPlayPressed()
	{
		GD.Print("Play");
		if (GameManager.Instance.FloorManager != null)
		{
			LoadGame();
		}
		else
		{
			GD.PushError("GameManager is not initialized.");
		}
	}
	
	public void LoadGame()
	{
		GD.Print("Starting Gameplay:");
			GameManager.Instance.FloorManager.LoadFloor("enemy0"); 
			//Drop the title scene and load home level
			GetNode("Title").QueueFree();
			// Open physics process in game manager
			GameManager.Instance.FloorManager.GameIsPlaying = true;
	}
	
	public void LoadTitle()
	{
		GameManager.Instance.FloorManager.UnloadFloor();
		GameManager.Instance.FloorManager.GameIsPlaying = false;
		TitleScreen = GD.Load<PackedScene>("res://scenes/controllers/Title.tscn");
		var titleInstance = TitleScreen.Instantiate<Node>();
		animatedSprite = titleInstance.GetNode<AnimatedSprite2D>("BlinkHippo");
		playButton = titleInstance.GetNode<Button>("Play");
		animatedSprite.Play("default");
		playButton.Pressed += OnPlayPressed;
		AddChild(titleInstance);
	}
}
