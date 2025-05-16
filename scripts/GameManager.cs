using Godot;

public partial class GameManager : Node
{
	//GameManager is a singleton
	public static GameManager Instance { get; private set; }
	//player is a singleton
	public Player Player_Movable { get; set; }
	public HugoBody3d Player_body { get; set; }

	public override void _Ready()
	{
		Instance = this;
	}
}
