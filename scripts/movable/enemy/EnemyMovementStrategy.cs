public partial class EnemyMovementStrategy
{
	public string strategy_name;

	public virtual void Move(Enemy enemy, string name, double delta)
	{
		strategy_name = name;
		if (enemy.Health < 0)
			{
				enemy.IsDead = true;
			}
	}
}
