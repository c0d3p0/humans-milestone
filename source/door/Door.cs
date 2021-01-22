using Godot;


public class Door : Spatial
{
	public void ExecuteHumanCommand(bool unlock)
	{
		doorSystem.ExecuteHumanCommand(unlock);
	}

	public void ExecuteSystemCommand(bool unlock)
	{
		doorSystem.ExecuteSystemCommand(unlock);
	}

	public void IsUnlocked(Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET, doorSystem.Unlocked);
	}

	private void Initialize()
	{
		doorSystem = GetNode<DoorSystem>(doorSystemNP);
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public override void _Ready()
	{
		EmitSignal(SignalKey.ADD_DOOR, this, roomId, doorId);
	}
	

	[Export]
	public NodePath doorSystemNP;
	
	[Export]
	public byte roomId;

	[Export]
	public byte doorId;


	private DoorSystem doorSystem;
}
