using Godot;
using System;
using System.Linq;

public class FriendActionBump : FriendActionStrategy
{
	private float moveSpeed = 6f;

	public override void Act(Friend friend, string name, double delta)
	{
		base.Act(friend, name, delta);

		if (friend is not GloopMinion gloop)
			return;

		BumpCharge(gloop, delta);
	}

	private void BumpCharge(GloopMinion gloop, double delta)
	{
		if (gloop.IsBumping)
		{
			gloop.BumpTimer -= (float)delta;
			if (gloop.BumpTimer <= 0)
			{
				gloop.IsBumping = false;
				gloop.BumpTimer = gloop.BumpCooldown;
			}
			return;
		}
		// Charge toward enemy
		if (gloop.CurrentDistanceToClosestEnemy < gloop.BumpChargeRadius)
		{
			Vector3 direction = (gloop.closestEnemy.GlobalPosition - gloop.GlobalPosition).Normalized();
			gloop.CurrentDirection = direction * moveSpeed;
			gloop.RigidBody.ApplyCentralForce(gloop.CurrentDirection);
			gloop.WanderTimer = gloop.WanderCooldown;
		}

		// Bump Trigger
		if (gloop.CurrentDistance < gloop.BumpRadius)
		{
			gloop.IsBumping = true;
			gloop.BumpTimer = gloop.BumpCooldown;
			GD.Print($"***************enemy Bumped*************** {gloop.closestEnemy.FullName}");
			gloop.animatedSprite.Play("bump");
			gloop.TakeDamage(25);           
			gloop.closestEnemy.TakeDamage(10);         
		}
	}
}
