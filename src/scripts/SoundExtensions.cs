using Godot;

public class SoundExtensions{

    public static AudioStream getAudioStream(Sound s){
        string path="";
        if(s==Sound.ENEMY_DEATH){
            path="res://assets/audio/GrossSplash.wav";
        }
        else if(s==Sound.ENEMY_SHOOT){
            path="res://assets/audio/LaserShoot.wav";
        }
        else if(s==Sound.PLAYER_SHOOT){
            path="res://assets/audio/BubblePop.mp3";
        }
        else if(s==Sound.GAME_OVER){
            path="res://assets/audio/GameOver1.wav";
        }
        else if(s==Sound.GAME_START){
            path="res://assets/audio/GameStart.ogg";
        }
        else if(s==Sound.CHANGE_DIFFICULTY){
            path="res://assets/audio/RetroSound.wav";
        }
        else if(s==Sound.BACKGROUND){
            path="BACKGROND";
        }
        return GD.Load<AudioStream>(path);
    }
}