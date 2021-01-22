using Godot;


public class InformationComputer : Spatial
{
	public void UpdateInformationPage(int amount)
	{
		informationSystem.UpdateInformationPage(amount);
	}

	public void GetButtonByTouch(ulong areaInstanceId, Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET,
				informationSystemGUI.GetButtonByTouch(areaInstanceId));
	}

	public void GetLabel(byte labelId, Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET,
				informationSystemGUI.GetLabel(labelId));
	}

	private void Initialize()
	{
		informationSystemGUI = GetNode<InformationSystemGUI>(informationSystemGUINP);
		informationSystem = GetNode<InformationSystem>(informationSystemNP);
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public override void _Ready()
	{
		EmitSignal(SignalKey.ADD_INFORMATION_COMPUTER, this, roomId, computerId);
	}


	[Export]
	public NodePath informationSystemNP;

	[Export]
	public NodePath informationSystemGUINP;

	[Export]
	public byte roomId;

	[Export]
	public byte computerId;
	

	private InformationSystem informationSystem;
	private InformationSystemGUI informationSystemGUI;
}
