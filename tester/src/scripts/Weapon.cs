using Godot;
using System;

public partial class Weapon : Node2D
{
	//[Export] public PackedScene GunScene;
	//[Export] public PackedScene ShieldScene;
	[Export] public Shield shield;

	private Node2D currentWeapon;

	public Weapon(){
	}

	public void EquipGun()
	{
		shield.destroyShield();
		// if (GunScene != null)
		// {
			
		// 	currentWeapon = GunScene.Instantiate<Node2D>();
		// 	AddChild(currentWeapon);
		// 	GD.Print("Equipped Gun");
		// }
	}

	public void EquipShield()
	{
		// if (ShieldScene != null)
		// {
		// 	currentWeapon = ShieldScene.Instantiate<Node2D>();

		// 	AddChild(currentWeapon);
		// 	GD.Print("Equipped Shield");
		// }
	}

	public void ActivateWeapon(Vector2 direction)
	{
		if (currentWeapon is Gun gun)
		{
			float speed = 500f; // Set an appropriate speed value
			gun.Shoot(direction, speed); // Pass both direction and speed
		}
		else if (currentWeapon is Shield shield)
		{
			shield.ActivatePush();
		}
	}


}
