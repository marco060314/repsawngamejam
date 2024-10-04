using Godot;
using System;

public partial class Bullet : Area2D
{
	private Vector2 destination;
	private float speed;
	private Vector2 velocity;
	private readonly int OUT_OF_BOUNDS=5000;
	public Bullet(){
		destination=new Vector2(0,0);
		speed=10;
	}

	public void setDestination(Vector2 position){
		this.destination=position;
	}
	public void setSpeed(float speed){
		this.speed=speed;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		velocity = Position.DirectionTo(destination) * speed;
		GD.Print(velocity);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Translate(velocity *(float)delta);
		if(Position.X>OUT_OF_BOUNDS||Position.Y>OUT_OF_BOUNDS||Position.X<-OUT_OF_BOUNDS||Position.Y<-OUT_OF_BOUNDS){
			QueueFree();
		}
	}

	public void onBodyEntered(Node2D other){
		if(other.GetType()==typeof(Enemy)){
			QueueFree();
		}
    	GD.Print("entered "+other.Name);
	}
	public void onBodyExited(Node2D other){
    	GD.Print("exited "+other.Name);
	}
}
