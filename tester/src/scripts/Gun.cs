using Godot;
using System;

public partial class Gun : Node2D{

	[Export] private Player player;
	[Export] private Bullete bullet;
	[Export] private float damage;
	[Export] private float attackSpeed;
	[Export] private float bulletSpeed;
	[Export] private float spread; // in degrees

	public Gun(){
		damage = 1;
		attackSpeed = 0;
		bulletSpeed = 500;
		spread = 0;
	}

	public override void _Process(double delta){
		// Shoot when left mouse button is pressed
		if (Input.IsActionJustPressed("MOUSELEFTCLICK")){
			Shoot(rotationVector, gun.getBulletSpeed());
		}
	}
	// TODO: implement attackSpeed and spread
	
	public void Shoot(Vector2 dir, float speed) {
		Bullet b = (Bullet) bullet.Duplicate();
		b.Position = position + rotationVector * 50;  // Offset bullet from player position
		b.setSpeed(speed);
		b.setDirection(dir);
		GetParent().AddChild(b);
	}

	// Getter methods
	public float getDamage(){
		return damage;
	}
	public float getAttackSpeed(){
		return attackSpeed;
	}
	public float getBulletSpeed(){
		return bulletSpeed;
	}
	public float getSpread(){
		return spread;
	}

	// Setter methods
	public void setDamage(float damage){
		this.damage = damage;
	}
	public void setAttackSpeed(float attackSpeed){
		this.attackSpeed = attackSpeed;
	}
	public void setBulletSpeed(float bulletSpeed){
		this.bulletSpeed = bulletSpeed;
	}
	public void setSpread(float spread){
		this.spread = spread;
	}


}
