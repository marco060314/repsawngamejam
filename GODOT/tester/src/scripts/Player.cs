using Godot;
using System;

public partial class Player : Entity
{

    [Signal]
    public delegate void gameOverEventHandler();

	private PackedScene bulletScene;
	private double lastShot;
	private bool autofire;
	private readonly double delay=100d;
	
	public Player() : base(300f,15f,23f)
	{
		bulletScene = GD.Load<PackedScene>("res://src/scripts/Bullet.tscn");
		autofire=false;
	}

	public override void _PhysicsProcess(double delta){
		updateDirection(Input.GetVector("LEFT", "RIGHT", "UP", "DOWN"));
		base._PhysicsProcess(delta);
		if(autofire){
			shoot(Position,GetGlobalMousePosition());
		}
	}

	public void die() {
		GetTree().ChangeSceneToFile("res://src/background/gameover.tscn");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if(mouseEvent.ButtonIndex!=Godot.MouseButton.Left)
				return;
			if (mouseEvent.Pressed)
			{
				autofire=true;
				shoot(Position,mouseEvent.Position);
			}
			if(mouseEvent.IsReleased()&&mouseEvent.ButtonIndex==Godot.MouseButton.Left){
				GD.Print("mouse released");
				autofire=false;
			}
		}
	}

	private void shoot(Vector2 start,Vector2 destination){
		if(lastShot+delay>Time.GetTicksMsec()){
			return;
		}
		var bulletInstance =(Bullet)bulletScene.Instantiate();
		
		bulletInstance.Position=start;
		bulletInstance.setDirection(Position.DirectionTo(destination));
		bulletInstance.setSpeed(1500f);

		GetParent().AddChild(bulletInstance);
		lastShot=Time.GetTicksMsec();
	}

}
