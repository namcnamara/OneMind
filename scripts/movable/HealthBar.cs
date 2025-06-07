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

	public void Visit(string FullName, int Health, int MaxHealth)
	{
		// Update fill amount based the on enemy Health / MaxHealth values
		float healthPercent = Health / MaxHealth;
		FillBar.Scale = new Vector3(healthPercent, 1, 1); 
		Label.Text = $"{Health}  /  {MaxHealth}";
		NameLabel.Text = FullName;
		//GD.Print($"updated: {FullName} ({Health}/{MaxHealth})");
	}
	
	private string buildName(Movable entity)
	{
		string action = char.ToUpper(entity.action[0]) + entity.action.Substring(1);
		string movement = char.ToUpper(entity.movement[0]) + entity.movement.Substring(1);
		string name = char.ToUpper(entity.TYPE[0]) + entity.TYPE.Substring(1);
		return $"{action} {movement} {name}";
	}
}
