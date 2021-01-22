using Godot;


public class DoorInitializer : Node
{
	private void Initialize()
	{
		door = GetNode<Door>(doorNP);
		doorSystem = GetNode(doorSystemNP);
		animationPlayer = GetNode(animationPlayerNP);
		soundPlayer3D = GetNode(soundPlayer3DNP);
		mainComputer = GetTree().Root.GetNode("MainComputer");
	}

	private void InitializeDoor()
	{
		door.AddUserSignal(SignalKey.IS_UNLOCKED);
		door.AddUserSignal(SignalKey.EXECUTE_HUMAN_COMMAND);
		door.AddUserSignal(SignalKey.EXECUTE_SYSTEM_COMMAND);
		door.AddUserSignal(SignalKey.ADD_DOOR);

		door.Connect(SignalKey.IS_UNLOCKED, door, SignalMethod.IS_UNLOCKED);
		door.Connect(SignalKey.EXECUTE_HUMAN_COMMAND, door,
				SignalMethod.EXECUTE_HUMAN_COMMAND);
		door.Connect(SignalKey.EXECUTE_SYSTEM_COMMAND, door,
				SignalMethod.EXECUTE_SYSTEM_COMMAND);
		door.Connect(SignalKey.ADD_DOOR, mainComputer,
				SignalMethod.ADD_DOOR);
	}

	private void InitializeDoorSystem()
	{
		doorSystem.AddUserSignal(SignalKey.ON_DOOR_UNLOCKED);
		doorSystem.AddUserSignal(SignalKey.ON_DOOR_LOCKED);
		doorSystem.AddUserSignal(SignalKey.ON_DOOR_MOVE);

		doorSystem.Connect(SignalKey.ON_DOOR_UNLOCKED, animationPlayer,
				SignalMethod.GD_PLAY, this.CreateSingleBind("unlock"));
		doorSystem.Connect(SignalKey.ON_DOOR_LOCKED, animationPlayer,
				SignalMethod.GD_PLAY, this.CreateSingleBind("lock"));
		doorSystem.Connect(SignalKey.ON_DOOR_MOVE, soundPlayer3D,
				SignalMethod.PLAY_IF_STOPPED,
				this.CreateSingleBind(SignalKey.ON_DOOR_MOVE));
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeDoor();
		InitializeDoorSystem();
	}


	[Export]
	public NodePath doorNP;

	[Export]
	public NodePath doorSystemNP;

	[Export]
	public NodePath animationPlayerNP;

	[Export]
	public NodePath soundPlayer3DNP;


	private Node mainComputer;
	private Door door;
	private Node doorSystem;
	private Node animationPlayer;
	private Node soundPlayer3D;
}
