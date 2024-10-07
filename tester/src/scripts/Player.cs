using Godot;
using System;

/*
 * List of accessible variables from Entity:
 * - Vector2 direction
 * - Vector2 position
 * - Vector2 velocity
 * - float rotation
 * - float speed
 * - float friction
 */

public partial class Player : Entity {
	private const float TOLERANCE = 0.05f; // Controller deadzone

	[Export] public bool isPlayerOne;
	[Export] public bool isPlayerTwo;
	//[Export] private PackedScene WeaponScene;
	[Export] private Weapon weapon;

	private Vector2 rotationVector;

	public Player() : base(300f, 15f, 23f, 100f) {
		// damage, attackSpeed, bulletSpeed, spread (in degrees)
	}

	public override void _Ready() {
		//if (WeaponScene != null) {
			// weapon = WeaponScene.Instantiate<Weapon>();
			// AddChild(weapon);
			
		if (isPlayerOne && !isPlayerTwo) {
			weapon.EquipGun();
			GD.Print("Player 1 has been assigned a gun");
		} else if (isPlayerTwo && !isPlayerOne) {
			weapon.EquipShield();
			GD.Print("Player 2 has been assigned a shield");
		}
		//}

		base._Ready();
	}

	public override void _PhysicsProcess(double delta) {
		if (isPlayerOne) {
			// Handle movement
			direction = Input.GetVector("LEFT", "RIGHT", "UP", "DOWN");
			weapon.EquipGun();
			// Rotate towards the mouse
			Vector2 mousePosition = GetGlobalMousePosition();
			rotationVector = (mousePosition - position).Normalized();
			Rotation = (mousePosition - GlobalPosition).Angle() + Mathf.Pi / 2;
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

			if (Input.IsActionJustPressed("P2_RIGHT_TRIGGER") && weapon != null) {
				weapon.ActivateWeapon(rotationVector);
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
