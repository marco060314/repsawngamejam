using Godot;
using System;

public partial class BaseShield : CharacterBody2D
{
	[Export] public int Durability { get; set; } = 100;
	[Export] public Vector2 Size { get; set; } = new Vector2(50, 10);
	[Export] public float PushForce { get; set; } = 500f; // Common push force for all shields

	protected Node2D owner; // The player or enemy this shield is attached to
	private Area2D detectionArea;

	public override void _Ready()
	{
		// Set up CollisionShape2D size
		if (GetNodeOrNull<CollisionShape2D>("CollisionShape2D") is CollisionShape2D shape)
		{
			if (shape.Shape is RectangleShape2D rectangle)
				rectangle.Size = Size;
		}

		// Initialize Area2D for detecting overlapping bodies
		detectionArea = GetNode<Area2D>("Area2D");
		detectionArea.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	public void LockToOwner(Node2D ownerNode)
	{
		owner = ownerNode;
		UpdateShieldPositionAndRotation();
	}

	public override void _Process(double delta)
	{
		if (owner != null)
			UpdateShieldPositionAndRotation();
	}

	protected void UpdateShieldPositionAndRotation()
	{
		GlobalPosition = owner.GlobalPosition;
		Rotation = owner.Rotation;
	}

	// Shared push functionality
	public virtual void ActivatePush()
	{
		// Detect and apply push to all bodies within Area2D
		foreach (Node2D target in detectionArea.GetOverlappingBodies())
		{
			if (target != owner)
			{
				Vector2 pushDirection = (target.GlobalPosition - GlobalPosition).Normalized();

				if (target is RigidBody2D rigidBodyTarget)
					rigidBodyTarget.ApplyImpulse(Vector2.Zero, pushDirection * PushForce);
				else if (target is CharacterBody2D characterBodyTarget)
					characterBodyTarget.Velocity += pushDirection * PushForce;
			}
		}
	}

	// Called when an overlapping body enters the Area2D
	private void OnBodyEntered(Node2D body)
	{
		if (body is Entity target && target != owner)
		{
			Vector2 pushDirection = (target.GlobalPosition - GlobalPosition).Normalized();
			// Apply push effect immediately or flag for further interaction
		}
	}

	public virtual void TakeDamage(int damage)
	{
		Durability -= damage;
		if (Durability <= 0)
			QueueFree();
	}
}
