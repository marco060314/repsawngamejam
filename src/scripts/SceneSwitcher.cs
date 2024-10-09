using Godot;
using System;

public partial class SceneSwitcher: Node
{
	public void gameOverWithScores(int p1score,int p2score){
		Gameover.p1score=p1score;
		Gameover.p2score=p2score;
        Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
		GetTree().ChangeSceneToFile("res://src/background/gameover.tscn");
	}
	public void startGame(Difficulty d){
		Background.difficulty=d;
        Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
		GetTree().ChangeSceneToFile("res://src/background/background.tscn");
	}
}
