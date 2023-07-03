class_name NodeGraph extends Control

signal connection_changed

@export var NODE_SELECTOR_WINDOW: PackedScene

# Called when the node enters the scene tree for the first time.
func _ready():
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("add_node"):
		var selected_node: String = ""
		selected_node = await _get_node_from_window()
		var node: GraphNode = _get_graph_node(selected_node).duplicate()
		$GraphEdit.add_child(node)
		node.position_offset = ($GraphEdit.scroll_offset + get_viewport().get_mouse_position()) / $GraphEdit.zoom
		if $GraphEdit.use_snap:
			node.position_offset = node.position_offset.snapped(Vector2.ONE * $GraphEdit.snap_distance)


func _get_graph_node(id: String) -> GraphNode:
	return get_node("Nodes/" + id)


func _on_graph_edit_connection_request(from_node, from_port, to_node, to_port):
	$GraphEdit.connect_node(from_node, from_port, to_node, to_port)
	get_node("GraphEdit/" + str(to_node) + "/Port" + str(to_port)).editable = false
	connection_changed.emit()


func _on_graph_edit_disconnection_request(from_node, from_port, to_node, to_port):
	$GraphEdit.disconnect_node(from_node, from_port, to_node, to_port)
	get_node("GraphEdit/" + str(to_node) + "/Port" + str(to_port)).editable = true
	connection_changed.emit()



func _get_node_from_window():
	$NodeSelector.popup_centered()
	var selected_node = await $NodeSelector.node_selected
	$NodeSelector.hide()
	return selected_node
