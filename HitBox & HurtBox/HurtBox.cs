using Godot;
using System;

public class HurtBox : Area2D
{
    [Signal]
    public delegate void invinciblilityStarted();
    [Signal]
    public delegate void invincibleEnded();
    PackedScene hitEffect;
    CollisionShape2D collisionShape2D;
    Timer timer;

    public bool invincible = false;

    public bool set_invincible{
        get { return invincible;}
        set{
            invincible = value;
            if(invincible == true){
                EmitSignal("invinciblilityStarted");
            } 
            else{
                EmitSignal("invincibleEnded");
            } 
        }
    }

    public override void _Ready()
    {
        hitEffect = (PackedScene)ResourceLoader.Load("res://Effects/HitEffect.tscn");
        timer = GetNode<Timer>("Timer");
        collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public void start_invincibility(float duration){
        set_invincible = true;
        timer.Start(duration);
    }

    public void create_hit_effect(){
        var effect = (AnimatedSprite)hitEffect.Instance();
        var main = GetTree().CurrentScene;
        main.AddChild(effect);
        effect.GlobalPosition = GlobalPosition;
    }

    void _on_Timer_timeout(){
        set_invincible = false;
    }

    void _on_HurtBox_invinciblilityStarted(){
        SetDeferred ("monitoring", false);
    }

    void _on_HurtBox_invincibleEnded(){
        Monitoring = true;
    }
}
