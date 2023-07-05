extends HBoxContainer

signal play_pressed
signal restart_pressed

@export var play_icon: Texture2D
@export var pause_icon: Texture2D

var playing: bool

# Called when the node enters the scene tree for the first time.
func _ready():
	$Play.texture_normal = play_icon


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if playing:
		$Play.texture_normal = pause_icon
	else:
		$Play.texture_normal = play_icon


func _on_restart_pressed():
	restart_pressed.emit()


func _on_play_pressed():
	play_pressed.emit()
