using Godot;
using System;

public class HealthUI : Control
{
   int hearts = 4;
   int max_hearts = 4;

    TextureRect heartUIEmpty = null;
    TextureRect heartUIFull = null;
    Node playerStats;

    public int set_hearts{
        get {return hearts;}
        set {
            hearts = Mathf.Clamp(value, 0, max_hearts);
            if(heartUIFull != null)
                heartUIFull.RectSize = new Vector2((hearts * 15) , 11);
        }
    }

    public int set_max_hearts{
        set {
            max_hearts = Math.Max(value,1);
            this.hearts = Math.Min(hearts, max_hearts);
            if(heartUIEmpty != null)
                heartUIEmpty.RectSize = new Vector2((max_hearts * 15), 11);
        }
        
    }

    public void call_set_hearts(int value){
        this.set_hearts = value;
    }

    public void call_set_max_hearts(int value){
        this.set_max_hearts = value;
    }
    
    public override void _Ready()
    {
        playerStats = GetNode<Node>("/root/PlayerStats");
        heartUIEmpty = GetNode<TextureRect>("HeartUIEmpty");
        heartUIFull = GetNode<TextureRect>("HeartUIFull");
        this.set_max_hearts = (int)playerStats.Get("max_health");
        this.set_hearts = (int)playerStats.Get("health");
        playerStats.Connect("health_changed", this, "call_set_hearts");
        playerStats.Connect("max_health_changed", this, "call_set_max_hearts");
    }
}
