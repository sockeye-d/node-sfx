class_name NodeGraph extends Control

signal connection_changed

@export var NODE_SELECTOR_WINDOW: PackedScene

# Called when the node enters the scene tree for the first time.
func _ready():
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("add_node") and $GraphEdit.has_focus() == true:
		_add_node(get_viewport().get_mouse_position())


func _add_node(pos: Vector2):
	var selected_node: String = ""
	selected_node = await _get_node_from_window(Vector2i(pos))
	var node: GraphNode = _get_graph_node(selected_node).duplicate()
	$GraphEdit.add_child(node)
	node.position_offset = ($GraphEdit.scroll_offset + pos) / $GraphEdit.zoom
	if $GraphEdit.use_snap:
		node.position_offset = node.position_offset.snapped(Vector2.ONE * $GraphEdit.snap_distance)
	return node

func _get_graph_node(id: String) -> GraphNode:
	return get_node("Nodes/" + id)


func _on_graph_edit_connection_request(from_node, from_port, to_node, to_port):
	var connections: Array[Dictionary] = $GraphEdit.get_connection_list()
	for connection in connections:
		if connection.to == to_node and connection.to_port == to_port:
			$GraphEdit.disconnect_node(connection.from, connection.from_port, connection.to, connection.to_port)
			break
	$GraphEdit.connect_node(from_node, from_port, to_node, to_port)
	get_node("GraphEdit/" + str(to_node) + "/Port" + str(to_port)).editable = false
	connection_changed.emit()


func _on_graph_edit_disconnection_request(from_node, from_port, to_node, to_port):
	$GraphEdit.disconnect_node(from_node, from_port, to_node, to_port)
	get_node("GraphEdit/" + str(to_node) + "/Port" + str(to_port)).editable = true
	connection_changed.emit()



func _get_node_from_window(popup_pos: Vector2i):
	$NodeSelector.popup(Rect2i(Vector2i(popup_pos.x - $NodeSelector.size.x / 2, popup_pos.y), Vector2i($NodeSelector.size)))
	var selected_node = await $NodeSelector.node_selected
	$NodeSelector.hide()
	return selected_node


func _on_graph_edit_connection_from_empty(to_node, to_port, release_position):
	var node: GraphNode = await _add_node(release_position)
	$GraphEdit.connect_node(node.name, 0, to_node, to_port)


func _on_graph_edit_delete_nodes_request(nodes):
	var connections: Array[Dictionary] = $GraphEdit.get_connection_list()
	for node in nodes:
		for connection in connections:
			if connection.from == node:
				$GraphEdit.disconnect_node(connection.from, connection.from_port, connection.to, connection.to_port)
			if connection.to == node:
				$GraphEdit.disconnect_node(connection.from, connection.from_port, connection.to, connection.to_port)
		$GraphEdit.remove_child(get_node("GraphEdit/" + node))
