using Godot;
using System;

public partial class Entity : CharacterBody2D
{
	// The maximum speed the entity can move
	public float speed = 300.0f;
	// More friction = entity stops faster (10 drift, 20 is practically instant)
	public float friction = 15.0f;
	// Acceleration for the entity
	public float acceleration = 23.0f;

    public Vector2 direction;

    protected Entity(float speed,float friction,float acceleration){
        this.speed=speed;
        this.friction=friction;
        this.acceleration=acceleration;
        direction=new Vector2(0,0);
    }

    public void updateDirection(Vector2 direction){
        this.direction=direction;
    }

    public override void _PhysicsProcess(double delta){
		Vector2 velocity = Velocity;

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
		Velocity = velocity;
		MoveAndSlide();
	}

	public void checkCollision(double delta){
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i).GetCollider();
			if(collision.GetType()==typeof(Player)){
				((Player)collision).die();
			}
		}
	}

}