using Godot;
using System;

public class Chest : Sprite
{
    bool playerInside = false;
    bool chestOpened = false;

    PackedScene increaseHealth;
    PackedScene increaseMaxHealth;
    void _on_Area2D_body_entered(Node body){
        playerInside = true;
    }

    void _on_Area2D_body_exited(Node body){
        playerInside = false;
    }

    public override void _Ready()
    {
        increaseHealth = (PackedScene)ResourceLoader.Load("res://UI/IncreaseHealth.tscn");
        increaseMaxHealth = (PackedScene)ResourceLoader.Load("res://UI/IncreaseMaxHealth.tscn");
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("interact") && (playerInside) && (!chestOpened)){
            chestOpened = true;
            GetNode<AnimationPlayer>("AnimationPlayer").Play("Open");
        }
    }

    public void dropObject(){
        var increaseHealthInstance = (Area2D)increaseHealth.Instance();
        GetParent().GetNode("Objects").AddChild(increaseHealthInstance);

        var increaseMaxHealthInstance = (Area2D)increaseMaxHealth.Instance();
        GetParent().GetNode("Objects").AddChild(increaseMaxHealthInstance);

        Vector2 dropPosition = GlobalPosition;

        increaseHealthInstance.GlobalPosition = new Vector2(dropPosition.x+12 , dropPosition.y+10);
        increaseMaxHealthInstance.GlobalPosition = new Vector2(dropPosition.x-12 , dropPosition.y+10);
    }
}
