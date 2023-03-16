using Godot;
using System;

public class Camera2D : Godot.Camera2D
{
    
    public override void _Ready()
    {
     var topLeft = GetNode<Position2D>("Limits/TopLeft");
     var bottomRight = GetNode<Position2D>("Limits/BottomRight");

     LimitTop = (int)topLeft.Position.y;
     LimitLeft = (int)topLeft.Position.x;   
     LimitBottom = (int)bottomRight.Position.y;
     LimitRight = (int)bottomRight.Position.x;
    }
}
