using Godot;
using System;
using System.Threading.Tasks;

public class Trap : Sprite
{
    public async void _on_HitBox_area_entered(Area2D body){
        GD.Print("conn");
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Triggered");
        
        await Task.Delay(200);
        QueueFree();
    }

    public override void _Ready()
    {
        var hitbox = GetNode<HitBox>("HitBox");
        hitbox.Connect("area_entered", this, "_on_HitBox_area_entered");
    }

}
