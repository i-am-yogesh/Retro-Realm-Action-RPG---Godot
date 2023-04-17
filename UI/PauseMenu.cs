using Godot;
using System;

public class PauseMenu : CanvasLayer
{
    Button resumeButton;
    Button soundButton;
    Button mainMenuButton;
    Button quitButton;
    
    public override void _Ready()
    {
        resumeButton = GetNode<Button>("ResumeButton");
        resumeButton.GrabFocus();

        soundButton = GetNode<Button>("SoundButton");
        mainMenuButton = GetNode<Button>("MainMenuButton");
        quitButton = GetNode<Button>("QuitButton");
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("pause")){
            if(GetTree().Paused == false){
                GetTree().Paused = true;
                Show();
                resumeButton.GrabFocus();
            }
            else{
                GetTree().Paused = false;
                Hide();
            }
        }

        if(@event.IsActionPressed("toggle_fullscreen")){
            OS.WindowFullscreen = !OS.WindowFullscreen;
        }
    }

    public void _on_ResumeButton_pressed(){
        GetTree().Paused = false;
        Hide();
    }

    public void _on_MainMenuButton_pressed(){
        GetTree().Paused = false;
        GetTree().ChangeScene("res://UI/MainMenu.tscn");
    }

    public void _on_QuitButton_pressed(){
        GetTree().Quit();
    }

    void _on_SoundButton_toggled(bool active){
        if(active){
            AudioServer.SetBusMute(AudioServer.GetBusIndex("ToggleBGM"), true);
            soundButton.Text = "MUSIC OFF";
        }
        else{
            AudioServer.SetBusMute(AudioServer.GetBusIndex("ToggleBGM"), false);
            soundButton.Text = "MUSIC ON";
        }
    }
}
