using Godot;
using System;

public partial class Player : CharacterBody2D
{
	// The maximum speed the player can move
	public const float SPEED = 300.0f;
	// More friction = player stops faster (10 drift, 20 is practically instant)
	public const float FRICTION = 15.0f;
	// Acceleration for the player
	public const float ACCELERATION = 23.0f;

	public override void _PhysicsProcess(double delta)
	{
		// Get the mouse position
		Vector2 mousePosition = GetGlobalMousePosition();
		// Get the vector from the player to the mouse
		Vector2 playerToMouse = mousePosition - GlobalPosition;

		Vector2 velocity = Velocity;
		Vector2 direction = Input.GetVector("LEFT", "RIGHT", "UP", "DOWN");
		// get the player's rotation
		float rotation = Mathf.Atan2(direction.Y, direction.X);

		// find the angle between the y axis and the vector playerToMouse
		float angle = Mathf.Atan2(playerToMouse.Y, playerToMouse.X) + Mathf.Pi / 2;

		// Get the input direction and handle the movement/deceleration.
		if (direction != Vector2.Zero)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, direction.X * SPEED, ACCELERATION);
			velocity.Y = Mathf.MoveToward(Velocity.Y, direction.Y * SPEED, ACCELERATION);

		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, FRICTION);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, FRICTION);
		}

		// Stop the player from moving diagonally faster than intended.
		if (velocity.Length() > SPEED)
		{
			velocity = velocity.Normalized() * SPEED;
		}

		// Using MoveAndCollide to see what we collided with.
		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			GD.Print("I collided with ", ((Node)collision.GetCollider()).Name);
		}

		Velocity = velocity;
		Rotation = angle;
		MoveAndSlide();
	}
}
