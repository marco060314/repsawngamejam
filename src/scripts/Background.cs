using Godot;
using System;

public partial class Background : Node2D
{
	public static Difficulty difficulty=Difficulty.EASY;
	public override void _Ready()
	{
		Timer spawnTimer=GetNode<Timer>("EnemySpawner/SpawnTimer");
		if(difficulty==Difficulty.EASY){
			spawnTimer.WaitTime=5;
		}
		if(difficulty==Difficulty.MEDIUM){
			spawnTimer.WaitTime=2;
		}
		if(difficulty==Difficulty.HARD){
			spawnTimer.WaitTime=0.5;
		}
		var click=ResourceLoader.Load("res://assets/cursorclicked.png");
		var arrow=ResourceLoader.Load("res://assets/cursor.png");
		Input.SetCustomMouseCursor(click,Input.CursorShape.PointingHand);
		Input.SetCustomMouseCursor(arrow,Input.CursorShape.Arrow);
	}

	[Signal] 
	public delegate void gameOverWithScoresEventHandler(int p1score,int p2score);
	public void gameOver(){
		int p1score=GetNode<Counter>("Camera/Control/ScoreStats/P1Score").counter;
		int p2score=GetNode<Counter>("Camera/Control/ScoreStats/P2Score").counter;
		EmitSignal(SignalName.gameOverWithScores,p1score,p2score);
	}
}
