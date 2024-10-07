using Godot;
using System;

public partial class Shield : Area2D
{
	[Export] public float size = 100.0f;
	[Export] public int durability = 100;
	[Export] public float pushForce = 500.0f;

	private bool isActive = true;

	public override void _Ready()
	{
		// Set the size of the shield's CollisionShape2D based on the `size` property
		CollisionShape2D collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		if (collisionShape != null && collisionShape.Shape is RectangleShape2D rectangleShape)
		{
			rectangleShape.Size = new Vector2(size, size / 2); // New property in Godot 4.x
		}
	}

	public void ActivatePush()
	{
		if (!isActive) return;

		GD.Print("Shield push activated!");

		// Apply push force to nearby enemies
		foreach (Node body in GetOverlappingBodies())
		{
			if (body is Enemy enemy)
			{
				Vector2 pushDirection = (enemy.GlobalPosition - GlobalPosition).Normalized();
				enemy.Velocity += pushDirection * pushForce; // Adjust velocity directly

			}
		}
	}

	public void AbsorbEnemyHit()
	{
		durability -= 10; // Decrease durability by a set amount (adjust as needed)
		GD.Print("Shield hit! Remaining durability: ", durability);

		if (durability <= 0)
		{
			isActive = false;
			Hide(); // Hide the shield when durability is depleted
			GD.Print("Shield deactivated due to zero durability.");
		}
	}

	public void SetDirection(Vector2 direction)
	{
		Rotation = direction.Angle() + Mathf.Pi / 2; // Rotate to face the direction
	}
	public void destroyShield(){
		QueueFree();
	}
}
