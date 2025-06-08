using Godot;
using System;

public class MovementCapSleepy : EnemyMovementStrategy
{
	public float moveSpeed = 3f;
	public float maxSpeed = 5f;
	
	public override void Move(Enemy enemy, string name, double delta)
	{
		//ensure run is ready
		base.Move(enemy, name, delta);
		if (enemy is not RedCap cap)
			return;
		var body = cap.GetRigidBody();
		var Player_Location = GameManager.Instance.PlayerManager.Player_Location;
		if (Player_Location == Vector3.Zero) return;
		cap.CurrentDistance = cap.RigidBody.GlobalPosition.DistanceTo(Player_Location);
		cap.CurrentDirection = choose_direction(cap, delta);
	}

	private Vector3 choose_direction(RedCap cap, double delta)
	{
		
		bool in_chasing_distance = cap.CurrentDistance < cap.DetectionRadius;
		if (in_chasing_distance)
		{
			// Wander periodically in different directions indefinately
			cap.IsSleeping = false;
			cap.WanderTimer -= (float)delta;  
			if (cap.WanderTimer <= 0)
			{
				cap.CurrentDirection = 5f * (GameManager.Instance.PlayerManager.Player_Location - cap.RigidBody.GlobalPosition).Normalized();
				cap.WanderTimer = cap.WanderCooldown; 
			}}
		else
		{
			cap.IsSleeping = true;
			return cap.CurrentDirection;
		}
	
		//Apply momementum
		cap.RigidBody.ApplyCentralForce(cap.CurrentDirection * moveSpeed);	
		//cap.RigidBody.LinearVelocity = cap.RigidBody.LinearVelocity * dampeningFactor;
		// Clamp velocity to max speed
		if (cap.RigidBody.LinearVelocity.Length() > maxSpeed)
		{
			cap.RigidBody.LinearVelocity = cap.RigidBody.LinearVelocity.Normalized() * maxSpeed;
		}
		return cap.CurrentDirection;
	}
	
	private void random_move(Enemy cap)
	{
		// Random float between -1 and 1
		float x = (float)(cap.random.NextDouble() * 2 - 1);
		float z = (float)(cap.random.NextDouble() * 2 - 1);
		Vector3 newDirection = new Vector3(x, 0, z).Normalized();
		cap.CurrentDirection = newDirection;
	}
}
