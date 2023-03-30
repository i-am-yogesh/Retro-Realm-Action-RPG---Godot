using Godot;
using System;

public class IncreaseMaxHealth : Area2D
{
    [Signal]
    public delegate void IncreasePlayerMaxHealth();
    public void _on_IncreaseMaxHealth_body_entered(Node body){
        EmitSignal("IncreasePlayerMaxHealth");
        QueueFree();
    }

    public override void _Ready()
    {
        var player = GetTree().Root.FindNode("Player", true, false);
        GetNode<AnimationPlayer>("AnimationPlayer").Play("popout");
        Connect("IncreasePlayerMaxHealth", player, "_on_IncreaseMaxHealth_IncreasePlayerMaxHealth");
    }
}
