using Godot;
using System;

public class FriendMovementFollow : FriendMovementStrategy
{
	private float moveSpeed = 6f;
	private float maxSpeed = 2f;

	public override void Move(Friend friend, string name, double delta)
	{
		base.Move(friend, name, delta);

		// Make sure it's a GloopMinion
		if (friend is not GloopMinion gloop)
			return;

		if (GameManager.Instance.PlayerManager.Player_Location == Vector3.Zero)
			return;
		Vector3 targetPos = GameManager.Instance.PlayerManager.Player_Location;
		Vector3 currentPos = gloop.GlobalPosition;
		gloop.CurrentDistance = currentPos.DistanceTo(targetPos);
		Vector3 direction = (targetPos - currentPos).Normalized();
		gloop.CurrentDirection = direction;
		gloop.RigidBody.ApplyCentralForce(direction * moveSpeed);
		if (gloop.RigidBody.LinearVelocity.Length() > maxSpeed)
		{
			gloop.RigidBody.LinearVelocity = gloop.RigidBody.LinearVelocity.Normalized() * maxSpeed;
		}
	}
}
