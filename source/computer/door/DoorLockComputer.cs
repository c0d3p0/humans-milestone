using Godot;


public class DoorLockComputer : Node
{
	private void Initialize()
	{
		doorLockSystemGUI = GetNode<DoorLockSystemGUI>(doorLockSystemGUINP);
		doorLockSystemGUI.Door = GetNode(doorNP);
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public void GetButtonByTouch(ulong areaInstanceId, Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET,
				doorLockSystemGUI.GetButtonByTouch(areaInstanceId));
	}


	[Export]
	public NodePath doorNP;

	[Export]
	public NodePath doorLockSystemGUINP;


	private DoorLockSystemGUI doorLockSystemGUI;
}
