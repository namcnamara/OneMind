using Godot;

public class PatrolMovement : MovementStrategy
{
	public new string strategy_name = "patrol";
	private static readonly Vector3[] Waypoints = {
		new Vector3(0, 0, 0),
		new Vector3(5, 0, 0)
	};

	private int _current = 0;

	public void Move(Enemy enemy, double delta)
	{
		Vector3 target = Waypoints[_current];
		Vector3 direction = (target - enemy.GlobalTransform.Origin).Normalized();
		float speed = 2.0f;

		enemy.Translate(direction * speed * (float)delta);

		if (enemy.GlobalTransform.Origin.DistanceTo(target) < 0.2f)
			_current = (_current + 1) % Waypoints.Length;
	}
}
