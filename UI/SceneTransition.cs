using Godot;
using System;

public class SceneTransition : CanvasLayer
{
    void _on_AnimationPlayer_animation_finished(String animeName){
        QueueFree();
    }

}
