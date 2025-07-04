using System.Collections.Generic;
public static class EnemyMovementStrategyRegistry 
{
	// This registry is generic and doesn't depend on the type of enemy
	private static readonly Dictionary<string, EnemyMovementStrategy> Strategies = new();

	public static EnemyMovementStrategy GetStrategy(string key)
	{
		if (!Strategies.TryGetValue(key, out var strategy))
		{
			strategy = CreateStrategy(key);
			Strategies[key] = strategy;
		}
		return strategy;
	}

	private static EnemyMovementStrategy CreateStrategy(string key)
	{
		return key switch
		{
			"chase" => new MovementGoopChase(),
			"random" => new MovementGoopRandom(),
			"sleepy" => new MovementCapSleepy(),
			_ => new MovementGoopChase()
		};
	}
}
