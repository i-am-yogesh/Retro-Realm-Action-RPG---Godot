using Godot;
using System;

public class Credits : CanvasLayer
{

    public override void _Ready()
    {
        GetNode<Button>("BackButton").GrabFocus();
    }

    public void _on_BackButton_pressed(){
        GetTree().ChangeScene("res://UI/MainMenu.tscn");
    }


}
