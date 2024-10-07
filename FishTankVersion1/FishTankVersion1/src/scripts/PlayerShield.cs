using Godot;
using System;

public partial class PlayerShield : BaseShield
{
	// Extra abilities for the player shield
	public override void ActivatePush()
	{
		base.ActivatePush(); // Calls the basic push functionality in BaseShield

		// Additional player-specific effects, like a visual flash or sound
		GD.Print("Player shield activated with a unique effect!");
	}
}
