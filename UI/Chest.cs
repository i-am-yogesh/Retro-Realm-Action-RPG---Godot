using Godot;
using System;

public class Chest : Sprite
{
    bool playerInside = false;
    bool chestOpened = false;

    PackedScene dropItem;
    Tween tween;
    void _on_Area2D_body_entered(Node body){
        playerInside = true;
    }

    void _on_Area2D_body_exited(Node body){
        playerInside = false;
    }

    public override void _Ready()
    {
        dropItem = (PackedScene)ResourceLoader.Load("res://UI/IncreaseMaxHealth.tscn");
        tween = GetNode<Tween>("Tween");
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("interact") && (playerInside) && (!chestOpened)){
            chestOpened = true;
            GetNode<AnimationPlayer>("AnimationPlayer").Play("Open");
        }
    }

    public void dropObject(){
        var dropItemInstance = (Area2D)dropItem.Instance();
        GetParent().GetNode("Objects").AddChild(dropItemInstance);
        dropItemInstance.Position = Position;
    }
}
