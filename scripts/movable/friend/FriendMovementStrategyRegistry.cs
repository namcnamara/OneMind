using System.Collections.Generic;
public static class FriendMovementStrategyRegistry 
{
	private static readonly Dictionary<string, FriendMovementStrategy> Strategies = new();

	public static FriendMovementStrategy GetStrategy(string key)
	{
		if (!Strategies.TryGetValue(key, out var strategy))
		{
			strategy = CreateStrategy(key);
			Strategies[key] = strategy;
		}
		return strategy;
	}

	private static FriendMovementStrategy CreateStrategy(string key)
	{
		return key switch
		{
			"follow" => new FriendMovementFollow(),
			_ => new FriendMovementFollow()
		};
	}
}
