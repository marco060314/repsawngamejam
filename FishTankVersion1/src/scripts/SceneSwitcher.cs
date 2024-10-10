using Godot;
using System;

public partial class SceneSwitcher: Node
{
	public void gameOverWithScores(int p1score,int p2score){
		Gameover.p1score=p1score;
		Gameover.p2score=p2score;
		resetCursors();
		GetTree().ChangeSceneToFile("res://src/background/gameover.tscn");
	}
	public void startGame(Difficulty d){
		Background.difficulty=d;
		resetCursors();
		GetTree().ChangeSceneToFile("res://src/background/background.tscn");
	}

	public void resetCursors(){
		var click=ResourceLoader.Load("res://assets/cursorclicked.png");
		var arrow=ResourceLoader.Load("res://assets/cursor.png");
		Input.SetCustomMouseCursor(click,Input.CursorShape.PointingHand);
		Input.SetCustomMouseCursor(arrow,Input.CursorShape.Arrow);
		Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
	}
}
