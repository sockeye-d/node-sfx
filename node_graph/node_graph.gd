extends Control

signal connection_changed

# Called when the node enters the scene tree for the first time.
func _ready():
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("add_node"):
		var node: GraphNode = _get_graph_node("Math").duplicate()
		$GraphEdit.add_child(node)
		node.position_offset = ($GraphEdit.scroll_offset + get_viewport().get_mouse_position()) / $GraphEdit.zoom
		if $GraphEdit.use_snap:
			node.position_offset = node.position_offset.snapped(Vector2.ONE * $GraphEdit.snap_distance)


func _get_graph_node(id: String) -> GraphNode:
	return get_node("Nodes/" + id)


func _on_graph_edit_connection_request(from_node, from_port, to_node, to_port):
	$GraphEdit.connect_node(from_node, from_port, to_node, to_port)
	connection_changed.emit()


func _on_graph_edit_disconnection_request(from_node, from_port, to_node, to_port):
	$GraphEdit.disconnect_node(from_node, from_port, to_node, to_port)
	connection_changed.emit()
