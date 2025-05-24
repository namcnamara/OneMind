using Godot;
using System;
using ProcGen;

public partial class ProcGenMaze : Node3D
{
	public class MazeTile {
		public bool   IsOuterWall;
		public bool   IsWall;
		public Node3D TileNode;
	}
	
	Grid2D<MazeTile> grid;
	
	PackedScene wall;
	PackedScene floor;
	PackedScene outer_tree;
	int max_x = 40;
	int max_y = 40;
	
	
	public override void _Ready() 
	{
		load_assets();
		draw_boundary();
		populate_contents();
		populate_trees();
	}
	
	public void load_assets()
	{
		wall  = GD.Load<PackedScene>("res://scenes/room_modules/wall_module.tscn" );
		floor = GD.Load<PackedScene>("res://scenes/room_modules/floor_module.tscn");
		outer_tree = GD.Load<PackedScene>("res://scenes/procgen_tree.tscn");
	}
	
	
	public void draw_boundary()
	{
		grid = new Grid2D<MazeTile>(max_x,max_y,true);
		for (int y=0; y<max_y; y++) {
			for (int  x=0; x<max_x; x++) {
				var tile = new MazeTile();
				if (x == 0 || x == max_x - 1 || y == 0 || y == max_y - 1)
					{
					tile.IsOuterWall = true;
					tile.IsWall = true;
					}
				else
					tile.IsWall = false;
				grid.At(new Vector2I(x,y)).Data = tile;
			}
		}
		Random rand = new Random();
		int decider = rand.Next(100);
		if (decider % 2 == 0)
			grid.At(new Vector2I(10,10)).Data.IsWall = true;
			grid.At(new Vector2I(max_x - 10,max_y - 10)).Data.IsWall = true;
		if (decider % 3 == 0)
			grid.At(new Vector2I(max_x - max_x / 2,max_y / 2)).Data.IsWall = true;
	}
	
	public void populate_contents()
	{
		for (int i=0; i<10; i++) {
			grid = grid.Map((cell) => {
				var result = new MazeTile();
				int count = 0;
				var adjCell = cell as IAdjCell<Vector2I,MazeTile>;
				foreach (var adj in adjCell.Adj()) {
					if (adj.Data.IsWall) {
						count++;
					}
				}
				if (!cell.Data.IsWall) {
					result.IsWall = (count==1);
				} else {
					result.IsWall = ((count>0)&&(count<4));
				}
				return result;
			}) as Grid2D<MazeTile>;

			foreach (var cell in grid) {
				if (cell.Data.IsWall) {
					cell.Data.TileNode = wall.Instantiate() as Node3D;
				} else {
					cell.Data.TileNode = floor.Instantiate() as Node3D;
				}
				cell.Data.TileNode.Transform = Transform3D.Identity
					.Translated(new Vector3(
						(float)cell.Location.X * 4.0f,
						0.0f,
						(float)cell.Location.Y * 4.0f
					));
				AddChild(cell.Data.TileNode);
			}
		}
	}
	
	public void populate_trees()
	{
		Random rand = new Random();

		foreach (var cell in grid)
		{
			if (!cell.Data.IsWall)
			{
				// Random chance to spawn a tree, e.g. 10%
				if (rand.NextDouble() < 0.3)
				{
					var treeInstance = outer_tree.Instantiate() as Node3D;
					Vector3 pos = new Vector3(cell.Location.X * 4.0f, 0.0f, cell.Location.Y * 4.0f);
					treeInstance.Transform = Transform3D.Identity.Translated(pos);
					float scaleFactor = 0.1f + (float)rand.NextDouble() * 1.4f;  
					treeInstance.Scale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
					AddChild(treeInstance);
				}
			}
		}
	}
}
