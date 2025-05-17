using System.Collections.Generic;
public static class ActionStrategyRegistry 
{
	private static readonly Dictionary<string, ActionStrategy> Strategies = new();

	public static ActionStrategy GetStrategy(string key)
	{
		if (!Strategies.TryGetValue(key, out var strategy))
		{
			strategy = CreateStrategy(key);
			Strategies[key] = strategy;
		}
		return strategy;
	}

	private static ActionStrategy CreateStrategy(string key)
	{
		return key switch
		{
			//Will always initially defaut to _ since the defautl name is for movement
			"explode" => new ActionGoopExplode(),
			"bump" => new ActionGoopBump(),
			_ => new ActionGoopBump()
		};
	}
}
