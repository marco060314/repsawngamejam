using Godot;
using System;

public partial class Gun : Node2D
{
	[Export] public int Damage { get; set; } = 10;
	[Export] public float BulletSpeed { get; set; } = 500f;
	[Export] public float FireRate { get; set; } = 0.5f;
	[Export] public PackedScene BulletScene;  // Add this line to export the Bullet scene
	
	protected Timer fireRateTimer;

	public override void _Ready()
	{
		// Timer to control fire rate
		fireRateTimer = new Timer();
		fireRateTimer.WaitTime = FireRate;
		fireRateTimer.OneShot = true;
		AddChild(fireRateTimer);
	}

	public virtual void Shoot(Vector2 direction)
	{
		if (!fireRateTimer.IsStopped())
			return; // Prevent shooting if the timer is running

		fireRateTimer.Start();

		// Instantiate and configure bullet
 	   if (BulletScene != null)
   		{
			var bullet = BulletScene.Instantiate<Bullet>();
	   	 	bullet.GlobalPosition = GlobalPosition;
	   	 	bullet.SetDirection(direction);
			GetTree().CurrentScene.AddChild(bullet);
		}else{
			GD.Print("BulletScene is not assigned.");
		}

		GD.Print("Gun fired a bullet!");
	}
}
