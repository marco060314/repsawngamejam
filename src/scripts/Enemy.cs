using Godot;
using System;

public partial class Enemy : Entity {
	public Player[] targets;
	public Enemy(float speed, float friction, float acceleration, float health) : base(speed, friction, acceleration, health) {
		
	}

	public float flashRedDelay;
	public const float FLASH_RED=0.1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		base._Ready();
	}

    public override void Damage(float damage)
    {
		flashRedDelay=FLASH_RED;
		Modulate=new Color(1,0.1f,0.1f);
        base.Damage(damage);
    }

    public override void handleDeath(){
		GD.Print("inside handleDeath()::Enemy");
		Background.soundManager.playSound(Sound.ENEMY_DEATH,-25f,1f);
		if(GetParent() is EnemySpawner){
			((EnemySpawner)GetParent()).died();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta) {
		if(flashRedDelay>0){
			flashRedDelay-=(float)delta;
		}
		else{
			Modulate=new Color(1,1,1);
		}
		if (health <= 0){
			QueueFree();
		}
		base._PhysicsProcess(delta);
	}
}
