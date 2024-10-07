using Godot;
using System;

public partial class MenuBtn : TextureButton
{
	[Signal]
	public delegate void onMainMenuEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	public void onPressed(){
		GD.Print("main menu clicked.");
		GetTree().ChangeSceneToFile("res://src/background/mainmenu.tscn");
	}
}
