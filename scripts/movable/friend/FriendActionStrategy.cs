public partial class FriendActionStrategy
{
	public string strategy_name;

	public virtual void Act(Friend friend, string name, double delta)
	{
		strategy_name = name;
	}
}
