using Godot;
using System;

public class ActionGoopExplode : ActionStrategy
{
	private float moveSpeed = 6f;
	private float velocityThreshold = 0.01f;
	private float maxSpeed = 6f;
	private float dampeningFactor = 0.9f;

	public override void Act(Enemy enemy, string name, double delta)
	{
		//ensure run is ready
		base.Act(enemy, name, delta);
		if (enemy is not GoopHead goop)
			return;
		var body = goop.GetRigidBody();
		var player_location = GameManager.Instance.Player_location;
	}
}
