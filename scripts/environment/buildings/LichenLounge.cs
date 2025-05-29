using Godot;
using System;

public partial class LichenLounge : RigidBody3D
{
	// Building for Home. 
	//Increases goop from 5 to 10 if loaded in home node.
	private Random random = new Random();
	private AnimatedSprite3D animatedSprite;
	private CollisionShape3D collisionShape;
	private bool isOccupied = false;
	public float OccupyDistance = 1f;
	public HugoBody3d player;
	
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite3D>("LichenLoungeAnim");
		collisionShape = GetNode<CollisionShape3D>("LichenLoungeCollide");
		animatedSprite.Play("idle");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (isOccupied)
			animatedSprite.Play("occupied");
		else
		{
			GD.Print(player);
			if (player == null)
			{
				player = GameManager.Instance.PlayerManager.Player_Body;
			}
			else
			{
				
				float distance = GlobalPosition.DistanceTo(player.GlobalPosition);
				GD.Print(distance);
				if (distance <= OccupyDistance)
				{
					isOccupied = true;
					animatedSprite.Play("occupied");
					GD.Print("Occupied!");

					GameManager.Instance.PlayerManager.PlayerMaxHealth += 25;
					GameManager.Instance.PlayerManager.Player_Body.HEALTH = GameManager.Instance.PlayerManager.PlayerMaxHealth;
					GameManager.Instance.PlayerManager.Player_Body.updateHUD();
				}
			}
		}
	}
}
