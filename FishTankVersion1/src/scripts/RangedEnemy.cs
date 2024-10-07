using Godot;
using System;

public partial class RangedEnemy : Enemy
{
	[Export] private Player[] players;
	[Export] private Area2D shootrange;
	[Export] private Area2D runawayrange;

	private Player closestPlayer;
	private float closestDistance;
	private Vector2 direction;
	private Vector2 velocity;
	private int numDanger;
	private Vector2 runVector;
	private float runAngle;
	private bool runMode;

	public RangedEnemy() : base(50f, 15f, 23f, 50f)	{
		Position = new Vector2(960, 720);
	}
	public override void _Ready()
	{
		numDanger = 0;
		runMode = false;
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		closestDistance = 10000;
		foreach (Player p in players){
			if (p.getPosition().DistanceTo(Position) < closestDistance)
			{
				closestDistance = p.getPosition().DistanceTo(Position);
				closestPlayer = p;
			}
		}

		// Move towards closest player
		direction = (closestPlayer.getPosition() - Position).Normalized();
		Rotation = direction.Angle() + Mathf.Pi / 2;
		if (runMode == false){
			velocity.X = Mathf.MoveToward(Velocity.X, direction.X * speed, acceleration);
			velocity.Y = Mathf.MoveToward(Velocity.Y, direction.Y * speed, acceleration);
		}
		
		// detect all bodies within the  detection range
		var bodies = shootrange.GetOverlappingBodies();
		foreach (var body in bodies){
			// If player is detected, stop  moving and begin shooting
			if (body == closestPlayer && numDanger == 0){
				velocity.X = Mathf.MoveToward(Velocity.X, 0, friction);
				velocity.Y = Mathf.MoveToward(Velocity.Y, 0, friction);
			}
		}
		var bodies2 = runawayrange.GetOverlappingBodies();
		foreach (var body in bodies2){
			foreach (Player p in players){
				if (body == p){
					numDanger++;
				}
			}
		}

		if (numDanger == 1){
			velocity.X = Mathf.MoveToward(Velocity.X, -direction.X * speed, acceleration);
			velocity.Y = Mathf.MoveToward(Velocity.Y, -direction.Y * speed, acceleration);
		} 
		else if (numDanger == 2){
			runMode = true;
			runVector = players[0].getPosition() - players[1].getPosition();
			runVector = runVector.Normalized();
			runAngle = runVector.Angle() + Mathf.Pi / 2;
			direction = new Vector2(Mathf.Sin(runAngle), Mathf.Cos(runAngle));
			direction = direction.Normalized();
			velocity.X = Mathf.MoveToward(Velocity.X, direction.X * speed, acceleration);
			velocity.Y = Mathf.MoveToward(Velocity.Y, direction.Y * speed, acceleration);
		}
		if (runMode == true){
			Rotation = velocity.Angle() + Mathf.Pi / 2;
		}

		numDanger = 0;
		runMode = false;
		Velocity = velocity;
		base._PhysicsProcess(delta);
	}
}
