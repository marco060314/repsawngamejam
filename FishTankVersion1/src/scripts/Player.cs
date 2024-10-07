using Godot;
using System;

public partial class Player : Entity {
	private const float TOLERANCE = 0.05f; // Controller deadzone

	[Export] public bool isPlayerOne;
	[Export] public bool isPlayerTwo;

	[Export] public PackedScene GunScene;    // Drag the PlayerGun.tscn here
	[Export] public PackedScene ShieldScene; // Drag the PlayerShield.tscn here

	private PlayerGun gun;
	private BaseShield shield;
	private Vector2 rotationVector;

	public Player() : base(300f, 15f, 23f, 100f) {
		// Initial setup of player properties
	}

	public override void _Ready() {
		if (isPlayerOne && GunScene != null) {
			gun = GunScene.Instantiate<PlayerGun>(); // Creates an instance of the gun
			AddChild(gun);                          // Adds it as a child of the player
			GD.Print("Player 1 has been assigned a gun");
		}
		else if (isPlayerTwo && ShieldScene != null) {
			shield = ShieldScene.Instantiate<BaseShield>(); // Creates an instance of the shield
			AddChild(shield);                                // Adds it as a child of the player
			shield.LockToOwner(this);                        // Locks shield to follow the player
			GD.Print("Player 2 has been assigned a shield");
		}
		
		GD.Print($"Player {Name} - isPlayerOne: {isPlayerOne}, isPlayerTwo: {isPlayerTwo}");
		GD.Print($"Gun: {gun}, Shield: {shield}");
		
		base._Ready();
	}

	public override void _PhysicsProcess(double delta) {
		if (isPlayerOne) {
			// Handle movement
			direction = Input.GetVector("LEFT", "RIGHT", "UP", "DOWN");

			// Rotate towards the mouse
			Vector2 mousePosition = GetGlobalMousePosition();
			rotationVector = (mousePosition - position).Normalized();
			Rotation = (mousePosition - GlobalPosition).Angle() + Mathf.Pi / 2;

			// Shoot if left mouse is pressed
			if (Input.IsActionJustPressed("MOUSELEFTCLICK") && gun != null) {
				gun.Shoot(rotationVector);
			}
		}
		else if (isPlayerTwo) {
			// Handle movement
			direction = Input.GetVector("P2_LEFT", "P2_RIGHT", "P2_UP", "P2_DOWN");

			// Use the right joystick for Player 2 rotation
			Vector2 rightStickDirection = new Vector2(
				Input.GetAxis("P2_LOOK_LEFT", "P2_LOOK_RIGHT"),
				Input.GetAxis("P2_LOOK_UP", "P2_LOOK_DOWN")
			);

			// If the right joystick is moved beyond the tolerance threshold
			if (rightStickDirection.Length() > TOLERANCE) {
				rightStickDirection = rightStickDirection.Normalized();
				Rotation = rightStickDirection.Angle() + Mathf.Pi / 2;
				rotationVector = rightStickDirection;
			}

			// Activate shield push if the right trigger is pressed
			if (Input.IsActionJustPressed("P2_RIGHT_TRIGGER") && shield != null) {
				shield.ActivatePush();
			}
		}

		base._PhysicsProcess(delta);
	}
	
	// Getter methods
	public Vector2 getPosition(){
		return position;
	}
	public Vector2 getVelocity(){
		return velocity;
	}
	public Vector2 getDirection(){
		return direction.Normalized();
	}
	public float getRotation(){
		return Rotation;
	}
	public Vector2 getRotationVector(){
		return rotationVector;
	}
}
