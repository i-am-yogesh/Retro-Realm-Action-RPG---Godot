using Godot;
using System;

public class Player : KinematicBody2D
{
    [Signal]
    public delegate void TeleportToNextLevel();
    PackedScene playerHurtSound;

    const int ACCELERATION = 500;
    const int MAX_SPEED = 80;
    const int FRICTION = 500;
    const int ROLL_SPEED = 125;
    bool levelCompeleted = false;
    bool allowUserControl = true;
    Vector2 velocity = Vector2.Zero;
    Vector2 roll_vector = Vector2.Down;
    Vector2 respawnPosition;

    enum Movement
    {
        MOVE,
        ROLL,
        ATTACK
    }

    Movement state = Movement.MOVE;
    AnimationPlayer animationPlayer = null;
    AnimationTree animationTree = null;
    AnimationNodeStateMachinePlayback animationState = null;
    AnimationPlayer blinkAnimationPlayer;
    Node stats = null;
    HurtBox hurtBox = null;

    public void remove()
    {
        Hide();
        var basicTransitionNode = GetTree().Root.GetNode<CanvasLayer>("world/BasicTransition");
        if(basicTransitionNode != null){
            basicTransitionNode.GetNode<AnimationPlayer>("AnimationPlayer").Play("Transition");
        }
        Position = respawnPosition;
        int health = (int)stats.Get("health");
        stats.Set("set_health", (health + 2));
        Show();
    }

    public override void _Ready()
    {

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationTree = GetNode<AnimationTree>("AnimationTree");
        animationState = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
        animationTree.Active = true;
        SwordHitBox.knockback_vector = roll_vector;

        stats = GetNode<Node>("/root/PlayerStats");
        stats.Connect("NoHealth", this, "remove");

        hurtBox = GetNode<HurtBox>("HurtBox");
        playerHurtSound = (PackedScene)ResourceLoader.Load("res://Player/PlayerHurtSound.tscn");
        blinkAnimationPlayer = GetNode<AnimationPlayer>("BlinkAnimationPlayer");
        respawnPosition = Position;
    
        var toNextLevel = GetTree().Root.FindNode("ToNextLevel", true, false);
        if(toNextLevel != null)
            Connect("TeleportToNextLevel", toNextLevel, "_on_Player_TeleportToNextLevel");

        
    }

    public override void _PhysicsProcess(float delta)
    {
        if (allowUserControl)
        {
            if (state == Movement.MOVE) move_state(delta);
            else if (state == Movement.ROLL) roll_state(delta);
            else if (state == Movement.ATTACK) attack_state(delta);
        }
        else{
            velocity = new Vector2(MAX_SPEED, 0);
            animationTree.Set("parameters/Run/blend_position", Vector2.Right);
            animationState.Travel("Run");
            move();
        }
    }

    public void move_state(float delta)
    {
        var input_vector = Vector2.Zero;
        input_vector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        input_vector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        input_vector = input_vector.Normalized();

        if (input_vector != Vector2.Zero)
        {
            roll_vector = input_vector;
            SwordHitBox.knockback_vector = input_vector;
            animationTree.Set("parameters/Idle/blend_position", input_vector);
            animationTree.Set("parameters/Run/blend_position", input_vector);
            animationTree.Set("parameters/Attack/blend_position", input_vector);
            animationTree.Set("parameters/Roll/blend_position", input_vector);
            animationState.Travel("Run");
            velocity = velocity.MoveToward(input_vector * MAX_SPEED, ACCELERATION * delta);
        }
        else
        {
            animationState.Travel("Idle");
            velocity = velocity.MoveToward(Vector2.Zero, FRICTION * delta);
        }

        move();

        if (Input.IsActionJustPressed("roll")) state = Movement.ROLL;
        if (Input.IsActionJustPressed("ui_select")) state = Movement.ATTACK;
    }

    public void roll_state(float delta)
    {
        velocity = roll_vector * ROLL_SPEED;
        animationState.Travel("Roll");
        move();
    }

    public void attack_state(float delta)
    {
        velocity = Vector2.Zero;
        animationState.Travel("Attack");
    }

    public void move()
    {
        velocity = MoveAndSlide(velocity);
    }

    public void roll_animation_finished()
    {
        velocity = velocity * 0.8f;
        state = Movement.MOVE;
    }

    public void attack_animation_finished()
    {
        state = Movement.MOVE;
    }

    public void _on_EnemyDetectionZone_body_entered(Area2D area){
        var directionTowardsEnemy = GlobalPosition.DirectionTo(area.GlobalPosition);
        animationTree.Set("parameters/Idle/blend_position", directionTowardsEnemy);
        animationTree.Set("parameters/Run/blend_position", directionTowardsEnemy);
        animationTree.Set("parameters/Attack/blend_position", directionTowardsEnemy);
        animationTree.Set("parameters/Roll/blend_position", directionTowardsEnemy);

    }

    public void _on_HurtBox_area_entered(Area2D area)
    {
        int health = (int)stats.Get("health");
        stats.Set("set_health", (health - (int)area.Get("damage")));
        hurtBox.start_invincibility(0.5f);
        hurtBox.create_hit_effect();
        var playerHurtSoundInstace = playerHurtSound.Instance();
        GetTree().CurrentScene.AddChild(playerHurtSoundInstace);

    }

    public void _on_HurtBox_invinciblilityStarted()
    {
        blinkAnimationPlayer.Play("Start");
    }

    public void _on_HurtBox_invincibleEnded()
    {
        blinkAnimationPlayer.Play("Stop");
    }

    public void _on_IncreaseHealth_IncreasePlayerHealth()
    {
        int health = (int)stats.Get("health");
        stats.Set("set_health", (health + 1));
    }

    public void _on_IncreaseMaxHealth_IncreasePlayerMaxHealth()
    {
        int max_health = (int)stats.Get("max_health");
        stats.Set("set_max_health", (max_health + 1));
    }

    public void _on_ToNextLevel_SetPalyerProcessInput()
    {
        allowUserControl = false;
        levelCompeleted = true;
    }

    public void _on_VisibilityNotifier2D_screen_exited(){
        var scene = GetParent().GetNodeOrNull("../ToNextLevel") as ToNextLevel;

        if(levelCompeleted && scene.IsInsideTree() != false)
           EmitSignal("TeleportToNextLevel");
        }
}
