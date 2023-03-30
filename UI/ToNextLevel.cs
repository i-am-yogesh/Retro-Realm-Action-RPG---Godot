using Godot;
using System;

public class ToNextLevel : Area2D
{
    [Signal]
    public delegate void SetPalyerProcessInput();

    [Export(PropertyHint.File)]
    public string NEXT_LEVEL = "";

    public override void _Ready()
    {
         var player = GetTree().Root.FindNode("Player", true, false);
        Connect("SetPalyerProcessInput", player,"_on_ToNextLevel_SetPalyerProcessInput");
    }

    void _on_ToNextLevel_body_entered(Node body){
        if(NEXT_LEVEL != ""){
            EmitSignal("SetPalyerProcessInput");
        }
    }

    public void _on_Player_TeleportToNextLevel(){
        if(NEXT_LEVEL != "")
            GetTree().ChangeScene(NEXT_LEVEL);
    }
}
