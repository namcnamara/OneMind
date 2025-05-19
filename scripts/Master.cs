using Godot;
using System;

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
		// The only function that changes this flag is the HugoBody3d's die function
		if (GameManager.Instance.IsDead)
		{
			LoadTitle();
			GameManager.Instance.IsDead = false;
		}
	}

	private void OnPlayPressed()
	{
		GD.Print("Play");
		if (GameManager.Instance != null)
		{
			GD.Print("Starting Gameplay:");
			GameManager.Instance.LoadFloor("enemy"); 
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
	
	public void LoadTitle()
	{
		GameManager.Instance.UnloadFloor();
		TitleScreen = GD.Load<PackedScene>("res://scenes/Title.tscn");
		var titleInstance = TitleScreen.Instantiate<Node>();
		animatedSprite = titleInstance.GetNode<AnimatedSprite2D>("BlinkHippo");
		playButton = titleInstance.GetNode<Button>("Play");
		animatedSprite.Play("default");
		playButton.Pressed += OnPlayPressed;
		AddChild(titleInstance);
	}
}
