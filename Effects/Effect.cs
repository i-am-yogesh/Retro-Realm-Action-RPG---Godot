using Godot;
using System;

public class Effect : AnimatedSprite
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("animation_finished", this , "on_animation_finished");
        Play();
    }

    public void on_animation_finished(){
        QueueFree();
    }
}
