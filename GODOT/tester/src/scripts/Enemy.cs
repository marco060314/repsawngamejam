using Godot;
using System;



public partial class Enemy : Entity
{

	[Export]
public CharacterBody2D player;

	public Enemy() : base(100f,15f,23f)
	{
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 position=player.Position;
		updateDirection(new Vector2(position.X-Position.X,position.Y-Position.Y));
		base._PhysicsProcess(delta);
	}
}
