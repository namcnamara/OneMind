using Godot;
using System;
using System.Collections.Generic;

public partial class ProcgenTree : Node3D
{
	partial class TreeComponent : MeshInstance3D
	{
		List<TreeComponent> children;

		RandomNumberGenerator rng;

		bool isBranch;
		uint depth;
		float scale;
		Vector3 direction;
		ShaderMaterial leafMaterial;
		ShaderMaterial trunkMaterial;

		public TreeComponent(uint seed, uint depth)
		{
			Shader lshader = GD.Load<Shader>("res://shaders/leaf.gdshader");
			Shader tshader = GD.Load<Shader>("res://shaders/trunk.gdshader");

			leafMaterial = new ShaderMaterial();
			leafMaterial.Shader = lshader;

			trunkMaterial = new ShaderMaterial();
			trunkMaterial.Shader = tshader;
			rng = new RandomNumberGenerator();
			rng.SetSeed((ulong)seed);
			direction = new Vector3(0.0f, 1.0f, 0.0f);
			scale = 0.1f;
			this.depth = depth;
			isBranch = false;
			children = new List<TreeComponent>();
		}

		public Vector3 RandomDirection(Vector3 baseDirection)
		{
			baseDirection.X += rng.RandfRange(-0.2f, 0.2f) * (float)depth;
			baseDirection.Y += rng.RandfRange(-0.2f, 0.2f) * (float)depth;
			baseDirection.Z += rng.RandfRange(-0.2f, 0.2f) * (float)depth;
			return baseDirection.Normalized();
		}

		
		private Vector3 RandomDomeOffset(float radius = 0.5f)
		{
			
			float theta = rng.RandfRange(0, Mathf.Pi);        
			float phi = rng.RandfRange(0, Mathf.Tau);        
			theta = rng.RandfRange(0, Mathf.Pi / 2);

			// Spherical to Cartesian
			float x = Mathf.Sin(theta) * Mathf.Cos(phi);
			float y = Mathf.Cos(theta); 
			float z = Mathf.Sin(theta) * Mathf.Sin(phi);
			Vector3 offset = new Vector3(x, y, z) * radius;
			return offset;
		}

		public void Grow()
		{
			foreach (var child in children)
			{
				child.Grow();
			}

			if (scale > 1.0f)
			{
				isBranch = true;
				int childCount = rng.RandiRange(2, 4);
				for (int i = 0; i < childCount; i++)
				{
					children.Add(new TreeComponent(rng.Randi(), depth + 1));
				}
			}

			if (!isBranch)
			{
				direction = RandomDirection(direction);
			}

			scale += rng.RandfRange(0.0f, 0.5f);
		}

		public void Reify()
		{
			if (depth > 3)
			{
				var sphereMesh = new SphereMesh();
				sphereMesh.Radius = scale * 1f;
				SetMesh(sphereMesh);
			}
			else if (isBranch)
			{
				var capsuleMesh = new CapsuleMesh();
				capsuleMesh.Height = scale;
				capsuleMesh.Radius = scale / 16.0f;
				SetMesh(capsuleMesh);
				SetMaterialOverride(trunkMaterial);
			}
			else
			{
				var sphereMesh = new SphereMesh();
				sphereMesh.Radius = scale * 1f;
				SetMesh(sphereMesh);
				SetMaterialOverride(leafMaterial);
			}

			Vector3 tipOffset = new Vector3(0.0f, scale / 2.0f, 0.0f);
			Transform = Transform.Translated(tipOffset);

			if (direction.Dot(new Vector3(0.0f, 1.0f, 0.0f)) < 0.99)
			{
				Transform = Transform * Transform3D.Identity.LookingAt(direction);
			}

			foreach (var child in children)
			{
				child.Reify();
				AddChild(child);
				child.SetOwner(this);
				Vector3 offset = tipOffset;
				if (child.depth > 3)
				{
					offset += RandomDomeOffset(0.5f + depth);
				}

				child.Transform = child.Transform.Translated(offset);
			}
		}
	};

	TreeComponent trunk;

	public override void _Ready()
	{
		PackedScene treeBaseScene = GD.Load<PackedScene>("res://scenes/TreeBase.tscn");
		Node3D treeBaseInstance = treeBaseScene.Instantiate() as Node3D;

		trunk = new TreeComponent((uint)GD.Hash(GlobalPosition), 0);
		AddChild(trunk);
		AddChild(treeBaseInstance);

		trunk.SetOwner(this);
		treeBaseInstance.SetOwner(this);

		for (int i = 0; i < 10; i++)
		{
			trunk.Grow();
		}
		trunk.Reify();
	}
}
