using Godot;
using System;
using System.Collections.Generic;

public partial class FlockingEnemy : Enemy
{
	// Parameters for flocking behavior
	[Export] public float neighborRadius = 100f;
	[Export] public float separationRadius = 100f;
	[Export] public float maxForce = 10f; // Adjusted for stronger steering
	[Export] public float maxSpeed = 150f; // Adjusted for faster movement
	
	

	private Vector2 velocity = Vector2.Zero;
	private float acceleration = 70f; // Use the same acceleration as in MeleeEnemy

	// Lists to store flockmates and targets
	private List<FlockingEnemy> flockmates = new List<FlockingEnemy>();
	private Player closestPlayer;
	

	public FlockingEnemy() : base(100f, 15f, 23f, 10f)
	{
		// Initialize position if necessary
	}

	public override void _Ready()
	{
		base._Ready();
		AddToGroup("FlockingEnemies");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		UpdateFlockmates();

		Vector2 flockingForce = ComputeFlockingForce();
		Vector2 playerForce = SeekPlayer();

		Vector2 steering = flockingForce + playerForce;
		steering = Limit(steering, maxForce);

		// Update velocity using MoveToward for smooth acceleration
		velocity.X = Mathf.MoveToward(velocity.X, steering.X * maxSpeed, acceleration * (float)delta);
		velocity.Y = Mathf.MoveToward(velocity.Y, steering.Y * maxSpeed, acceleration * (float)delta);
		Velocity = velocity;

		// Update rotation based on velocity
		if (velocity.Length() > 0)
		{
			Rotation = velocity.Angle() + Mathf.Pi / 2;
		}

		// Apply movement and handle collisions
		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			Node collider = (Node)collision.GetCollider();
			if (collider is Player player)
			{
				player.damage(3f);
				Rotation += Mathf.Pi;
				velocity = -velocity;
			}else if (collider is BaseShield shield)
			{
				velocity = -velocity;
			}
		}
	}

	private void UpdateFlockmates()
	{
		var allFlockmates = GetTree().GetNodesInGroup("FlockingEnemies");
		flockmates.Clear();

		foreach (Node node in allFlockmates)
		{
			if (node != this && node is FlockingEnemy enemy)
			{
				float distance = Position.DistanceTo(enemy.Position);
				if (distance < neighborRadius)
				{
					flockmates.Add(enemy);
				}
			}
		}
	}

	private Vector2 ComputeFlockingForce()
	{
		Vector2 separation = Vector2.Zero;
		Vector2 alignment = Vector2.Zero;
		Vector2 cohesion = Vector2.Zero;
		int total = 0;

		foreach (FlockingEnemy mate in flockmates)
		{
			float distance = Position.DistanceTo(mate.Position);

			// Separation
			if (distance < separationRadius && distance > 0)
			{
				Vector2 diff = (Position - mate.Position).Normalized() / distance;
				separation += diff;
			}

			// Alignment and Cohesion
			alignment += mate.velocity;
			cohesion += mate.Position;

			total++;
		}

		if (total > 0)
		{
			// Alignment
			alignment /= total;
			alignment = alignment.Normalized() * maxSpeed;
			alignment -= velocity;
			alignment = Limit(alignment, maxForce);

			// Cohesion
			cohesion /= total;
			Vector2 cohesionForce = Seek(cohesion);
			cohesionForce = Limit(cohesionForce, maxForce);

			// Separation
			separation = separation.Normalized() * maxSpeed;
			separation -= velocity;
			separation = Limit(separation, maxForce);

			// Combine forces with weights
			float separationWeight = 1.5f;
			float alignmentWeight = 1.0f;
			float cohesionWeight = 1.0f;

			return (separation * separationWeight) + (alignment * alignmentWeight) + (cohesionForce * cohesionWeight);
		}

		return Vector2.Zero;
	}

	private Vector2 SeekPlayer()
	{
		float minDistance = float.MaxValue;
		foreach (Player player in targets)
		{
			float distance = Position.DistanceTo(player.getPosition());
			if (distance < minDistance)
			{
				minDistance = distance;
				closestPlayer = player;
			}
		}

		return Seek(closestPlayer.getPosition());
	}

	private Vector2 Seek(Vector2 target)
	{
		Vector2 desired = (target - Position).Normalized() * maxSpeed;
		Vector2 steer = desired - velocity;
		return Limit(steer, maxForce);
	}

	private Vector2 Limit(Vector2 vector, float max)
	{
		if (vector.Length() > max)
		{
			vector = vector.Normalized() * max;
		}
		return vector;
	}
}
