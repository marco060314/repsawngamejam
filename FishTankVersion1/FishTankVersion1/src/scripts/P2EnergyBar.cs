using Godot;
using System;

public partial class P2EnergyBar : TextureProgressBar
{
	double energy=0;
	public double add(double delta){
		double old=energy;
		energy=Math.Min(100,energy+delta);
		return Math.Max(0,100-old-delta);
	}
}
