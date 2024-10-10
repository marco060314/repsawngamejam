using Godot;
using System;

public partial class EnemiesLeft : Counter
{
	private int enemiesLeftInRound;

	[Signal] public delegate void signalNewRoundEventHandler();

	EnemiesLeft(): base("Enemies Left:"){}

	public override void _Ready() {
		base._Ready();
	}

	public void enemySpawned(){
		add(1);
	}
	public void enemyDied() {
		GD.Print("inside enemyDied");
		add(-1);
		enemiesLeftInRound--;
		if(enemiesLeftInRound==0){
			EmitSignal(SignalName.signalNewRound);
		}
	}
	public void newRoundEnemies(int numEnemies){
		enemiesLeftInRound=numEnemies;
	}
}
