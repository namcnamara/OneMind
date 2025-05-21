using Godot;
using System;

public class MovementGoopChase : MovementStrategy
{
	private float moveSpeed = 6f;
	private float velocityThreshold = 0.01f;
	private float maxSpeed = 2f;
	private float dampeningFactor = 0.9f;

	public override void Move(Enemy enemy, string name, double delta)
	{
		//ensure run is ready
		base.Move(enemy, name, delta);
		if (enemy is not GoopHead goop)
			return;
		var body = goop.GetRigidBody();
		var Player_Location = GameManager.Instance.Player_Location;
		if (Player_Location == Vector3.Zero) return;
		goop.CurrentDistance = goop.RigidBody.GlobalPosition.DistanceTo(Player_Location);
		goop.CurrentDirection = choose_direction(goop, delta);
	}

	private Vector3 choose_direction(GoopHead goop, double delta)
	{
		bool in_chasing_distance = goop.CurrentDistance < goop.DetectionRadius;
		if (in_chasing_distance && goop.PlayerBody.state != "head")
		{
			goop.CurrentDirection = 1f * (GameManager.Instance.Player_Location - goop.RigidBody.GlobalPosition).Normalized();
		}
		else
		{
			// Wander periodically in different directions indefinately
			goop.WanderTimer -= (float)delta;  
			if (goop.WanderTimer <= 0)
			{
				random_move(goop);
				goop.WanderTimer = goop.WanderCooldown; 
			}
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
	
	private void random_move(GoopHead goop)
	{
		// Random float between -1 and 1
		float x = (float)(goop.random.NextDouble() * 2 - 1);
		float z = (float)(goop.random.NextDouble() * 2 - 1);
		Vector3 newDirection = new Vector3(x, 0, z).Normalized();
		goop.CurrentDirection = newDirection;
	}
}
