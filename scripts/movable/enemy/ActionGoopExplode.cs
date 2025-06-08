using Godot;
using System;

public class ActionGoopExplode : EnemyActionStrategy
{
	public override void Act(Enemy enemy, string name, double delta)
	{
		//ensure run is ready
		base.Act(enemy, name, delta);
		if (enemy is not GoopHead goop)
			return;
		var body = goop.GetRigidBody();
		var Player_Location = GameManager.Instance.PlayerManager.Player_Location;
		Explode(goop);
	}
	private void Explode(GoopHead goop)
	{
		if (goop.hasExploded) return;

		if (goop.CurrentDistance < goop.ExplodeRadius && goop.PlayerBody.state != "head")
		{
			goop.hasExploded = true; 
			GD.Print("***************startExplode*****************" + goop.TYPE);
			goop.animatedSprite.Play("Explode");
			//Damage player and enemy
			goop.TakeDamage(100);
			goop.PlayerBody.take_damage(goop.Damage);
			goop.Die("Explode");
		}
	}
}
