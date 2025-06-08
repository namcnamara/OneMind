using Godot;
using System;
using System.Linq;

public class FriendActionExplode : FriendActionStrategy
{
	private float moveSpeed = 6f;

	public override void Act(Friend friend, string name, double delta)
	{
		//ensure run is ready
		base.Act(friend, name, delta);
		if (friend is not GloopMinion goop)
			return;
		var Player_Location = GameManager.Instance.PlayerManager.Player_Location;
		Explode(goop);
	}
	private void Explode(GloopMinion goop)
	{
		if (goop.hasExploded) return;

		if (goop.CurrentDistanceToClosestEnemy < goop.ExplodeRadius)
		{
			goop.hasExploded = true; 
			GD.Print("***************startExplode*****************" + goop.TYPE);
			goop.animatedSprite.Play("Explode");
			//Damage player and enemy
			goop.TakeDamage(100);
			goop.closestEnemy.TakeDamage(70);
			goop.Die("Explode");
		}
	}
}
