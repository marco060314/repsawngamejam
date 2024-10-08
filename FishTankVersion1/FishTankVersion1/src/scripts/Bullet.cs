using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export] public int Damage { get; set; } = 10;
	[Export] public float Speed { get; set; } = 1700f;
	[Export] public float Lifetime { get; set; } = 3f; // Lifetime in seconds

	private Timer lifetimeTimer;
	private Vector2 velocity;

	public override void _Ready()
	{
		// Initialize lifetime timer to remove bullet after its lifetime
		lifetimeTimer = new Timer();
		lifetimeTimer.WaitTime = Lifetime;
		lifetimeTimer.OneShot = true;
		lifetimeTimer.Connect("timeout", new Callable(this, nameof(OnLifetimeTimeout)));
		AddChild(lifetimeTimer);
		lifetimeTimer.Start();

		// Connect the area entered signal for collision detection
		Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
	}

	public void SetDirection(Vector2 direction)
	{
		velocity = direction.Normalized() * Speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		// Move the bullet manually without physics influence
		GlobalPosition += velocity * (float)delta;
	}

	private void OnLifetimeTimeout()
	{
		QueueFree(); // Removes the bullet from the scene after the timer ends
	}

	private void OnAreaEntered(Area2D area)
	{
		//if (area is CharacterBody2D body && body is Entity target)
		//{
			//target.Hit(true, Damage); // Deal damage to the target using the Hit method
			//QueueFree(); // Destroy the bullet upon collision
		//}
	}
}
