using Godot;
using System;

public partial class Game : Node2D
{
	public override void _Ready(){
		GetNode<SceneSwitcher>("SceneSwitcher").startGame(Difficulty.EASY);
	}
}
