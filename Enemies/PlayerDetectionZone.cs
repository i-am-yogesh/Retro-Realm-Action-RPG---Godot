using Godot;
using System;

public class PlayerDetectionZone : Area2D
{
    public KinematicBody2D Player = null;

    public bool can_see_player(){
        return (Player != null);
    }

    void _on_PlayerDetectionZone_body_entered(Node body){
        Player = (KinematicBody2D)body;
    }

    void _on_PlayerDetectionZone_body_exited(Node body){
        Player = null;
    }
}
