using Godot;
using System;

public class Ocunid : KinematicBody2D
{
    [Export]
    int ACCELERATION = 300;
    [Export]
    int MAX_SPEED = 50;
    [Export]
    int FRICTION = 200;
    [Export]
    float WANDER_TARGET_RANGE = 4;
    bool MOVEMENT_PAUSED = false;

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
        CHASE,
        ATTACK
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
        knockback = knockback.MoveToward(Vector2.Zero, (FRICTION * 2) * delta);
        knockback = MoveAndSlide(knockback);

        if(state == Movement.IDLE){
            velocity = -velocity.MoveToward(Vector2.Zero, FRICTION * delta);
            seek_player();
            if(wanderController.get_time_left() == 0)
                update_wander();
            
        }
        if(state == Movement.WANDER){ 
            seek_player();
            if(wanderController.get_time_left() == 0)
                update_wander(); 
            
            accelarate_toward_point(wanderController.target_position,delta);

            if(GlobalPosition.DistanceTo(wanderController.target_position) <= WANDER_TARGET_RANGE)
                update_wander();
        }
        if(state == Movement.CHASE){
            var player = (KinematicBody2D)playerDetectionZone.Player;
            if(player != null)
               accelarate_toward_point(player.GlobalPosition, delta);
            else
                state = Movement.IDLE;
            
        }
        if(state == Movement.ATTACK){
            animationPlayer.Play("Attack");
            if(MOVEMENT_PAUSED == false){
                velocity = Vector2.Zero;
                MOVEMENT_PAUSED = true;
            }
        }

        if(softCollision.is_colliding())
        velocity += softCollision.get_push_vector() * delta * 400;
        velocity = MoveAndSlide(velocity);
    }

    void from_attack_to_idle(){
        animationPlayer.Play("Idle");
        velocity = Vector2.Zero;
        MOVEMENT_PAUSED = false;
        state = Movement.CHASE;
    }

    void move_towards_player_with_attack(){
        var player = (KinematicBody2D)playerDetectionZone.Player;
        if(player != null){
            var direction = GlobalPosition.DirectionTo(player.GlobalPosition);
           velocity = velocity.MoveToward(direction * MAX_SPEED * 6, ACCELERATION * 10);
        }
    }

    void accelarate_toward_point(Vector2 point, float delta){
        var direction = GlobalPosition.DirectionTo(point);
        velocity = velocity.MoveToward(direction * MAX_SPEED, ACCELERATION * delta);
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
        knockback = SwordHitBox.knockback_vector * 70;
        stats.set_health -= (int)area.Get("damage");
        hurtBox.create_hit_effect();
        state = Movement.ATTACK;
    }

    void _on_Stats_NoHealth()
    {
      QueueFree();
      var enemyDeathEffect = (PackedScene)ResourceLoader.Load("res://Effects/EnemyDeathEffect.tscn");
      var enemyEffect = (AnimatedSprite)enemyDeathEffect.Instance();
      GetParent().AddChild(enemyEffect);
      enemyEffect.GlobalPosition = GlobalPosition;
    }
}