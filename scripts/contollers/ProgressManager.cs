using Godot;
using System.Collections.Generic;

public partial class ProgressManager : Node
{
	public static ProgressManager Instance { get; private set; }
	public int level = 0;
	public string progress = "";

	public override void _Ready()
	{
		Instance = this;
		progress = "enemy"+level.ToString();
	}
	
	public void beat_level()
	{
		level += 1;
		progress = "enemy"+level.ToString();
		GD.Print("New Level unlocked!", progress);
	}

}
