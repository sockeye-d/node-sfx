extends Window

signal node_selected(selected_node: String)

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func _type_selected(index):
	var selected_index = $Control/ItemList.get_selected_items()[0]
	var selected: String = $Control/ItemList.get_item_text(selected_index)
	node_selected.emit(selected)
	print(selected)


func _on_item_list_item_clicked(index, at_position, mouse_button_index):
	$Control/Button.disabled = false


func _on_close_requested():
	hide()
