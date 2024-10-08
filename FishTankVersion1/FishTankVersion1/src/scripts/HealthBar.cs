using Godot;
using System;

public partial class P1HealthBar : TextureProgressBar
{
	double hp=100;
	public bool damage(double delta) {
		hp-=delta;
		hp=Math.Max(0,hp-delta);
		Value=hp;
		return hp==0;
	}
}
