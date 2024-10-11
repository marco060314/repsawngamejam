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

	private Vector2 center=new Vector2(1000,500);

	[Export] public float distance = 100.0f; // this determines how far away we want the enemies spawning from the player
	private Player closestPlayer;
	private float closestDistance;
	private  Random random = new Random();
	public float spawnTimer = 0.3f;
	
	private int toSpawn;

	[Signal] public delegate void enemySpawnedEventHandler();
	[Signal] public delegate void enemyDiedEventHandler();
	[Signal] public delegate void allEnemiesEventHandler(int numEnemies);

	public override void _Ready(){
		Timer timer = GetNode<Timer>("SpawnTimer");
		timer.WaitTime = spawnTimer;
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
		EmitSignal(SignalName.allEnemies,toSpawn);
	}

	public void spawnEnemy(){



		//int randomEnemyIndex = random.Next(enemyList.Length);
		
		Random random = new Random();
		int randomChance = random.Next(10);
		int randomEnemyIndex = randomChance == 0 ? 1 : 0;
		
		// select the position of the enemy at random. Base it upon the player's location to avoid
		// unfair spawning.

		float playerX = center.X;
		float playerY = center.Y;
		
		float randomX = 0.0f;
		float randomY = 0.0f;
		
		// repeat if x val is too close to player
		do{
			// to tweak valued: GD.Randomf() * (max - min) + min
			randomX = (float)(random.NextDouble() * (playerX+distance - (playerX-distance)) + (playerX-distance));
			
		}while(randomX < playerX+(distance/2) && randomX > playerX-(distance/2));
		
		// repeat if y val is too  close to player
		do{
			randomY = (float)(random.NextDouble() * (playerY+distance - (playerY-distance)) + (playerY-distance));
		
		}while(randomY > playerY-(distance/2) && randomY < playerY+(distance/2));
		
		GD.Print("Player Position: ("+playerX+","+playerY+")");
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
		return 5;
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
