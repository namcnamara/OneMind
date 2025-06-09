using Godot;
using System;

public partial class basic_floor : Node3D
{
	public PackedScene TreeScene;
	public PackedScene ProcGenTreeScene;
	public PackedScene WallScene;
	private PackedScene TriggerAreaScene;
	public Vector3 TriggerPosition;
	public bool NeedToTrigger = false;
	
	private LevelBuilderInterface contentBuilder;
	RandomNumberGenerator rand = new RandomNumberGenerator();
	public Player Player_Movable { get; set;}
	public HugoBody3d Player_Body { get; set; }
	public MeshInstance3D meshInstance;
	public BoxMesh boxMesh;
	public CollisionShape3D colShape;
	public BoxShape3D boxShape;
	public Aabb aabb;
	public Vector3 FloorSize;
	public String BuildType {get; set;} = "home";
	
	public override void _Ready()
	{	
		load_assets();
		size_floor();
		populate_boundary();
		populate_walls();
		UpdatePlayerReference();
		populate_contents(BuildType);
		GameManager.Instance.FloorManager.CurrentFloor = BuildType;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if(GameManager.Instance.FloorManager.CurrentFloor == "home" && NeedToTrigger)
		{
			SpawnTriggerArea(TriggerPosition, BuildType); 
			NeedToTrigger = false;
		}
		else if (GameManager.Instance.FloorManager.EnemiesDefeated && NeedToTrigger)
		{
			GD.Print("spawning area");
			SpawnTriggerArea(TriggerPosition, BuildType); 
			NeedToTrigger = false;
		}
	}
	
	private void load_assets()
	{
		TriggerAreaScene = GD.Load<PackedScene>("res://scenes/environment/trigger_area.tscn");
		Player_Movable =  GetNode<Player>("hugo");
		Player_Body = GetNode<HugoBody3d>("hugo/hugo_char");
		meshInstance = GetNode<MeshInstance3D>("ground/MeshInstance3D");
		boxMesh = meshInstance.Mesh as BoxMesh;
		colShape = GetNode<CollisionShape3D>("ground/CollisionShape3D");
		boxShape = colShape.Shape as BoxShape3D;
		TreeScene = GD.Load<PackedScene>("res://scenes/environment/trees/random_tree.tscn");
		ProcGenTreeScene = GD.Load<PackedScene>("res://scenes/environment/trees/ProcGenTree.tscn");
		WallScene = GD.Load<PackedScene>("res://scenes/environment/levels/wall_module.tscn");
		aabb = boxMesh.GetAabb();
		UpdatePlayerReference();
	}
	
	public void UpdatePlayerReference()
	{
		Player_Movable = GetNodeOrNull<Player>("hugo");
		Player_Body = GetNodeOrNull<HugoBody3d>("hugo/hugo_char");
		GD.Print("basic_floor: Player references updated.");
	}
	
	private void size_floor()
	{
		// Get random generator
		rand.Randomize();
		//resize and assign the new size
		float sizeX = rand.RandfRange(20, 40);
		float sizeZ = rand.RandfRange(20, 40);
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
		Node3D tree;
		tree = TreeScene.Instantiate<Node3D>();
		AddChild(tree);
		tree.GlobalPosition = pos;
		tree.Scale = new Vector3(1, heightScale, 1);
	}
	
	private void SpawnOutsideTree(Vector3 pos, float heightScale = 1.0f)
	{
		//Spawns the procgen trees
		Node3D tree;
		tree = ProcGenTreeScene.Instantiate<Node3D>();
		AddChild(tree);
		tree.GlobalPosition = pos;
		tree.Scale = new Vector3(1, heightScale, 1);
	}
	
	private void SpawnWallSegment(Vector3 position, float rotationYDeg, float scaleX, bool bottom_wall = false)
	{
		var wall = WallScene.Instantiate<Node3D>();
		AddChild(wall);

		wall.GlobalTransform = new Transform3D(
			Basis.FromEuler(new Vector3(0, Mathf.DegToRad(rotationYDeg), 0)),
			position
		);

		wall.Scale = new Vector3(scaleX, 1, 1);
		var collisionShape = wall.GetNodeOrNull<CollisionShape3D>("WallBody/Shape");
		if (collisionShape != null && collisionShape.Shape is BoxShape3D boxShape)
		{
			Vector3 originalSize = boxShape.Size;
			boxShape.Size = new Vector3(originalSize.X * scaleX, originalSize.Y*6, originalSize.Z);
		}
	}
	
