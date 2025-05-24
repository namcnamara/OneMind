public partial class EnemyActionStrategy
{
	public string strategy_name;

	public virtual void Act(Enemy enemy, string name, double delta)
	{
		strategy_name = name;
	}
}
