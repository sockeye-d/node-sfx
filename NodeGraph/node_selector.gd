extends Window

signal node_selected(selected_node: String)

@export var NODES: PackedScene

var nodes_instance: Node

# Called when the node enters the scene tree for the first time.
func _ready():
	nodes_instance = NODES.instantiate()
	$Control/ItemList.clear()
	var node_names = _get_node_list()
	node_names.sort()
	for node_name in node_names:
		$Control/ItemList.add_item(node_name)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func _type_selected(index):
	var selected_index = $Control/ItemList.get_selected_items()[0]
	var selected: String = $Control/ItemList.get_item_text(selected_index)
	node_selected.emit(selected)


func _on_item_list_item_clicked(index, at_position, mouse_button_index):
	$Control/Button.disabled = false


func _on_close_requested():
	hide()


func _get_node_list() -> PackedStringArray:
	var res: PackedStringArray = []
	for node in nodes_instance.get_children():
		res.append(node.name)
	return res
