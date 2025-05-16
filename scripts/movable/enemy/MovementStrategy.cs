public partial class MovementStrategy
{
	public string strategy_name;

	public virtual void Move(Enemy enemy, string name, double delta)
	{
		strategy_name = name;
	}
}
