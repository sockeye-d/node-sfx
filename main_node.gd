extends Node2D

@export var SAMPLE_RATE: float = 22050.0

var playback_stream: AudioStreamGeneratorPlayback = null

var phase: float = 0

# Called when the node enters the scene tree for the first time.
func _ready():
	$Player.play()
	playback_stream = $Player.get_stream_playback()
	$Player.stream.mix_rate = SAMPLE_RATE


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	_fill_buffer()


func _fill_buffer():
	var increment = 440 / SAMPLE_RATE

	var to_fill = playback_stream.get_frames_available()
	while to_fill > 0:
		playback_stream.push_frame(Vector2.ONE * sin(phase * TAU) * 0.) # Audio frames are stereo.
		phase = fmod(phase + increment, 1.0)
		to_fill -= 1
