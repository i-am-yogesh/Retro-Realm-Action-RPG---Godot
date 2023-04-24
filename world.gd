extends Node2D

export (bool) var ready_transition
onready var player_marker = $PlayerMarker
onready var player = $YSort/Player

func _ready():
	if ready_transition:
		$BasicTransition.show()

func _input(event):
	if event.is_action_pressed('Map'):
		$Camera2D.zoom = Vector2(5,4)
		pause_mode = PAUSE_MODE_STOP
		player_marker.global_position.x = player.global_position.x
		player_marker.global_position.y = player.global_position.y - 50
		player_marker.show()
	if event.is_action_released("Map"):
		$Camera2D.zoom = Vector2(1.4,1.4)
		$YSort/Player.pause_mode = PAUSE_MODE_PROCESS
		player_marker.hide()
