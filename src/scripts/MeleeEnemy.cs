using Godot;
using System;

public partial class MeleeEnemy : Enemy{
	private Player closestPlayer;
	private float closestDistance;
	private Vector2 direction;

	private Vector2 velocity;

	public MeleeEnemy() : base(100f, 15f, 23f, 50f) {
		Position = new Vector2(960, 540);
	}
	public override void _Ready()
	{
		// Find the player in the scene
		//player = GetNode<CharacterBody2D>("../Player");
		base._Ready();
	}
	
	public override void _PhysicsProcess(double delta){
		closestDistance = 10000;
		foreach (Player p in targets){
			if (p.getPosition().DistanceTo(Position) < closestDistance){
				closestDistance = p.getPosition().DistanceTo(Position);
				closestPlayer = p;
			}
		}
		// Get the direction of the player
		direction = (closestPlayer.getPosition() - Position).Normalized();
		Rotation = direction.Angle() + Mathf.Pi / 2;
		// Mave teh enemy towards the player
		velocity.X = Mathf.MoveToward(Velocity.X, direction.X * speed, acceleration);
		velocity.Y = Mathf.MoveToward(Velocity.Y, direction.Y * speed, acceleration);
		Velocity = velocity;

		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			Node n=(Node)collision.GetCollider();
			// GD.Print(n.GetType());
			if(n.GetType()== typeof(Player)){
				((Player)n).damage(10f);
			}
		}

		base._PhysicsProcess(delta);
	}
}
