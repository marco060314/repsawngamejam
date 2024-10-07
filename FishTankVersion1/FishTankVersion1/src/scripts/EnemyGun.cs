using Godot;
using System;

public partial class PlayerGun : Gun
{
	public override void Shoot(Vector2 direction)
	{
		base.Shoot(direction);
		GD.Print("Player gun fired with additional effects!");
	}
}
