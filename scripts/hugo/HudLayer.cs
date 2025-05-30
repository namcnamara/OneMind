using Godot;
using System;

public partial class HudLayer : CanvasLayer
{
	private TextureProgressBar healthProgressBar;
	private Label healthLabelText;
	private Label gloopLabelText;
	private AnimatedSprite2D GloopAnimation;
	private string default_text = "GOOPS LEFT:";

	public override void _Ready()
	{
		healthProgressBar = GetNode<TextureProgressBar>("TopLeftPanel/ItemList/HBOXHealth/HealthBar");
		healthLabelText = GetNode<Label>("TopLeftPanel/ItemList/HBOXHealth/HealthBar/HealthLabel");
		gloopLabelText = GetNode<Label>("TopLeftPanel/ItemList/HBOXGloop/GloopLabel");
		GloopAnimation = GetNode<AnimatedSprite2D>("TopLeftPanel/ItemList/HBOXGloop/GloopAnimation");
		GloopAnimation.Play("default");
		CallDeferred(nameof(InitHUD)); 
	}

	private void InitHUD()
	{
		if (GameManager.Instance.PlayerManager.Player_Body != null)
		{
			UpdateHealth(GameManager.Instance.PlayerManager.PlayerMaxHealth);
			UpdateGloop(GameManager.Instance.PlayerManager.PlayerMaxGloop);
		}
		else
		{
			GD.PrintErr("GameManager or Player_Body not ready when HudLayer.InitHUD ran.");
		}
	}

	public void UpdateHealth(int health)
	{
		health = Mathf.Clamp(health, 0, GameManager.Instance.PlayerManager.PlayerMaxHealth); 
		healthProgressBar.Value = health;
		healthLabelText.Text = $"Health: {health}";
	}

	public void UpdateGloop(int gloop)
	{
		gloop = Mathf.Clamp(gloop, 0, GameManager.Instance.PlayerManager.PlayerMaxGloop); 
		gloopLabelText.Text = default_text + " " + gloop.ToString();
	}
}
