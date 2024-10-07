using Godot;
using System;

public partial class Menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var click=ResourceLoader.Load("res://assets/cursorclicked.png");
		var arrow=ResourceLoader.Load("res://assets/cursor.png");
		Input.SetCustomMouseCursor(click,Input.CursorShape.PointingHand);
		Input.SetCustomMouseCursor(arrow,Input.CursorShape.Arrow);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void onPressed(){
		GD.Print("Start Button Pressed!");
		GetTree().ChangeSceneToFile("res://src/background/betterBackground.tscn");
		DifficultySelector ds=GetNode<DifficultySelector>("DifficultySelector");
		Difficulty d=ds.getDifficulty(); //somehow pass the difficulty to the betterBackground.tscn scene
	}
}
