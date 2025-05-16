using Godot;
using System;

public partial class basic_floor : Node3D
{
	public PackedScene TreeScene;
	public PackedScene WallScene;
	private LevelBuilderInterface contentBuilder;
	
	public MeshInstance3D meshInstance;
	public BoxMesh boxMesh;
	public CollisionShape3D colShape;
	public BoxShape3D boxShape;
	public Aabb aabb;
	public Vector3 FloorSize;

	public override void _Ready()
	{	
		load_assets();
		size_floor();
		populate_boundary();
		populate_walls();
		String build_type = "enemy";
		populate_contents(build_type);
	}
	
	private void load_assets()
	{
		meshInstance = GetNode<MeshInstance3D>("ground/MeshInstance3D");
		boxMesh = meshInstance.Mesh as BoxMesh;
		colShape = GetNode<CollisionShape3D>("ground/CollisionShape3D");
		boxShape = colShape.Shape as BoxShape3D;
		TreeScene = GD.Load<PackedScene>("res://scenes/environment/random_tree.tscn");
		WallScene = GD.Load<PackedScene>("res://scenes/environment/levels/wall_module.tscn");
		aabb = boxMesh.GetAabb();
	}
	
	private void size_floor()
	{
		// Get random generator
		var rand = new RandomNumberGenerator();
		rand.Randomize();
		
		//resize and assign the new size
		float sizeX = rand.RandfRange(40, 80);
		float sizeZ = rand.RandfRange(40, 80);
		Vector3 meshSize = new Vector3(sizeX, 2, sizeZ);
		Vector3 shapeSize = meshSize / 2.0f;
		boxMesh = new BoxMesh();
		boxShape = new BoxShape3D();
		boxMesh.Size = meshSize;
		boxShape.Size = meshSize;
		meshInstance.Mesh = boxMesh;
		colShape.Shape = boxShape;
		
		// set at origin
		meshInstance.Position = Vector3.Zero;
		colShape.Position = Vector3.Zero;
		//update the aabb for spawning
		aabb = boxMesh.GetAabb();
	}
	
	private void SpawnTree(Vector3 pos, float heightScale = 1.0f)
	{
		var tree = TreeScene.Instantiate<Node3D>();
		tree.GlobalPosition = pos;
		tree.Scale = new Vector3(1, heightScale, 1);
		AddChild(tree);
	}
	
	private void SpawnWallSegment(Vector3 position, float rotationYDeg, float scaleX)
	{
		var wall = WallScene.Instantiate<Node3D>();

		AddChild(wall);

		wall.GlobalTransform = new Transform3D(
			Basis.FromEuler(new Vector3(0, Mathf.DegToRad(rotationYDeg), 0)),
			position
		);
		wall.Scale = new Vector3(scaleX, 6, 1);
	}
	
	private void populate_boundary()
	{
		Vector3 center = meshInstance.GlobalTransform.Origin;
		float magicNum = 0.75f;
		Vector3 min_edge = center + aabb.Position;
		Vector3 min_per = min_edge * magicNum;
		Vector3 max_edge = min_edge + aabb.Size;
		Vector3 max_per = max_edge * magicNum;
		float spacing = 6.0f;
		float heightScale = 1.0f;
		int layerCount = 6;

		Random rand = new Random();

		// Populate center with random trees
		for (float x = min_per.X; x <= max_per.X; x += spacing)
		{
			float randomX = (float)(rand.NextDouble() * (max_per.X - min_per.X) + min_per.X);
			float randomZ = (float)(rand.NextDouble() * (max_per.Z - min_per.Z) + min_per.Z);
			SpawnTree(new Vector3(randomX, center.Y, randomZ));
		}

		// Draw perimeter layers growing more dense going outward
		for (int i = 0; i < layerCount; i++)
		{
			spacing *= .5f;
			heightScale *= 1.5f;
			if (spacing < .5f)
				spacing = 3f;
			float t = (i + 1f) / (layerCount + 1f);
			Vector3 min = min_per + (min_edge - min_per) * t;
			Vector3 max = max_per + (max_edge - max_per) * t;
			for (float x = min.X; x <= max.X; x += spacing)
			{
				SpawnTree(new Vector3(x, center.Y, min.Z), heightScale);
				SpawnTree(new Vector3(x, center.Y, max.Z), heightScale);
			}
			for (float z = min.Z; z <= max.Z; z += spacing)
			{
				SpawnTree(new Vector3(min.X, center.Y, z), heightScale);
				SpawnTree(new Vector3(max.X, center.Y, z), heightScale);
			}
		}
	}
	
	private void populate_walls()
	{ 
		Vector3 center = meshInstance.GlobalTransform.Origin;
		Vector3 floorSize = boxMesh.Size;

		// Wall vertical placement
		float wallY = meshInstance.GlobalTransform.Origin.Y + boxMesh.Size.Y / 2.0f;

		float halfX = floorSize.X / 2.0f;
		float halfZ = floorSize.Z / 2.0f;

		// North wall 
		SpawnWallSegment(
			position: new Vector3(center.X, wallY, center.Z + halfZ),
			rotationYDeg: 180f,
			scaleX: floorSize.X
		);

		// South wall
		SpawnWallSegment(
			position: new Vector3(center.X, wallY, center.Z - halfZ),
			rotationYDeg: 0f,
			scaleX: floorSize.X
		);

		// East wall
		SpawnWallSegment(
			position: new Vector3(center.X + halfX, wallY, center.Z),
			rotationYDeg: -90f,
			scaleX: floorSize.Z
		);

		// West wall
		SpawnWallSegment(
			position: new Vector3(center.X - halfX, wallY, center.Z),
			rotationYDeg: 90f,
			scaleX: floorSize.Z
		);
	}
	
	private void populate_contents(String build_type)
	{
		if (build_type == "enemy")
		{
			contentBuilder = new EnemyLevelBuilder();
		}
		else if (build_type == "home")
		{
			contentBuilder = new HomeLevelBuilder();
		}
		else
			contentBuilder = new HomeLevelBuilder();
		Vector3 center = meshInstance.GlobalTransform.Origin;
		Vector3 size = boxMesh.Size;
		contentBuilder.Build(this, center, size);
		
	}
	
}
