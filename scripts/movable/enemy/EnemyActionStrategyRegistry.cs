using System.Collections.Generic;
public static class EnemyActionStrategyRegistry 
{
	private static readonly Dictionary<string, EnemyActionStrategy> Strategies = new();

	public static EnemyActionStrategy GetStrategy(string key)
	{
		if (!Strategies.TryGetValue(key, out var strategy))
		{
			strategy = CreateStrategy(key);
			Strategies[key] = strategy;
		}
		return strategy;
	}

	private static EnemyActionStrategy CreateStrategy(string key)
	{
		return key switch
		{
			//Will always initially defaut to _ since the default name is for movement in enemies
			"Explode" => new ActionGoopExplode(),
			"bump" => new ActionGoopBump(),
			_ => new ActionGoopBump()
		};
	}
}
