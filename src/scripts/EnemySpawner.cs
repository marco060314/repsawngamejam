using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	// set up all globals
	private PackedScene[] enemyList = new PackedScene[]
	{
		(PackedScene)GD.Load("res://src/scripts/FlockingEnemy.tscn"),
		(PackedScene)GD.Load("res://src/scripts/RangedEnemy.tscn")
	};
	[Export] Player[] players;
	private Vector2 center; // average position of players

	[Export] public float distance = 1600.0f; // this determines how far away we want the enemies spawning from the player
	private float randAngle;
	private  Random random = new Random();
	public float spawnTimer = 0.3f;
	private int flockSize;
	private int toSpawn;


	[Signal] public delegate void enemySpawnedEventHandler();
	[Signal] public delegate void enemyDiedEventHandler();
	[Signal] public delegate void allEnemiesEventHandler(int numEnemies);

	public override void _Ready(){
		Timer timer = GetNode<Timer>("SpawnTimer");
		timer.WaitTime = spawnTimer;
		flockSize = 5;
		center = new Vector2(0, 0);
		center.X = (players[0].GlobalPosition.X + players[1].GlobalPosition.X) / 2;
		center.Y = (players[0].GlobalPosition.Y + players[1].GlobalPosition.Y) / 2;
	}
		
	public void onSpawnTimerTimeout(){
		if(toSpawn==0) return;
		// select a random enemy to spawn next
		spawnEnemy();
	}



	public void newRound(int round) {
		//EmitSignal(SignalName.allEnemies,toSpawn);
		//EmitSignal("upgrade_triggered");

		Difficulty d=Background.difficulty;
		int k;
		if(d==Difficulty.EASY){
			k=1;
		}
		else if(d==Difficulty.MEDIUM){
			k=2;
		}
		else{
			k=3;
		}

		toSpawn=round*k;
		if (round % 3 == 0){
			flockSize++;
		}

		EmitSignal(SignalName.allEnemies,toSpawn);
	}

	public void spawnEnemy(){



		//int randomEnemyIndex = random.Next(enemyList.Length);
		
		Random random = new Random();
		int randomChance = random.Next(10);
		int randomEnemyIndex = randomChance == 0 ? 1 : 0;
		
		// select the position of the enemy at random. Base it upon the player's location to avoid
		// unfair spawning.

		
		float randomX = 0.0f;
		float randomY = 0.0f;

		randAngle = (float)random.NextDouble() * 2 * Mathf.Pi;

		randomX = (float) Mathf.Cos(randAngle) * distance + center.X;
		randomY = (float) Mathf.Sin(randAngle) * distance + center.Y;
		GD.Print("Enemy Spawned at: ("+randomX+","+randomY+")");
		//GD.Print("Difference Between Them: ("+randomX+","+randomY+")")
		// set the randomized position

		PackedScene selectedEnemy = enemyList[randomEnemyIndex];
		Vector2 randomPosition = new Vector2(randomX, randomY);

		if(randomEnemyIndex==0){//flocking
			int flockCount=random.Next(1,Math.Min(toSpawn+1,maxFlockSize()));
			for(int i=0; i<flockCount; i++){
				createEnemy(selectedEnemy,randomPosition);
			}
		}
		else{//ranged
			createEnemy(selectedEnemy,randomPosition);
		}
	}

	public int maxFlockSize(){
		return flockSize;
	}

	public void createEnemy(PackedScene selectedEnemy,Vector2 position){
		if(toSpawn==0) return;
		toSpawn--;
		// create an instance of the enemy
		Node instance = selectedEnemy.Instantiate();
		Player[] players=new Player[2];
		players[0]=GetParent().GetNode<Player>("Player1");
		players[1]=GetParent().GetNode<Player>("Player2");
		((Enemy)instance).targets=players;

		// implement slightly random positions!
		int range=50;
		position+=new Vector2(random.Next(--range,range),random.Next(--range,range));
		((Node2D)instance).Position = position;
		
		EmitSignal(SignalName.enemySpawned);
		// add the enemy to the scene
		AddChild(instance);
	}

	public void died() {
		GD.Print("inside died()::EnemySpawner");
		EmitSignal(SignalName.enemyDied);
	}
}
