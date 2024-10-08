using Godot;
using System;

public partial class Camera : Camera2D
{
	[Export] public NodePath Player1Path;  // Drag your first player node here
	[Export] public NodePath Player2Path;  // Drag your second player node here

	private Node2D player1;
	private Node2D player2;
	private float zoom;
	private Vector2 z1; 
	private Vector2 z2;

	public override void _Ready()
	{
		// Assign player nodes based on provided paths
		player1 = GetNode<Node2D>(Player1Path);
		player2 = GetNode<Node2D>(Player2Path);
		z1 = new Vector2(1f, 1f);
		//z2 = new Vector2(0.5f, 0.5f);
		Zoom = z1;
	}

	public override void _Process(double delta)
	{
		if (player1 != null && player2 != null)
		{
			// Calculate the midpoint between the two players
			Vector2 midpoint = (player1.GlobalPosition + player2.GlobalPosition) / 2;

			// Update the camera's position to follow the midpoint
			GlobalPosition = midpoint;
			if (player1.GlobalPosition.DistanceTo(player2.GlobalPosition) < 1200)
			{
				Zoom = z1;
			}
			else
			{
				//Zoom = z2;
			}
		}
	}
}
