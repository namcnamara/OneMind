using Godot;
using System;

public class MovementFollow : MovementStrategy
{
	private float moveSpeed = 6f;
	private float maxSpeed = 2f;

	public override void Move(Friend friend, string name, double delta)
	{
		base.Move(friend, name, delta);

		// Make sure it's a GoopMinion
		if (friend is not GoopMinion goop)
			return;

		if (GameManager.Instance.Player_Location == Vector3.Zero)
			return;

		// Calculate direction to player
		Vector3 targetPos = GameManager.Instance.Player_Location;
		Vector3 currentPos = goop.GlobalPosition;

		goop.CurrentDistance = currentPos.DistanceTo(targetPos);
		Vector3 direction = (targetPos - currentPos).Normalized();
		goop.CurrentDirection = direction;

		// Apply force toward the player
		goop.RigidBody.ApplyCentralForce(direction * moveSpeed);

		// Clamp velocity
		if (goop.RigidBody.LinearVelocity.Length() > maxSpeed)
		{
			goop.RigidBody.LinearVelocity = goop.RigidBody.LinearVelocity.Normalized() * maxSpeed;
		}
	}
}
