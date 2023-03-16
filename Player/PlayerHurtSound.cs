using Godot;
using System;

public class PlayerHurtSound : AudioStreamPlayer2D
{
    public void free_this(){
        QueueFree();
    }

    public override void _Ready()
    {
        Connect("finished", this, "free_this"); 
    }

}
