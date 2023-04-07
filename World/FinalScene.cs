using Godot;
using System;

public class FinalScene : Control
{
    void _on_Timer_timeout(){
        GetTree().ChangeScene("res://UI/MainMenu.tscn");
    }
}
