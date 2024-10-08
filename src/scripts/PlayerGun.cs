using Godot;
using System;

public partial class PlayerGun : Gun
{
	public void ShootPlayerSpecific(Vector2 direction) // Temporarily renamed
	{
		base.Shoot(direction); // Calls the base Shoot method
		GD.Print("Player gun fired with additional effects!");
		// Additional effects specific to PlayerGun
	}
}
