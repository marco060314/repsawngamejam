using Godot;
using System;

public partial class BackgroundMusic : AudioStreamPlayer
{
	
	public static readonly AudioStream backgroundSound=SoundExtensions.getAudioStream(Sound.BACKGROUND);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playMusic();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void playMusic(){
		if(Stream==backgroundSound){
			return;
		}
		Stream=backgroundSound;
		Play();
	}
}
