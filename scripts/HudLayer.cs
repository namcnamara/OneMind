using Godot;
using System;

public partial class HudLayer : CanvasLayer
{
	private TextureProgressBar healthBar;
	private Label healthLabel;
	private Label gloopLabel;

	public override void _Ready()
	{
		healthBar = GetNode<TextureProgressBar>("TopLeftPanel/ItemList/HBOXHealth/HealthBar");
		healthLabel = GetNode<Label>("TopLeftPanel/ItemList/HBOXHealth/HealthBar/HealthLabel");
		gloopLabel = GetNode<Label>("TopLeftPanel/ItemList/HBOXGloop/GloopLabel");
	}

	public void SetHealth(int health)
	{
		healthBar.Value = health;
		healthLabel.Text = $"Health: {health}";
	}

	public void SetGloop(int gloop)
	{
		gloopLabel.Text = gloop.ToString();
	}
}
