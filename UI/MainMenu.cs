using Godot;
using System;

public class MainMenu : Control
{
    [Export(PropertyHint.File)]
    public string FIRST_LEVEL = "";

    Button startButton;
    Button optionsButton;
    Button helpButton;
    Button quitButton;
    
    public override void _Ready()
    {
        startButton = GetNode<Button>("StartButton");
        optionsButton = GetNode<Button>("OptionsButton");
        helpButton = GetNode<Button>("HelpButton");
        quitButton = GetNode<Button>("QuitButton");

        startButton.GrabFocus();
    }

    public void _on_StartButton_pressed(){
        if(FIRST_LEVEL != "")
            GetTree().ChangeScene(FIRST_LEVEL);
    }

    public void _on_QuitButton_pressed(){
        GetTree().Quit();
    }
}
