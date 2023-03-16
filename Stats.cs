using Godot;
using System;

public class Stats : Node
{
    [Export]
    public int max_health = 1;
    public int health = 0;
	
	[Signal]
 public delegate void NoHealth();

 [Signal]
 public delegate void health_changed(int value);
 [Signal]
 public delegate void max_health_changed(int value);


   public int set_health{
		get{
			return this.health;
		}
		set{
			health = value;
			EmitSignal("health_changed", health);
			if (health <= 0){
				EmitSignal("NoHealth");
			}
		}
	}

	public int set_max_health{
		get {return max_health;}
		set {
			max_health = value;
			this.set_health = Math.Min(health,max_health);
			EmitSignal("max_health_changed", max_health);
		}
	}

	public override void _Ready(){
		health = max_health;
	}
}
