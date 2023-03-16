using Godot;
using System;

public class SoftCollision : Area2D
{
    public bool is_colliding(){
        var areas = GetOverlappingAreas();
        return (areas.Count > 0);
    }

    public Vector2 get_push_vector(){
        var areas = GetOverlappingAreas();
        Vector2 push_vector = Vector2.Zero;

        if(is_colliding()){
            SoftCollision area = (SoftCollision)areas[0];
            push_vector = area.GlobalPosition.DirectionTo(GlobalPosition);
            push_vector = push_vector.Normalized();
        }

        return push_vector;
    }
}
