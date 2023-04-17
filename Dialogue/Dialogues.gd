extends CanvasLayer

export(String, FILE, "*.json") var d_file

var diagloue = []
var current_diagloue_id = 0
var d_active = false

func _ready():
	$NinePatchRect.visible = true
	
func start():
	if d_active:
		return
	
	d_active = true
	$NinePatchRect.visible = true
	diagloue = load_diagloue()
	current_diagloue_id = -1
	next_diagloue()
	
func load_diagloue():
	var file = File.new()
	if file.file_exists(d_file):
		file.open(d_file, file.READ)
		return parse_json(file.get_as_text())

func _input(event):
	if not d_active:
		return
		
	if event.is_action_pressed("ui_accept"):
		next_diagloue()

func next_diagloue():
	current_diagloue_id += 1
	
	if current_diagloue_id >= len(diagloue):
		$Timer.start()
		$NinePatchRect.visible = false
		return
	
	$NinePatchRect/Name.text= diagloue[current_diagloue_id]['name']
	$NinePatchRect/Chat.text = diagloue[current_diagloue_id]['text']
	$NinePatchRect/Sprite.texture = load(diagloue[current_diagloue_id]['image'])


func _on_Timer_timeout():
	d_active = false
