using System.Collections.Generic;

public static class MovementStrategyRegistry 
{
	private static readonly Dictionary<string, MovementStrategy> Strategies = new();

	public static MovementStrategy GetStrategy(string key)
	{
		if (!Strategies.TryGetValue(key, out var strategy))
		{
			strategy = CreateStrategy(key);
			Strategies[key] = strategy;
		}

		return strategy;
	}

	private static MovementStrategy CreateStrategy(string key)
	{
		return key switch
		{
			"goop_chase" => new GoopChaseMovement(),
			"patrol" => new PatrolMovement(),
			"idle" => new GoopChaseMovement(),
			_ => new GoopChaseMovement()
		};
	}
}
