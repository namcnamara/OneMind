using Godot;
using System;

public partial class HealthBar : Node3D, HealthBarVisitor
{
	private Node3D FillBar;
	private Node3D BackBar;
	private Label3D Label;
	private Label3D NameLabel;

	public override void _Ready()
	{
		FillBar = GetNode<Node3D>("BackBar/FillBar");
		BackBar = GetNode<Node3D>("BackBar");
		Label = GetNode<Label3D>("BackBar/Label");
		NameLabel = GetNode<Label3D>("BackBar/NameLabel");
	}

	public void Visit(Enemy enemy)
	{
		// Update fill amount based the on enemy Health / MaxHealth values
		GD.Print("Attempt");
		float healthPercent = (float)enemy.Health / enemy.MaxHealth;
		FillBar.Scale = new Vector3(healthPercent, 1, 1); 
		Label.Text = $"{enemy.Health}  /  {enemy.MaxHealth}";
		NameLabel.Text = buildName(enemy);
		GD.Print($"updated: {enemy.EnemyName} ({enemy.Health}/{enemy.MaxHealth})");
	}
	
	private string buildName(Enemy enemy)
	{
		string action = char.ToUpper(enemy.action[0]) + enemy.action.Substring(1);
		string movement = char.ToUpper(enemy.movement[0]) + enemy.movement.Substring(1);
		string name = char.ToUpper(enemy.EnemyName[0]) + enemy.EnemyName.Substring(1);
		return $"{action} {movement} {name}";
	}
}
