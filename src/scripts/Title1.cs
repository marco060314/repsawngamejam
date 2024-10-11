using Godot;
using System;

public partial class Title1 : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Position=new Vector2(-100,Position.Y);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Position.X>=546){
			Position=new Vector2(546,Position.Y);
		}
		Translate(new Vector2(200*(float)delta,0));
	}
}
