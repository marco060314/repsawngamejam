using Godot;
using System;

public partial class RangedEnemyTest : CharacterBody2D
{
	[Export] public float Speed = 200f; 
	[Export] private CharacterBody2D player;
	[Export] private Area2D detection;
	
	
	public override void _Ready()
	{
		// Find the player in the scene
		//player = GetNode<CharacterBody2D>("../Player");
		
	}
	
	public override void _PhysicsProcess(double delta)
	{
		
		if (player == null){
			return;
		}
		// Get the direction of the player
		Vector2 direction = (player.GlobalPosition - this.GlobalPosition).Normalized();

		// Mave teh enemy towards the player
		Velocity = direction * Speed;
		
		// detect all bodies within the  detection range
		var bodies = detection.GetOverlappingBodies();
		foreach (var body in bodies)
		{
				// If player is detected, stop  moving and begin shooting
				if( ((Node)body).Name == "Player"){
					Vector2 stopMoving = new Vector2(0,0);
					Velocity = stopMoving;
				}
			
			
		}
		
		MoveAndSlide();
	}
	
}
