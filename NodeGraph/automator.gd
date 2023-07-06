extends GraphNode

var nodes: Array[BaseButton]
var keyframes: Array[Vector2]

var selected_keyframe: int = -1
var is_dragging: bool = false

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	for i in range(nodes.size()):
		if nodes[i].button_pressed and not is_dragging:
			selected_keyframe = i
			is_dragging = true
	if selected_keyframe >= 0:
		if is_dragging and Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT):
			nodes[selected_keyframe].position = $Interface/InterfaceArea.get_local_mouse_position()
		elif not Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT):
			is_dragging = false


func _on_remove_pressed():
	pass # Replace with function body.


func _on_add_pressed():
	selected_keyframe = -1
	is_dragging = false
	var node: BaseButton = $Interface/InterfaceArea/Keyframe.duplicate()
	node.show()
	nodes.append(node)
	$Interface/InterfaceArea.add_child(node)
