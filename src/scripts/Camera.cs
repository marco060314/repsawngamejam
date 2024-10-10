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

	private FastNoiseLite fnl;
	private Vector2 basePosition;
	private const float ShakeTime=0.4f;
	private const float ShakeAmount=5000.0f;
	private float screenShakeDelay;

	public override void _Ready()
	{
		// Assign player nodes based on provided paths
		player1 = GetNode<Node2D>(Player1Path);
		player2 = GetNode<Node2D>(Player2Path);
		z1 = new Vector2(1f, 1f);
		//z2 = new Vector2(0.5f, 0.5f);
		Zoom = z1;
		fnl=new();
	}

	public override void _Process(double delta)
	{
		if (player1 != null && player2 != null)
		{
			// Calculate the midpoint between the two players
			Vector2 midpoint = (player1.GlobalPosition + player2.GlobalPosition) / 2;

			// Update the camera's position to follow the midpoint
			GlobalPosition = midpoint;
			basePosition=midpoint;
			if (player1.GlobalPosition.DistanceTo(player2.GlobalPosition) < 1200)
			{
				Zoom = z1;
			}
		}

		if(screenShakeDelay>0f){
			GlobalPosition=basePosition+new Vector2(getNoise(0),getNoise(1));
			screenShakeDelay-=(float)delta;
		}
	}

	public float getNoise(int seed){
		fnl.Seed=seed;
		return fnl.GetNoise1D(GD.Randf()*screenShakeDelay)*ShakeAmount;
	}

	public void shake(){
		basePosition=GlobalPosition;
		screenShakeDelay=ShakeTime;
	}
}
