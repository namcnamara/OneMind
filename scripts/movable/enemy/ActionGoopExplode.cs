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
		Explode(goop);
	}
	private void Explode(GoopHead goop)
	{
		if (goop.hasExploded) return;

		if (goop.CurrentDistance < goop.ExplodeRadius && goop.PlayerBody.state != "head")
		{
			goop.hasExploded = true; 
			goop.health = 0;
			GD.Print("***************startEXPLODE*****************" + goop.EnemyName);
			goop.animatedSprite.Play("explode");
			goop.PlayerBody.take_damage(goop.damage);
				
			// Delay deletion to allow animation to play
			goop.GetTree().CreateTimer(0.5f).Timeout += () =>
			{
				GD.Print("deleted");
				goop.QueueFree();
			};
		}
	}
}
