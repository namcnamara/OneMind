using Godot;
using System;

public class ActionPricklyBlobSpike : EnemyActionStrategy
{
	private float moveSpeed = 60f;

	public override void Act(Enemy enemy, string name, double delta)
	{
		//	Ensure run is ready
		base.Act(enemy, name, delta);
		if (enemy is not PricklyBlob blob)
			return;
		var body = blob.GetRigidBody();
		var Player_Location = GameManager.Instance.PlayerManager.Player_Location;
		SpikeCharge(blob, delta);
	}
	
	private void SpikeCharge(PricklyBlob blob, double delta)
	{
		// If in Spike cooldown, skip
		if (blob.IsSpikeing)
		{
			blob.SpikeTimer -= (float)delta;  
			if (blob.SpikeTimer <= 0)
			{
				blob.IsSpikeing = false;
				blob.SpikeTimer = blob.SpikeCooldown; 
			}
		}
		else
		{
			// Check if within charge range (but not yet Spikeed)
			if (blob.CurrentDistance < blob.SpikeChargeRadius && blob.PlayerBody.state != "head")
			{
				Vector3 direction = (GameManager.Instance.PlayerManager.Player_Location - blob.RigidBody.GlobalPosition).Normalized();
				blob.CurrentDirection = direction * moveSpeed;
				blob.RigidBody.ApplyCentralForce(blob.CurrentDirection);
				blob.WanderTimer = blob.WanderCooldown;
			}

			// Spike Trigger
			if (blob.CurrentDistance < blob.SpikeRadius && blob.PlayerBody.state != "head")
			{
				blob.IsSpikeing = true;
				blob.SpikeTimer = blob.SpikeCooldown;
				GD.Print("***************Spikein*****************" + blob.TYPE);
				blob.animatedSprite.Play("Spike");
				blob.TakeDamage(50);
				blob.PlayerBody.take_damage(5);
			}
		}
	}
}
