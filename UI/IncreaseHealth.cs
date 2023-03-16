using Godot;
using System;

public class IncreaseHealth : Area2D
{
    [Signal]
    public delegate void IncreasePlayerHealth();
    void _on_IncreaseHealth_body_entered(Node body){
        EmitSignal("IncreasePlayerHealth");
        QueueFree();
    }
}
