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

    public override void _Ready(){
        GetNode<AnimationPlayer>("AnimationPlayer").Play("popout");
        var player = GetTree().Root.FindNode("Player", true, false);
        Connect("IncreasePlayerHealth", player, "_on_IncreaseHealth_IncreasePlayerHealth");
    }
}
