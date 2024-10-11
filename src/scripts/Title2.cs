using Godot;
using System;

public partial class Title2 : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Position=new Vector2(1440,Position.Y);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Position.X<=801){
			Position=new Vector2(801,Position.Y);
		}
		Translate(new Vector2(-200*(float)delta,0));
	}
}
