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
	[Export] private Bullet bullet;
	[Export] private Gun gun;

	private Vector2 rotationVector;

	public Player() : base(300f, 15f, 23f, 100f){
		// damage, attackSpeed, bulletSpeed, spread (in degrees)
		gun = new Gun();
		gun.setDamage(10);
		gun.setAttackSpeed(0.5f);
		gun.setBulletSpeed(500);
		gun.setSpread(0);
	}

	public override void _Ready(){
		// runs once after the node is added to the scene
		
		if (isPlayerOne){
			Position = new Vector2(480, 540);  // Player 1 starting position
		}
		else if (isPlayerTwo){
			Position = new Vector2(1440, 540);  // Player 2 starting position
		}

		base._Ready();
	}

	public override void _PhysicsProcess(double delta){
		if (isPlayerOne){
			// Handle movement
			direction = Input.GetVector("LEFT", "RIGHT", "UP", "DOWN");

			// Rotate towards the mouse
			Vector2 mousePosition = GetGlobalMousePosition();
			rotationVector = mousePosition - position;
			rotationVector = rotationVector.Normalized();
			Rotation = (mousePosition - GlobalPosition).Angle() + Mathf.Pi / 2;

		}
		else if (isPlayerTwo){
			// Handle movement
			direction = Input.GetVector("P2_LEFT", "P2_RIGHT", "P2_UP", "P2_DOWN");

			// Use the right joystick for Player 2 rotation
			Vector2 rightStickDirection = new Vector2(
					Input.GetAxis("P2_LOOK_LEFT", "P2_LOOK_RIGHT"),  // X-axis: left and right
					Input.GetAxis("P2_LOOK_UP", "P2_LOOK_DOWN")      // Y-axis: up and down
			);

			// If the right joystick is moved beyond the tolerance threshold
			if (rightStickDirection.Length() > TOLERANCE) {
				rightStickDirection = rightStickDirection.Normalized(); // Normalize direction
				Rotation = rightStickDirection.Angle() + Mathf.Pi / 2;
				rotationVector = rightStickDirection;  // Update rotation vector for shooting direction
			}

			// Example shooting for Player 2 (You can add the controller button to shoot here)
			// if (Input.IsActionJustPressed("P2_SHOOT")) {
			//     Shoot(rotationVector, gun.getBulletSpeed());
			// }
		}

		// Call base method to process movement and physics
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
