using Godot;
using System;

public class CutScenesOne : CanvasLayer
{
    [Export(PropertyHint.File)]
    public string NEXT_SCENE = "";

    void ChangeScene()
    {
        if (NEXT_SCENE != "")
            GetTree().ChangeScene(NEXT_SCENE);
    }

    void _on_SkipButton_pressed()
    {
        ChangeScene();
    }
}
