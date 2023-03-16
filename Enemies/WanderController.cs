using Godot;
using System;

public class WanderController : Node2D
{
    [Export]
    int wander_range = 32;
    Vector2 start_position;
    public Vector2 target_position;
    Timer timer;

    public void update_targer_position(){
        Random num = new Random();
        target_position = new Vector2(num.Next(-wander_range,wander_range), num.Next(-wander_range,wander_range));
        target_position = start_position + target_position;
    }
    public override void _Ready()
    {
        start_position = GlobalPosition;
        target_position = GlobalPosition;
        timer = GetNode<Timer>("Timer");
    }

    public float get_time_left(){
        return timer.TimeLeft;
    }

    public void start_wander_timer(float duration){
        timer.Start(duration);
    }

    public void _on_Timer_timeout(){
        update_targer_position();
    }

}
