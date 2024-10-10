using Godot;
using System;

public partial class Gun : Node2D
{
	[Export] public int Damage { get; set; } = 10;
	[Export] public float FireRate { get; set; } = 0.5f;
	[Export] public PackedScene BulletScene;  // Add this line to export the Bullet scene
	
	protected Timer fireRateTimer;
	protected Entity owner;
	protected Player p;
	private bool isPlayer = false;
	public override void _Ready()
	{
		this.TopLevel = true;
		// Timer to control fire rate
		fireRateTimer = new Timer();
		fireRateTimer.WaitTime = FireRate;
		fireRateTimer.OneShot = true;
		AddChild(fireRateTimer);
	}

	public void LockToOwner(Entity ownerNode){
		owner = ownerNode;
		try {
			p = (Player) owner;
			isPlayer = true;
		} catch (InvalidCastException e) {
			isPlayer = false;
		}
		UpdatePositionAndRotation();
	}

	public override void _Process(double delta)
	{
		if (owner != null)
			UpdatePositionAndRotation();
	}
	protected void UpdatePositionAndRotation()
	{
		if (isPlayer){
			Rotation = p.getRotationVector().Angle() + Mathf.Pi / 2;
		} 
		else {
			Rotation = owner.Rotation;
		}
		float xOffset = 30 * Mathf.Cos(Rotation); // Adjust for fish length
		float yOffset = 18 * Mathf.Sin(Rotation); // Adjust for fish width
		Vector2 offset = new Vector2(xOffset, yOffset);
		Vector2 forwardDirection = new Vector2(0, -18).Rotated(Rotation); // 50 pixels forward
		GlobalPosition = owner.GlobalPosition + forwardDirection;
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
			bullet.hitsPlayers=!isPlayer;
			if(isPlayer){
				bullet.Modulate=new Color(0,255,0);
				bullet.Damage=10;
				Background.soundManager.playSound(Sound.PLAYER_SHOOT,0,1);
			}
			else{
				bullet.Modulate=new Color(255,0,0);
				bullet.Speed/=3;
				bullet.Damage=3;
				Background.soundManager.playSound(Sound.ENEMY_SHOOT,-25f,1);
			}
	   	 	bullet.GlobalPosition = GlobalPosition + direction * 30;
	   	 	bullet.SetDirection(direction);
			GetTree().CurrentScene.AddChild(bullet);
		}else{
			GD.Print("BulletScene is not assigned.");
		}

		GD.Print("Gun fired a bullet!");
	}
}
