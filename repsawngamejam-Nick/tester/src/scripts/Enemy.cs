using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Enemy : Entity {
	// Player to follow
	[Export] public Player p;
	[Export] public Bullet b;
	public Enemy() : base(100f, 15f, 23f, 50f) {
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		position = new Vector2(480, 240);
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta) {
		direction = p.getPosition() - position;
		direction = direction.Normalized();
		
		Rotation = direction.Angle() + Mathf.Pi / 2;
		if (health <= 0){
			QueueFree();
		}
		base._PhysicsProcess(delta);
	}
}
