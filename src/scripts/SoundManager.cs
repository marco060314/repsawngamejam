using Godot;
using System;

public partial class SoundManager : Node
{
	public void playSound(Sound sound,float volume,float pitch){
		AudioStreamPlayer p=new AudioStreamPlayer();
		p.Stream=SoundExtensions.getAudioStream(sound);
		p.PitchScale=pitch;
		p.VolumeDb=volume;
		AddChild(p);
		p.Play();
	}
}
