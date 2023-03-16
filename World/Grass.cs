using Godot;
using System;

public class Grass : Node2D
{
    /*
    * effectID --> 1 = Green Land(Default)
    * effectId --> 2 = Winter
    */
    void create_grass_effect(int effectID)
    {
        var GrassEffect = (PackedScene)ResourceLoader.Load("res://Effects/GrassEffect.tscn");
        var grassEffect = (AnimatedSprite)GrassEffect.Instance();
        if(effectID == 2) grassEffect.Animation = "Winter";
        var world = GetTree().CurrentScene;
        world.AddChild(grassEffect);
        grassEffect.GlobalPosition = GlobalPosition;
    }

    void _on_HurtBox_area_entered(Area2D area , int effectID)
    {
        create_grass_effect(effectID);
        QueueFree();
    }
}