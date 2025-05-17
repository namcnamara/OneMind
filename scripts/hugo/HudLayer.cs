using Godot;
using System;

public partial class HudLayer : CanvasLayer
{
	private TextureProgressBar healthProgressBar;
	private Label healthLabelText;
	private Label gloopLabelText;
	private string default_text = "GLOOPS LEFT:";

	public override void _Ready()
	{
		healthProgressBar = GetNode<TextureProgressBar>("TopLeftPanel/ItemList/HBOXHealth/HealthBar");
		healthLabelText = GetNode<Label>("TopLeftPanel/ItemList/HBOXHealth/HealthBar/HealthLabel");
		gloopLabelText = GetNode<Label>("TopLeftPanel/ItemList/HBOXGloop/GloopLabel");
		UpdateHealth(100);
		UpdateGloop(5);
	}

	public void UpdateHealth(int health)
	{
		health = Mathf.Clamp(health, 0, 100); 
		healthProgressBar.Value = health;
		healthLabelText.Text = $"Health: {health}";
	}

	public void UpdateGloop(int gloop)
	{
		gloop = Mathf.Clamp(gloop, 0, 10); 
		gloopLabelText.Text = default_text + gloop.ToString();
	}
}