	private void populate_boundary()
	{
		// This popualates the tree boundary of sprite trees.
		Vector3 center = meshInstance.GlobalTransform.Origin;
		float magicNum = 0.75f;
		Vector3 min_edge = center + aabb.Position;
		Vector3 min_per = min_edge * magicNum;
		Vector3 max_edge = min_edge + aabb.Size;
		Vector3 max_per = max_edge * magicNum;
		float spacing = 6.0f;
		float heightScale = 1.0f;
		int layerCount = 4;

		Random rand = new Random();

		for (float x = min_per.X; x <= max_per.X; x += spacing)
		{
			float randomX = (float)(rand.NextDouble() * (max_per.X - min_per.X) + min_per.X);
			float randomZ = (float)(rand.NextDouble() * (max_per.Z - min_per.Z) + min_per.Z);
			SpawnTree(new Vector3(randomX, center.Y, randomZ));
		}

		for (int i = 0; i < layerCount; i++)
		{
			spacing *= 0.5f;
			heightScale *= 1.5f;
			if (spacing < 0.5f)
				spacing = 3f;

			float t = (i + 1f) / (layerCount + 1f);
			Vector3 min = min_per + (min_edge - min_per) * t;
			Vector3 max = max_per + (max_edge - max_per) * t;
			float x_min = min.X;
			float x_max = max.X;
			float x_offset_for_center = 1.3f;
			float halfway = (x_min + x_max) / 2;
			bool NeedToTriggerSpawnArea = true;

			for (float x = x_min; x <= x_max; x += spacing)
			{
				float offsetZ = (float)(rand.NextDouble() - 0.5f);
				
				
				if (x < (halfway - x_offset_for_center) || x > (halfway + x_offset_for_center))
				{
					SpawnTree(new Vector3(x, center.Y, min.Z + offsetZ), heightScale);
				}
				else
				{
					if (NeedToTriggerSpawnArea && layerCount -1 == i)
					{
						var triggerPos = new Vector3(x, center.Y, min.Z + offsetZ+3);
						TriggerPosition = triggerPos;
						NeedToTriggerSpawnArea = false;
					}
				}
				SpawnTree(new Vector3(x, center.Y, max.Z + offsetZ), heightScale);
			}
			for (float z = min.Z; z <= max.Z; z += spacing)
			{
				float offsetX = (float)(rand.NextDouble() - 0.5f);
				SpawnTree(new Vector3(min.X + offsetX, center.Y, z), heightScale);
				SpawnTree(new Vector3(max.X + offsetX, center.Y, z), heightScale);
			}
		}
		
		//outside wall logic
		float outsideOffset = 1f;
		float outsideSpacing = 1.5f;
		int outsideLayers = 3;
		Vector3 outsideMin = min_edge - new Vector3(outsideOffset, 0, outsideOffset);
		Vector3 outsideMax = max_edge + new Vector3(outsideOffset, 0, outsideOffset);
		for (int layer = 0; layer < outsideLayers; layer++)
		{
			float offsetAmount = layer * outsideSpacing;
			Vector3 min = outsideMin - new Vector3(offsetAmount, 0, offsetAmount);
			Vector3 max = outsideMax + new Vector3(offsetAmount, 0, offsetAmount);

			for (float x = min.X; x <= max.X; x += outsideSpacing)
			{
				float randOffset = (float)(rand.NextDouble() - 0.5f);
				SpawnOutsideTree(new Vector3(x + randOffset, center.Y, min.Z + randOffset));
				//SpawnOutsideTree(new Vector3(x + randOffset, center.Y, max.Z + randOffset));
			}
			for (float z = min.Z; z <= max.Z; z += outsideSpacing)
			{
				float randOffset = (float)(rand.NextDouble() - 0.5f);
				SpawnOutsideTree(new Vector3(min.X + randOffset, center.Y, z + randOffset));
				SpawnOutsideTree(new Vector3(max.X + randOffset, center.Y, z + randOffset));
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
		if (build_type == "enemy0")
		{
			contentBuilder = new EnemyLevel0Builder();
		}
		else if (build_type == "enemy1")
		{
			contentBuilder = new EnemyLevel1Builder();
		}
		else if (build_type == "enemy2")
		{
			contentBuilder = new EnemyLevel2Builder();
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
	
	private void SpawnTriggerArea(Vector3 position, string BuildType)
	{
		if (BuildType == "home")
		{
			// If it a home level being spawned, then an enemy level was beaten; update progress.
			GameManager.Instance.ProgressManager.beat_level();
		}
		//Update progress manager
	
		
		var triggerInstance = TriggerAreaScene.Instantiate<TriggerArea>();
		AddChild(triggerInstance);
		triggerInstance.GlobalPosition = position;
		triggerInstance.UpdateTrigger(BuildType);
		GD.Print("Trigger successfully spawned at: " + position);
	}
}
