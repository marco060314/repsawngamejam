using Godot;
using System;

public partial class Round : Counter
{
	[Signal] public delegate void newRoundEventHandler(int round);
	private Timer roundTimer;
	public Round(): base("Round:"){}

	public override void _Ready()
	{
		base._Ready();
		roundTimer=GetNode<Timer>("RoundTimer");
	}

	public void onRoundTimerTimeout(){
		EmitSignal(SignalName.newRound,counter);
		roundTimer.Stop();
	}

	public void signalNewRound(){
		add(1);
		roundTimer.Start();
	}
}
