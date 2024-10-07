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

	private float diff = 0.0f;
	private float rightStickAngle = 0.0f;

	public Player() : base(300f, 15f, 23f, 100f) {
		// Initial setup of player properties
	}

	public override void _Ready() {
		if (isPlayerOne && GunScene != null) {
			Position = new Vector2(400, 400);       // Set the player's initial position
			gun = GunScene.Instantiate<PlayerGun>(); // Creates an instance of the gun
			AddChild(gun);                          // Adds it as a child of the player
			gun.LockToOwner(this);                  // Locks gun to follow the player
			GD.Print("Player 1 has been assigned a gun");
		}
		else if (isPlayerTwo && ShieldScene != null) {
			Position = new Vector2(1400, 400);  
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
			rotation = Rotation;

			// Rotate towards the mouse
			Vector2 mousePosition = GetGlobalMousePosition();
			rotationVector = (mousePosition - position).Normalized(); // which way player is facing for shooting
			
			// rotation for the sprite
			if (Velocity != Vector2.Zero) {
				rotation = Mathf.LerpAngle(Rotation, Velocity.Angle() + Mathf.Pi / 2, 0.4f);
			}

			// Shoot if left mouse is pressed
			if (Input.IsActionJustPressed("MOUSELEFTCLICK") && gun != null) {
				gun.Shoot(rotationVector);
			}

			Rotation = rotation;
		}
		else if (isPlayerTwo) {
			// Handle movement
			direction = Input.GetVector("P2_LEFT", "P2_RIGHT", "P2_UP", "P2_DOWN");

			// Raw input from the right joystick
			Vector2 rightStickDirection = new Vector2(
				Input.GetActionRawStrength("P2_LOOK_LEFT", true) - Input.GetActionRawStrength("P2_LOOK_RIGHT", true),
				Input.GetActionRawStrength("P2_LOOK_UP", true) - Input.GetActionRawStrength("P2_LOOK_DOWN", true)
			);

			// Calculate the angle of the right joystick
			rightStickAngle = rightStickDirection.Angle() + Mathf.Pi / 2;
			diff = 0;

			// If the right joystick is moved beyond the tolerance threshold
			if (rightStickDirection.Length() > TOLERANCE) {
				rightStickDirection = rightStickDirection.Normalized(); // Normalize the direction

				diff = (float) delta * Mathf.Abs(Rotation - rightStickAngle) * 3;
				Rotation = Mathf.LerpAngle(Rotation, rightStickAngle, Mathf.Max(diff, 0.4f)); // Smoothly rotate the player
				rotationVector.X = Mathf.Cos(Rotation - Mathf.Pi / 2);
				rotationVector.Y = Mathf.Sin(Rotation - Mathf.Pi / 2);
				rotationVector = rotationVector.Normalized();
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
