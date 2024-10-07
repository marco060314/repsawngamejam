using Godot;
using System;

public partial class Camera : Camera2D
{
	[Export] public NodePath Player1Path;  // Drag your first player node here
	[Export] public NodePath Player2Path;  // Drag your second player node here

	private Node2D player1;
	private Node2D player2;

	public override void _Ready()
	{
		// Assign player nodes based on provided paths
		player1 = GetNode<Node2D>(Player1Path);
		player2 = GetNode<Node2D>(Player2Path);
	}

	public override void _Process(double delta)
	{
		if (player1 != null && player2 != null)
		{
			// Calculate the midpoint between the two players
			Vector2 midpoint = (player1.GlobalPosition + player2.GlobalPosition) / 2;

			// Update the camera's position to follow the midpoint
			GlobalPosition = midpoint;
		}
	}
}
