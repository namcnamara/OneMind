using Godot;
using System;

public class ActionBubble : EnemyActionStrategy
{
		public override void Act(Enemy enemy, string name, double delta)
	{
		//	Ensure run is ready
		base.Act(enemy, name, delta);
		var body = enemy.GetRigidBody();
		var Player_Location = GameManager.Instance.PlayerManager.Player_Location;
		Bubble(enemy, delta);
	}
	
	private void Bubble(Enemy enemy, double delta)
	{	
		if (enemy is not RedCap redCap) return;

		float radius = redCap.BubbleRadius;
		float d_radius = redCap.BubbleDamageRadius;
		var pos = enemy.GlobalPosition;
		var playerBody = GameManager.Instance.PlayerManager.Player_Body;
		if (playerBody != null)
		{
			var dist = pos.DistanceTo(playerBody.GlobalPosition);

			if (dist <= radius)
			{
				ResetBubbleTimer(redCap);
				if (dist <= d_radius)
				{
					GD.Print("ddddddddddd\nddddddddddddddddddddddddddddddddddddddd\ndddddddddddd");
				}
					playerBody.take_damage(redCap.BubbleDamage);
			}
		}

		// Friends
		foreach (var friend in GameManager.Instance.GetAllFriends())
		{
			var dist = pos.DistanceTo(friend.GlobalPosition);
			if (dist <= radius)
			{
				ResetBubbleTimer(redCap);
				if (dist <= d_radius)
					friend.TakeDamage(redCap.BubbleDamage);
			}
		}

		// Enemies
		foreach (var other in GameManager.Instance.GetAllEnemies())
		{
			if (other.TYPE == "red_cap") continue;
			var dist = pos.DistanceTo(other.GlobalPosition);
			if (dist <= radius)
			{
				ResetBubbleTimer(redCap);
				if (dist <= d_radius)
					other.TakeDamage(redCap.BubbleDamage);
			}
		}
		ManageBubbling(delta, redCap);
	}
	
	public void ResetBubbleTimer(RedCap	rc)
	{
		rc.IsBubbling = true;
		rc.BubbleTimer = rc.BubbleCooldown;
	}
	
	public void ManageBubbling(double delta, RedCap rc)
	{
		if (rc.IsBubbling)
		{
			rc.BubbleTimer -= (float)delta;
			if (rc.BubbleTimer < 0)
				rc.IsBubbling = false;
		}
	}
}
