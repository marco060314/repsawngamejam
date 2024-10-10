using Godot;
using System;

public partial class Gameover : Control
{
	public static int p1score;
	public static int p2score;
	public static SoundManager soundManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameOver(p1score,p2score);
		var click=ResourceLoader.Load("res://assets/cursorclicked.png");
		var arrow=ResourceLoader.Load("res://assets/cursor.png");
		Input.SetCustomMouseCursor(click,Input.CursorShape.PointingHand);
		Input.SetCustomMouseCursor(arrow,Input.CursorShape.Arrow);
		soundManager=GetNode<SoundManager>("SoundManager");
		soundManager.playSound(Sound.GAME_OVER,0f,1f);
	}

	public void gameOver(int p1score,int p2score){
		GetNode<Counter>("Panel/Score/ShooterScore").set(p1score);
		GetNode<Counter>("Panel/Score/ShieldScore").set(p2score);
	}
}
