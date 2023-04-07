using Godot;
using System;

public class SceneTransition : CanvasLayer
{
    public override void _Ready()
    {
        
    }

    void _on_AnimationPlayer_animation_finished(String animeName){
        QueueFree();
    }

}
