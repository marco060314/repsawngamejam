using Godot;
using System;

public partial class Player : Entity {
	private const float TOLERANCE = 0.05f; // Controller deadzone

	[Export] public bool isPlayerOne;
	[Export] public bool isPlayerTwo;

	[Export] public PackedScene GunScene;    // Drag the PlayerGun.tscn here
	[Export] public PackedScene ShieldScene; // Drag the PlayerShield.tscn here

	[Export] public Texture2D player1Texture;
	[Export] public Texture2D player2Texture;
	
	private Sprite2D playerSprite;

	private PlayerGun gun;
	private BaseShield shield;
	private Vector2 rotationVector;

	private float diff = 0.0f;
	private float rightStickAngle = 0.0f;

	public Player() : base(300f, 15f, 23f, 100f) {
		// Initial setup of player properties
	}

	public override void _Ready() {
		playerSprite = GetNode<Sprite2D>("Sprite");
		float desiredWidth = 0;
		float desiredHeight = 0;
		if (isPlayerOne && GunScene != null) {
			playerSprite.Texture = player1Texture;
			Position = new Vector2(200, 200);       // Set the player's initial position
			gun = GunScene.Instantiate<PlayerGun>(); // Creates an instance of the gun
			AddChild(gun);                          // Adds it as a child of the player
			gun.LockToOwner(this);                  // Locks gun to follow the player
			GD.Print("Player 1 has been assigned a gun");
			desiredWidth = 200;  // Example desired width
			desiredHeight = 200; // Example desired height
		}
		else if (isPlayerTwo && ShieldScene != null) {
			playerSprite.Texture = player2Texture;
			Position = new Vector2(1400, 400);  
			shield = ShieldScene.Instantiate<BaseShield>(); // Creates an instance of the shield
			AddChild(shield);                                // Adds it as a child of the player
			shield.LockToOwner(this);                        // Locks shield to follow the player
			GD.Print("Player 2 has been assigned a shield");
			desiredWidth = 250;  // Example desired width
			desiredHeight = 250; // Example desired height
		}
		
		GD.Print($"Player {Name} - isPlayerOne: {isPlayerOne}, isPlayerTwo: {isPlayerTwo}");
		GD.Print($"Gun: {gun}, Shield: {shield}");
		
		
		Vector2 scale = new Vector2(desiredWidth / playerSprite.Texture.GetWidth(), 
								desiredHeight / playerSprite.Texture.GetHeight());
		playerSprite.Scale = scale;
		
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
			if (Input.IsActionPressed("MOUSELEFTCLICK") && gun != null) {
				gun.Shoot(rotationVector);
			}

			Rotation = rotation;
		}
		else if (isPlayerTwo) {
			float rotationSpeed = 10.0f;
			
			direction = Input.GetVector("P2_LEFT", "P2_RIGHT", "P2_UP", "P2_DOWN");

			if (direction.Length() > TOLERANCE)
			{
				float targetRotation = direction.Angle() + Mathf.Pi / 2;
				Rotation = Mathf.LerpAngle(Rotation, targetRotation, rotationSpeed * (float)delta);
			}

			Vector2 rightStickDirection = new Vector2(
				Input.GetActionRawStrength("P2_LOOK_LEFT", true) - Input.GetActionRawStrength("P2_LOOK_RIGHT", true),
				Input.GetActionRawStrength("P2_LOOK_UP", true) - Input.GetActionRawStrength("P2_LOOK_DOWN", true)
			);

			if (rightStickDirection.Length() > TOLERANCE)
			{
				float shieldRotation = rightStickDirection.Angle();
				shield.SetShieldRotation(shieldRotation); // Rotate shield separately
			}
			

			if (Input.IsActionJustPressed("P2_RIGHT_TRIGGER") && shield != null)
			{
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
