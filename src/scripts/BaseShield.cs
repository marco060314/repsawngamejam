using Godot;
using System;

public partial class BaseShield : CharacterBody2D
{
	[Export] public int Durability { get; set; } = 100;
	[Export] public Vector2 Size { get; set; } = new Vector2(15, 5);
	[Export] public float PushForce { get; set; } = 100f; // Common push force for all shields

	protected Node2D owner; // The player or enemy this shield is attached to
	private Area2D detectionArea;

	public override void _Ready()
	{
		// Set up CollisionShape2D size
		if (GetNodeOrNull<CollisionShape2D>("CollisionShape2D") is CollisionShape2D shape)
		{
			//shape.Scale = Size;
		}
		if (GetNodeOrNull<Sprite2D>("Sprite2D") is Sprite2D shape2)
		{
			//shape2.Scale = new Vector2(Size.X * 9.263391059f, Size.Y * 4.385752688f);
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
		if (owner == null) return;
		GlobalPosition = owner.GlobalPosition ;//+ offset;
	}

	public virtual void SetShieldRotation(float rotationAngle)
	{
		Rotation = rotationAngle; 
	}

	// Offset logic (if needed)
	public void SetShieldOffset(Vector2 ownerPosition)
	{
		float offsetDistance = 60; // Adjust this distance as needed
		Vector2 offset = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation)) * offsetDistance;
		GlobalPosition = ownerPosition + offset;
	}
	

	public virtual void ActivatePush()
	{
		// Detect and apply push to all bodies within Area2D
		foreach (Node2D target in detectionArea.GetOverlappingBodies())
		{
			if (target != owner)
			{
				Vector2 pushDirection = (target.GlobalPosition - GlobalPosition).Normalized();

				if (target is RigidBody2D rigidBodyTarget){
					rigidBodyTarget.ApplyImpulse(Vector2.Zero, pushDirection * PushForce);
				}
					
				else if (target is CharacterBody2D characterBodyTarget){
					characterBodyTarget.Velocity += pushDirection * PushForce;
				}
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
