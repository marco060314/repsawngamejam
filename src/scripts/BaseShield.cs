using Godot;
using System;

public partial class BaseShield : CharacterBody2D
{
	[Export] public int Durability { get; set; } = 100;
	[Export] public float Size { get; set; } = 3f;
	[Export] public float PushForce { get; set; } = 10000f; // Common push force for all shields

	protected Node2D owner; // The player or enemy this shield is attached to
	private Area2D detectionArea;
	private Vector2 rotationVector = new Vector2(0, 0);

	public override void _Ready()
	{
		this.TopLevel = true;
		// Set up CollisionShape2D size
		// if (GetNodeOrNull<CollisionShape2D>("CollisionShape2D") is CollisionShape2D shape)
		// {
		// 	shape.Scale *= Size;
		// }
		// if (GetNodeOrNull<Sprite2D>("Sprite2D") is Sprite2D shape2)
		// {
		// 	shape2.Scale *= Size;
		// }

		this.Scale = new Vector2(Size, Size);
		// Initialize Area2D for detecting overlapping bodies
		detectionArea = GetNode<Area2D>("Area2D");
	}

	public void LockToOwner(Node2D ownerNode)
	{
		owner = ownerNode;
	}

	public override void _Process(double delta)
	{
		GlobalPosition = owner.GlobalPosition + rotationVector * 0;
		
		// Use the right joystick for Player 2 rotation (raw input)
		Vector2 rightStickDirection = new Vector2(
				Input.GetActionRawStrength("P2_LOOK_RIGHT", true) - Input.GetActionRawStrength("P2_LOOK_LEFT", true),
				Input.GetActionRawStrength("P2_LOOK_DOWN", true) - Input.GetActionRawStrength("P2_LOOK_UP", true));

		// Calculate the angle of the right joystick in radians
		float rightStickAngle = rightStickDirection.Angle() + Mathf.Pi / 2;
		float diff = 0;

		// If the right joystick is moved beyond the tolerance threshold
		if (rightStickDirection.Length() > 0.05f)
		{
			diff = (float)delta * Mathf.Abs(Rotation - rightStickAngle) * 2;
			Rotation = Mathf.LerpAngle(Rotation, rightStickAngle, Mathf.Max(diff, 0.1f));
			rotationVector.X = Mathf.Cos(Rotation - Mathf.Pi / 2);
			rotationVector.Y = Mathf.Sin(Rotation - Mathf.Pi / 2);
		}
	}
	
	protected void UpdateShieldPositionAndRotation()
	{
		GlobalPosition = owner.GlobalPosition;
	}

	public virtual void SetShieldRotation(float rotationAngle)
	{
		Rotation = rotationAngle; 
	}

	// Offset logic (if needed)
	public void SetShieldOffset(Vector2 ownerPosition)
	{
		GlobalPosition = ownerPosition;
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

	public virtual void TakeDamage(int damage)
	{
		Durability -= damage;
		if (Durability <= 0){
			//QueueFree();
		}

	}
}
