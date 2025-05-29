using Godot;
using System;

public class MovementGoopRandom : EnemyMovementStrategy
{
	private float moveSpeed = 4f;
	private float maxSpeed = 5f;

	public override void Move(Enemy enemy, string name, double delta)
	{
		//ensure run is ready
		base.Move(enemy, name, delta);
		var body =enemy.GetRigidBody();
		var Player_Location = GameManager.Instance.PlayerManager.Player_Location;
		if (Player_Location == Vector3.Zero) return;
		enemy.CurrentDistance = enemy.RigidBody.GlobalPosition.DistanceTo(Player_Location);
		enemy.CurrentDirection = choose_direction(enemy, delta);
	}
	
	private Vector3 choose_direction(Enemy goop, double delta)
	{
		// Wander periodically in different directions indefinately
		goop.WanderTimer -= (float)delta;  
		if (goop.WanderTimer <= 0)
		{
			random_move(goop);
			goop.WanderTimer = goop.WanderCooldown; 
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
	
	private void random_move(Enemy goop)
	{
		// Random float between -1 and 1
		float x = (float)(goop.random.NextDouble() * 2 - 1);
		float z = (float)(goop.random.NextDouble() * 2 - 1);
		Vector3 newDirection = new Vector3(x, 0, z).Normalized();
		goop.CurrentDirection = newDirection;
	}
}
