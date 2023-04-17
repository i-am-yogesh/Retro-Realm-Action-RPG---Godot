using Godot;
using System;

public class MainMenu : Control
{
    [Export(PropertyHint.File)]
    public string FIRST_LEVEL = "";

    Button startButton;
    Button creditsButton;
    Button helpButton;
    Button quitButton;
    Button musicButton;
    AudioStreamPlayer2D menuSelect;
    PackedScene sceneTransition;

    public override void _Ready()
    {
        startButton = GetNode<Button>("StartButton");
        creditsButton = GetNode<Button>("CreditsButton");
        helpButton = GetNode<Button>("HelpButton");
        quitButton = GetNode<Button>("QuitButton");
        musicButton = GetNode<Button>("MusicButton");

        startButton.GrabFocus();
        menuSelect = GetNode<AudioStreamPlayer2D>("MenuSelect");

        sceneTransition = (PackedScene)ResourceLoader.Load("res://UI/SceneTransition.tscn");

        var stats = GetNode<Node>("/root/PlayerStats");
        stats.Set("set_health", 4);
        stats.Set("set_max_health", 4);
    }

    public void _on_StartButton_pressed(){
        menuSelect.Play();
        if(FIRST_LEVEL != ""){
            var sceneTransitionInstance = sceneTransition.Instance();
            GetTree().Root.AddChild(sceneTransitionInstance);
            
            GetTree().ChangeScene(FIRST_LEVEL);
        }
    }

    public void _on_QuitButton_pressed(){
        menuSelect.Play();
        GetTree().Quit();
    }

    public void _on_HelpButton_pressed(){
        menuSelect.Play();
        GetTree().ChangeScene("res://UI/HelpScene.tscn");
    }

    public void _on_CreditsButton_pressed(){
        menuSelect.Play();
        GetTree().ChangeScene("res://UI/Credits.tscn");
    }

    void _on_MusicButton_toggled(bool active){
        if(active){
            AudioServer.SetBusMute(AudioServer.GetBusIndex("ToggleBGM"), true);
            musicButton.Text = "MUSIC OFF";
        }
        else{
            AudioServer.SetBusMute(AudioServer.GetBusIndex("ToggleBGM"), false);
            musicButton.Text = "MUSIC ON";
        }
    }

    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_right") || Input.IsActionJustPressed("ui_left") || Input.IsActionJustPressed("ui_down") || Input.IsActionJustPressed("ui_up")){
            GetNode<AudioStreamPlayer2D>("MenuMove").Play();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("toggle_fullscreen")){
            OS.WindowFullscreen = !OS.WindowFullscreen;
        }
    }
}
