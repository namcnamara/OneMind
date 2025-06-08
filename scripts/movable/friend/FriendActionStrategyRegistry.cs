using System.Collections.Generic;
public static class FriendActionStrategyRegistry 
{
	private static readonly Dictionary<string, FriendActionStrategy> Strategies = new();

	public static FriendActionStrategy GetStrategy(string key)
	{
		if (!Strategies.TryGetValue(key, out var strategy))
		{
			strategy = CreateStrategy(key);
			Strategies[key] = strategy;
		}
		return strategy;
	}

	private static FriendActionStrategy CreateStrategy(string key)
	{
		return key switch
		{
			//Will always initially defaut to _ since the default name is for movement in enemies
			"explode" => new FriendActionExplode(),
			_ => new FriendActionExplode()
		};
	}
}
