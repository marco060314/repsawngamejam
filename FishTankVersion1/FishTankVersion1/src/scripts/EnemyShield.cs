using Godot;
using System;

public partial class EnemyShield : BaseShield
{
	// Additional behaviors for the enemy shield
	public void ActivateAttack()
	{
		GD.Print("Enemy shield performing an attack move!");
		// Additional logic for attack if needed
	}
}
