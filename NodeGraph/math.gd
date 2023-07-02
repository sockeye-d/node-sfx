extends GraphNode


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var path: NodePath = get_path()
	title = path.get_name(path.get_name_count() - 1)
#	if path.get_name(path.get_name_count() - 1).contains("@"):
#		title = path.get_name(path.get_name_count() - 1).split("@")[1]
