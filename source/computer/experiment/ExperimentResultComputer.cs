using Godot;


public class ExperimentResultComputer : Node
{
	public void UpdateExperimentDataPage(ExperimentResultData experimentResultData)
	{
		experimentResultSystem.UpdateExperimentDataPage(experimentResultData);
	}

	public void GetButtonByTouch(ulong areaInstanceId, Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET,
				experimentResultSystemGUI.GetButtonByTouch(areaInstanceId));
	}

	public void GetLabel(byte labelId, Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET,
				experimentResultSystemGUI.GetLabel(labelId));
	}

	public void GetButton(byte buttonId, Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET,
				experimentResultSystemGUI.GetButton(buttonId));
	}

	private void Initialize()
	{
		experimentResultSystem =
				GetNode<ExperimentResultSystem>(experimentResultSystemNP);
		experimentResultSystemGUI =
				GetNode<ExperimentResultSystemGUI>(experimentResultSystemGUINP);
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public override void _Ready()
	{
		EmitSignal(SignalKey.ADD_EXPERIMENT_RESULT_COMPUTER,
				this, roomId, computerId);
	}


	[Export]
	public NodePath experimentResultSystemNP;

	[Export]
	public NodePath experimentResultSystemGUINP;

	[Export]
	public byte roomId;

	[Export]
	public byte computerId;
	

	private ExperimentResultSystem experimentResultSystem;
	private ExperimentResultSystemGUI experimentResultSystemGUI;
}
