using Godot;
using System;

public class FriendMovementFollow : FriendMovementStrategy
{
	private float moveSpeed = 6f;
	private float maxSpeed = 2f;

	public override void Move(Friend friend, string name, double delta)
	{
		if (friend is not GloopMinion gloop || friend.PlayerBody == null)
			return;

		Vector3 direction = (friend.PlayerBody.GlobalPosition - friend.GlobalPosition).Normalized();

		friend.LastDirection = friend.CurrentDirection;
		friend.CurrentDirection = direction;

		// Set controlled movement velocity directly
		Vector3 velocity = direction * moveSpeed;
		gloop.RigidBody.LinearVelocity = velocity;

		// Optional: Add a bit of damp for smooth stop
		gloop.RigidBody.LinearDamp = 5.0f;
	}
}
