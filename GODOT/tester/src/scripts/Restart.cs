using Godot;
using System;

public partial class Restart : TextureButton
{
	[Signal]
	public delegate void onRestartEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void onPressed(){
		GD.Print("restart pressed, reloading current scene.");
		GetTree().ChangeSceneToFile("res://src/background/betterBackground.tscn");
		GetTree().ReloadCurrentScene();
	}
}
