using Godot;
using System;

public class ToNextLevel : Area2D
{
    [Signal]
    public delegate void SetPalyerProcessInput();

    [Export(PropertyHint.File)]
    public string NEXT_LEVEL = "";

    Viewport root;
    Node currentScene;
    PackedScene sceneTransition;

    public override void _Ready()
    {
        root = GetTree().Root;
        currentScene = root.GetChild(GetChildCount() - 1);

        var player = GetTree().Root.FindNode("Player", true, false);
        Connect("SetPalyerProcessInput", player,"_on_ToNextLevel_SetPalyerProcessInput");

        sceneTransition = (PackedScene)ResourceLoader.Load("res://UI/SceneTransition.tscn");
    }

    void _on_ToNextLevel_body_entered(Node body){
        if(NEXT_LEVEL != ""){
            EmitSignal("SetPalyerProcessInput");
        }
    }


    public void _on_Player_TeleportToNextLevel(){
        if(NEXT_LEVEL != ""){
            var sceneTransitionInstance = sceneTransition.Instance();
            GetTree().Root.AddChild(sceneTransitionInstance);

            GetTree().ChangeScene(NEXT_LEVEL);
        }
    }
}
