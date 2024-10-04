using Godot;
using System;

public partial class EnemyTest : CharacterBody2D
{
	[Export] public float Speed = 200f; 
	private CharacterBody2D player;
	
	public override void _Ready()
	{
		// Find the player in the scene
		player = GetNode<CharacterBody2D>("../Player");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (player == null)
		{
			return;
		}
		// Get the direction of the player
		Vector2 direction = (player.GlobalPosition - this.GlobalPosition).Normalized();

		// Mave teh enemy towards the player
		Velocity = direction * Speed;
		MoveAndSlide();
		
	}
	
	
}
