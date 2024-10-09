using Godot;
using System;

public partial class Enemy : Entity {
	public Player[] targets;
	public Enemy(float speed, float friction, float acceleration, float health) : base(speed, friction, acceleration, health) {
		
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta) {
		if (health <= 0){
			QueueFree();
		}
		base._PhysicsProcess(delta);
	}
}
