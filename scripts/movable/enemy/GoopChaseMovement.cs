using Godot;
using System;

public class GoopChaseMovement : MovementStrategy
{
	private float moveSpeed = 10f;
	private float velocityThreshold = 0.01f;
	private float maxSpeed = 5f;
	private float dampeningFactor = 0.9f;
	private float wanderCooldown = 2f;
	private Random random = new();

	public override void Move(Enemy enemy, string name, double delta)
	{
		base.Move(enemy, name, delta);

		if (enemy is not GoopHead goop)
			return;
		
		var body = goop.GetRigidBody();
		var player = GameManager.Instance.Player_body;
		if (player == null) return;

		Vector3 playerPos = player.GlobalPosition;
		Vector3 myPos = goop.GlobalPosition;
		goop.CurrentDistance = myPos.DistanceTo(playerPos);
		GD.Print(goop.CurrentDistance);

		// Chase logic
		if (goop.CurrentDistance < goop.DetectionRadius && player.state != "head")
		{
			goop.CurrentDirection = (playerPos - myPos).Normalized();
		}
		else
		{
			// Wander logic
			goop.WanderTimer -= (float)delta;
			if (goop.WanderTimer <= 0)
			{
				goop.CurrentDirection = new Vector3(
					(float)(random.NextDouble() * 2 - 1),
					0,
					(float)(random.NextDouble() * 2 - 1)
				).Normalized();
				goop.WanderTimer = wanderCooldown;
			}
		}

		// Apply movement force
		if (goop.CurrentDirection.Length() > velocityThreshold)
		{
			body.ApplyCentralForce(-1f * goop.CurrentDirection * moveSpeed);
		}
		else
		{
			// Dampen when idle
			body.ApplyCentralForce(goop.LastDirection);
		}

		// Clamp velocity to max speed
		if (body.LinearVelocity.Length() > maxSpeed)
		{
			body.LinearVelocity = body.LinearVelocity.Normalized() * maxSpeed;
		}

		goop.LastDirection = goop.CurrentDirection;
	}
}
