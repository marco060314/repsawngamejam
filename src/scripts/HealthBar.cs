using Godot;
using System;

public partial class HealthBar : TextureProgressBar
{
	double hp=100;

	[Signal]
	public delegate void gameOverEventHandler();

	public bool damage(double delta) {
		hp-=delta;
		hp=Math.Max(0,hp-delta);
		Value=hp;
		return hp==0;
	}

	public void onPlayerDamaged(float damageDx){
		GD.Print("changing hp");
		if(damage(damageDx)) {
			EmitSignal(SignalName.gameOver);
			GD.Print("Game Over!");
		}
	}
}
