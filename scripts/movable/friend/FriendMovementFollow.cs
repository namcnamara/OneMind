using Godot;
using System;

public class FriendMovementFollow : FriendMovementStrategy
{
	private float moveSpeed = 2f;
	private float maxSpeed = 8f;

	public override void Move(Friend friend, string name, double delta)
	{
		//ensure run is ready
		base.Move(friend, name, delta);
		if (friend is not Friend goop)
			return;
		var Player_Location = GameManager.Instance.PlayerManager.Player_Location;
		if (Player_Location == Vector3.Zero) return;
		goop.CurrentDistance = goop.RigidBody.GlobalPosition.DistanceTo(Player_Location);
		goop.CurrentDirection = choose_direction(goop, delta);
	}

	private Vector3 choose_direction(Friend goop, double delta)
	{
		bool in_chasing_distance = goop.CurrentDistance < goop.DetectionRadius;
		if (goop.closestEnemy != null)
		{
			if ( goop.CurrentDistance < goop.CurrentDistanceToClosestEnemy)
			{
				if (goop.CurrentDistance < 1f)
				{
						goop.CurrentDirection = 1f * (GameManager.Instance.PlayerManager.Player_Location - goop.RigidBody.GlobalPosition).Normalized();
				}
				else if (goop.CurrentDistance < 3f)
				{
						goop.CurrentDirection = 5f * (GameManager.Instance.PlayerManager.Player_Location - goop.RigidBody.GlobalPosition).Normalized();
				}
				else
				{
						goop.CurrentDirection = 10f * (GameManager.Instance.PlayerManager.Player_Location - goop.RigidBody.GlobalPosition).Normalized();
				}
			}
			else 
			{
				goop.CurrentDirection = 1f * (goop.closestEnemy.GlobalPosition - goop.RigidBody.GlobalPosition).Normalized();
			}
		}
		else
		{
			
		}
	
		//Apply momementum
		goop.RigidBody.ApplyCentralForce(goop.CurrentDirection * moveSpeed);	
		//goop.RigidBody.LinearVelocity = goop.RigidBody.LinearVelocity * dampeningFactor;
		// Clamp velocity to max speed
		if (goop.RigidBody.LinearVelocity.Length() > maxSpeed)
		{
			goop.RigidBody.LinearVelocity = goop.RigidBody.LinearVelocity.Normalized() * maxSpeed;
		}
		return goop.CurrentDirection;
	}
}
