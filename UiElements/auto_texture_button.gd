class_name AutoTextureButton extends TextureButton

@export var normal_alpha = 0.7
@export var hover_alpha = 0.8
@export var pressed_alpha = 1.0

func _ready():
	modulate.a = normal_alpha
	connect("mouse_entered", _on_mouse_entered)
	connect("mouse_exited", _on_mouse_exited)
	connect("button_down", _on_button_down)
	connect("button_up", _on_button_up)


func _on_mouse_entered():
	modulate.a = hover_alpha


func _on_mouse_exited():
	modulate.a = normal_alpha


func _on_button_down():
	modulate.a = pressed_alpha


func _on_button_up():
	modulate.a = hover_alpha
