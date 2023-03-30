using Godot;
using System;

public class Monster : KinematicBody2D
{
        [Export]
    int ACCELERATION = 300;
    [Export]
    int MAX_SPEED = 50;
    [Export]
    int FRICTION = 200;
    [Export]
    float WANDER_TARGET_RANGE = 4;

    Vector2 knockback = Vector2.Zero;
    Vector2 velocity = Vector2.Zero;
    Node statsNode = null;
    Stats stats;
    SwordHitBox swordHitBox;
    PlayerDetectionZone playerDetectionZone;
    HurtBox hurtBox = null;
    WanderController wanderController;
    SoftCollision softCollision;
    AnimationPlayer animationPlayer;
    enum Movement{
        IDLE,
        WANDER,
        CHASE
    }

    Movement state = Movement.IDLE;

    public override void _Ready()
    {
        statsNode = GetNode<Node>("Stats");
        stats = GetNode<Stats>("Stats");
        swordHitBox = new SwordHitBox();
        playerDetectionZone = GetNode<PlayerDetectionZone>("PlayerDetectionZone");
        hurtBox = GetNode<HurtBox>("HurtBox");
        softCollision = GetNode<SoftCollision>("SoftCollision");
        wanderController = GetNode<WanderController>("WanderController");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _Process(float delta)
    {
        knockback = knockback.MoveToward(Vector2.Zero, FRICTION * delta);
        knockback = MoveAndSlide(knockback);

        if(state == Movement.IDLE){
            animationPlayer.Play("Idle");
            velocity = -velocity.MoveToward(Vector2.Zero, FRICTION * delta);
            seek_player();
            if(wanderController.get_time_left() == 0)
                update_wander();
            
        }
        if(state == Movement.WANDER){ 
            animationPlayer.Play("Move");
            seek_player();
            if(wanderController.get_time_left() == 0)
                update_wander(); 
            
            accelarate_toward_point(wanderController.target_position,delta);

            if(GlobalPosition.DistanceTo(wanderController.target_position) <= WANDER_TARGET_RANGE)
                update_wander();
        }
        if(state == Movement.CHASE){
            // animationPlayer.Play("Move");
            var player = (KinematicBody2D)playerDetectionZone.Player;
            if(player != null)
                accelarate_toward_point(player.GlobalPosition, delta);
            else
                state = Movement.IDLE;
            
        }

        if(softCollision.is_colliding())
        velocity += softCollision.get_push_vector() * delta * 400;
        velocity = MoveAndSlide(velocity);
    }

    void accelarate_toward_point(Vector2 point, float delta){
        var direction = GlobalPosition.DirectionTo(point);
        velocity = velocity.MoveToward(direction * MAX_SPEED, ACCELERATION * delta);
        if(velocity.x < 0){
            Scale = new Vector2(1,-1);
            RotationDegrees = 180;
        }
        else{
            Scale = new Vector2(1,1);
            RotationDegrees = 0;
        }
    }

    void update_wander(){
        var state_list = new Godot.Collections.Array<Movement>{Movement.IDLE, Movement.WANDER};
        state = pick_random_state(state_list);
        wanderController.start_wander_timer(2);
    }

    void seek_player(){
        if(playerDetectionZone.can_see_player()) state = Movement.CHASE;
    }

    Movement pick_random_state(Godot.Collections.Array<Movement> state_list){
        state_list.Shuffle();
        Movement random_state = state_list[0];
        state_list.RemoveAt(0);
        return random_state;
    }

    void _on_HurtBox_area_entered(Area2D area)
    {
        knockback = SwordHitBox.knockback_vector * 75;
        animationPlayer.Play("Hurt");
        stats.set_health -= (int)area.Get("damage");
        hurtBox.create_hit_effect();
    }

    void _on_Death_animation_finished(){
      QueueFree();
    }

    void _on_Stats_NoHealth()
    {
        SetProcess(false);
        animationPlayer.Play("Death");
    }

    void _on_Hurt_animation_finished(){
        animationPlayer.Play("Attack");
    }

    void _on_HitboxPivot_body_exited(Node body){
        GD.Print("HELO2");
        animationPlayer.Play("Idle");
    }

    void _on_HitboxPivot_body_entered(Node body){
        GD.Print("HELO");
        animationPlayer.Play("Attack");
    }
}
