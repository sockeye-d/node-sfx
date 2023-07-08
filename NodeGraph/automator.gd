class_name Automator extends GraphNode

var nodes: Array[AutoTextureButton]
var keyframes: Array[Vector2]

var selected_keyframe: int = -1
var is_dragging: bool = false
var drag_offset: Vector2 = Vector2.ZERO

# Called when the node enters the scene tree for the first time.
func _ready():
	_add_node(Vector2.ZERO)
	$Interface/InterfaceArea/Keyframe.hide()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	for i in range(nodes.size()):
		if nodes[i].button_pressed and not is_dragging:
			if selected_keyframe > -1:
				nodes[selected_keyframe].normal_alpha = 0.5
				nodes[selected_keyframe].force_update_alpha()
			selected_keyframe = i
			nodes[selected_keyframe].normal_alpha = 0.8
			is_dragging = true
			drag_offset = $Interface/InterfaceArea.get_local_mouse_position() - nodes[selected_keyframe].position
	
	if selected_keyframe > -1:
		if is_dragging and Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT):
			var pos: Vector2 = $Interface/InterfaceArea.get_local_mouse_position() - drag_offset + $Interface/InterfaceArea/Keyframe.size * 0.5
			pos = pos.clamp(Vector2.ZERO, $Interface/InterfaceArea.size) - $Interface/InterfaceArea/Keyframe.size * 0.5
			nodes[selected_keyframe].position = pos
		elif not Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT):
			is_dragging = false
	$Interface/Buttons/Remove.disabled = nodes.size() < 2 or selected_keyframe < 0
	$Interface/InterfaceArea/DrawingSurface.queue_redraw()


func _on_remove_pressed():
	nodes[selected_keyframe].queue_free()
	nodes.remove_at(selected_keyframe)
	selected_keyframe = -1
	is_dragging = false


func _on_add_pressed():
	if selected_keyframe > -1:
		nodes[selected_keyframe].normal_alpha = 0.5
		nodes[selected_keyframe].force_update_alpha()
	selected_keyframe = -1
	is_dragging = false
	var node: BaseButton = $Interface/InterfaceArea/Keyframe.duplicate()
	node.show()
	nodes.append(node)
	$Interface/InterfaceArea.add_child(node)

func _add_node(pos: Vector2):
	if selected_keyframe > -1:
		nodes[selected_keyframe].normal_alpha = 0.5
		nodes[selected_keyframe].force_update_alpha()
	selected_keyframe = -1
	is_dragging = false
	var node: AutoTextureButton = $Interface/InterfaceArea/Keyframe.duplicate()
	node.position = pos
	node.show()
	nodes.append(node)
	$Interface/InterfaceArea.add_child(node)


func _on_interface_area_focus_entered():
	if selected_keyframe > -1:
		nodes[selected_keyframe].normal_alpha = 0.5
		nodes[selected_keyframe].force_update_alpha()
	selected_keyframe = -1


func _on_drawing_surface_draw():
	var lines: Array[Vector2] = []
	if nodes.size() > 0:
		for node in nodes:
			lines.append(node.position)
		lines.sort_custom(func(a, b):
				return a.x < b.x)
		for i in range(lines.size()):
			lines[i] += $Interface/InterfaceArea/Keyframe.size * 0.5
	match nodes.size():
		0:
			lines.append(Vector2(0.0, 0.5) * $Interface/InterfaceArea.size)
			lines.append(Vector2(1.0, 0.5) * $Interface/InterfaceArea.size)
		1:
			lines.insert(0, Vector2(0.0, lines[0].y))
			lines.append(Vector2($Interface/InterfaceArea.size.x, lines[0].y))
		_:
			lines.insert(0, Vector2(0.0, lines[0].y))
			lines.append(Vector2($Interface/InterfaceArea.size.x, lines[-1].y))
	keyframes = lines.duplicate()
	for i in range(keyframes.size()):
		keyframes[i] = keyframes[i] / $Interface/InterfaceArea.size
		keyframes[i].y = 1.0 - keyframes[i].y
	$Interface/InterfaceArea/DrawingSurface.draw_polyline(lines, Color.WHITE, 1.0, true)
