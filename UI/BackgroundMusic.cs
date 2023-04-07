using Godot;
using System;

public class BackgroundMusic : AudioStreamPlayer2D
{
    public static void pauseResumeAudio(){
        
    }

    public void changeStream(){
        if(StreamPaused == true)
            StreamPaused = false;
        else
            StreamPaused = true;
    }

    public override void _Ready()
    {
    }

}
