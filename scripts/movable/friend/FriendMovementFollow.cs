using Godot;
using System;

public class FriendMovementFollow : FriendMovementStrategy
{
	private float moveSpeed = 6f;
	private float maxSpeed = 2f;
	private Random random = new Random();

	// Add fields to FriendMovementFollow:
	private float wanderTimer = 0f;
	private Vector3 randomOffset = Vector3.Zero;

	public override void Move(Friend friend, string name, double delta)
	{
		base.Move(friend, name, delta);

		if (friend is not GloopMinion gloop)
		{
			GD.Print("Error: friend is not a GloopMinion");
			return;
		}

		wanderTimer -= (float)delta;
		if (wanderTimer <= 0)
		{
			randomOffset = new Vector3(
				(float)(random.NextDouble() - 0.5),
				0,
				(float)(random.NextDouble() - 0.5)
			);
			wanderTimer = 1f;
		}
		
		GD.Print("1 - Moving");
		
		Vector3 targetPos = GameManager.Instance.PlayerManager.Player_Location + randomOffset;
		Vector3 currentPos = gloop.GlobalPosition;
		gloop.CurrentDistance = currentPos.DistanceTo(targetPos);
		Vector3 direction = (targetPos - currentPos).Normalized();
		gloop.CurrentDirection = direction;
		float forceMagnitude = moveSpeed;

		if (gloop.RigidBody != null)
		{
			gloop.RigidBody.ApplyCentralForce(direction * forceMagnitude);
			GD.Print($"Applied force: {direction * forceMagnitude}, velocity: {gloop.RigidBody.LinearVelocity}");

			if (gloop.RigidBody.LinearVelocity.Length() > maxSpeed)
			{
				gloop.RigidBody.LinearVelocity = gloop.RigidBody.LinearVelocity.Normalized() * maxSpeed;
				GD.Print("2 - Velocity clamped");
			}
		}
		else
		{
			GD.Print("Error: RigidBody is null");
		}
	}
}
