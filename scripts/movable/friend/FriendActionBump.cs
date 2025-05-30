using Godot;
using System;
using System.Linq;

public class FriendActionBump : FriendActionStrategy
{
	private float moveSpeed = 6f;

	public override void Act(Friend friend, string name, double delta)
	{
		base.Act(friend, name, delta);

		if (friend is not GloopMinion goop)
			return;

		BumpCharge(goop, delta);
	}

	private void BumpCharge(GloopMinion goop, double delta)
	{
		if (goop.IsBumping)
		{
			goop.BumpTimer -= (float)delta;
			if (goop.BumpTimer <= 0)
			{
				goop.IsBumping = false;
				goop.BumpTimer = goop.BumpCooldown;
			}
			return;
		}

		// Find closest enemy
		var enemies = GameManager.Instance.GetAllEnemies();
		Enemy target = null;
		float closestDistance = float.MaxValue;

		foreach (var enemy in enemies)
		{
			if (enemy == null || enemy.IsDead)
				continue;

			float distance = goop.GlobalPosition.DistanceTo(enemy.GlobalPosition);
			if (distance < closestDistance)
			{
				closestDistance = distance;
				target = enemy;
			}
		}

		if (target == null)
			return;

		goop.CurrentDistance = closestDistance;

		// Charge toward enemy
		if (goop.CurrentDistance < goop.BumpChargeRadius)
		{
			Vector3 direction = (target.GlobalPosition - goop.GlobalPosition).Normalized();
			goop.CurrentDirection = direction * moveSpeed;
			goop.RigidBody.ApplyCentralForce(goop.CurrentDirection);
			goop.WanderTimer = goop.WanderCooldown;
		}

		// Bump Trigger
		if (goop.CurrentDistance < goop.BumpRadius)
		{
			goop.IsBumping = true;
			goop.BumpTimer = goop.BumpCooldown;
			GD.Print($"***************Minion Bumped*************** {target.TYPE}");
			goop.animatedSprite.Play("bump");
			goop.TakeDamage(25);           
			target.TakeDamage(10);         
		}
	}
}
