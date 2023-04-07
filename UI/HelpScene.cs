using Godot;
using System;

public class HelpScene : Control
{
    void _on_Button_pressed(){
        GetTree().ChangeScene("res://UI/MainMenu.tscn");
    }

    public override void _Ready()
    {
        GetNode<Button>("BackButton").GrabFocus();
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("toggle_fullscreen")){
            OS.WindowFullscreen = !OS.WindowFullscreen;
        }
    }
}
