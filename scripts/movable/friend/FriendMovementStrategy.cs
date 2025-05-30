public partial class FriendMovementStrategy
{
	public string strategy_name;

	public virtual void Move(Friend friend, string name, double delta)
	{
		strategy_name = name;
		if (friend.Health < 0)
			{
				friend.IsDead = true;
			}
	}
}
