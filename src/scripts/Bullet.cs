using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export] public int Damage { get; set; } = 3;
	[Export] public float Speed { get; set; } = 1700f;
	[Export] public float Lifetime { get; set; } = 3f; // Lifetime in seconds

	private Timer lifetimeTimer;
	private Vector2 velocity;

	public bool hitsPlayers=false;

	public override void _Ready()
	{
		// Initialize lifetime timer to remove bullet after its lifetime
		lifetimeTimer = new Timer();
		lifetimeTimer.WaitTime = Lifetime;
		lifetimeTimer.OneShot = true;
		lifetimeTimer.Connect("timeout", new Callable(this, nameof(OnLifetimeTimeout)));
		AddChild(lifetimeTimer);
		lifetimeTimer.Start();
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

	public void onBodyEntered(Node2D node){
		GD.Print(node.GetType());
		if(hitsPlayers&&node.GetType()==typeof(Player)){
			((Player)node).damage(Damage);
			QueueFree();
		}
		if(!hitsPlayers&&node is Enemy){
			((Entity)node).Damage(Damage);
			QueueFree();
		}
	}
}
