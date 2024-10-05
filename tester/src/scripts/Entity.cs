using Godot;
using System;

public partial class Entity : CharacterBody2D
{
	protected Vector2 direction;
	protected Vector2 position;
	protected Vector2 velocity;
	protected float rotation;

	private bool hit;

	[Export] protected float speed;
	[Export] protected float friction;
	[Export] protected float acceleration;
	[Export] protected float health;

	protected Entity(float speed, float friction, float acceleration, float health){
		this.speed = speed;
		this.friction = friction;
		this.acceleration = acceleration;
		this.health = health;
		direction = new Vector2(0, 0);
		hit = false;
	}

	public void updateDirection(Vector2 direction){
		this.direction = direction;
	}

	public void Hit(bool hit, float damage){
		this.hit = hit;
		if (this.hit == true){
			health -= damage;
			this.hit = false;
		}
	}

	public override void _PhysicsProcess(double delta){
		velocity = Velocity;
		rotation = Rotation;
		position = Position;

		// Get the input direction and handle the movement/deceleration.
		if (direction != Vector2.Zero)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, direction.X * speed, acceleration);
			velocity.Y = Mathf.MoveToward(Velocity.Y, direction.Y * speed, acceleration);

		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, friction);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, friction);
		}

		// Stop the player from moving diagonally faster than intended.
		if (velocity.Length() > speed)
		{
			velocity = velocity.Normalized() *speed;
		}

		// Using MoveAndCollide to see what we collided with.
		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			velocity = Vector2.Zero;
			GD.Print((Name), " collided with ", ((Node)collision.GetCollider()).Name);
		}

		Rotation = rotation;
		Velocity = velocity;
		MoveAndSlide();
	}
}
