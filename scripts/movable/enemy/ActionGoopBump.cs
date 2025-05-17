using Godot;
using System;

public class ActionGoopBump : ActionStrategy
{
	private float moveSpeed = 60f;

	public override void Act(Enemy enemy, string name, double delta)
	{
		//ensure run is ready
		base.Act(enemy, name, delta);
		if (enemy is not GoopHead goop)
			return;
		var body = goop.GetRigidBody();
		var player_location = GameManager.Instance.Player_location;
		BumpCharge(goop, delta);
	}
	
	private void BumpCharge(GoopHead goop, double delta)
	{
		// If in bump cooldown, skip
		if (goop.IsBumping)
		{
			goop.BumpTimer -= (float)delta;  
			if (goop.BumpTimer <= 0)
			{
				goop.IsBumping = false;
				goop.BumpTimer = goop.BumpCooldown; 
			}
		}
		else
		{
			// Check if within charge range (but not yet bumped)
			if (goop.CurrentDistance < goop.BumpChargeRadius && goop.PlayerBody.state != "head")
			{
				Vector3 direction = (GameManager.Instance.Player_location - goop.RigidBody.GlobalPosition).Normalized();
				goop.CurrentDirection = direction * moveSpeed;
				goop.RigidBody.ApplyCentralForce(goop.CurrentDirection);
				goop.WanderTimer = goop.WanderCooldown;
			}

			// Bump Trigger
			if (goop.CurrentDistance < goop.BumpRadius && goop.PlayerBody.state != "head")
			{
				goop.IsBumping = true;
				goop.BumpTimer = goop.BumpCooldown;
				goop.health -= 50;
				GD.Print("***************Bumpin*****************" + goop.EnemyName);
				goop.animatedSprite.Play("bump");
				goop.PlayerBody.take_damage(5);

				// Handle death
				if (goop.health <= 0)
				{
					goop.GetTree().CreateTimer(0.5f).Timeout += () =>
					{
						GD.Print("deleted");
						goop.QueueFree();
					};
				}
			}
		}
	}
}
