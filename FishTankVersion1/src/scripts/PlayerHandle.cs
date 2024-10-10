using Godot;
using System;

public partial class PlayerHandle : Node2D
{
	[Export] public Player[] players;
	public static PlayerHandle p;
	static PlayerHandle(){
		p=new PlayerHandle();
	}
	public static Player[] getPlayers(){
		GD.Print(p.players);
		return p.players;
	}
}
