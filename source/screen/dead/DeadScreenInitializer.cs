using Godot;


public class DeadScreenInitializer : Node
{
	private void Initialize()
	{
		deadScreenManager = GetNode(deadScreenManagerNP);
		deadScreen = GetNode<DeadScreen>(deadScreenNP);
		mainComputer = GetTree().Root.GetNode("MainComputer");
	}

	private void InitializeDeadScreenManager()
	{
		deadScreenManager.AddUserSignal(SignalKey.RESTART_CHARACTERS);
		deadScreenManager.AddUserSignal(SignalKey.IS_EXPERIMENT_FINISHED);

		deadScreenManager.Connect(SignalKey.RESTART_CHARACTERS,
				deadScreen.ExperimentManager, SignalMethod.RESTART_CHARACTERS);
		deadScreenManager.Connect(SignalKey.RESTART_CHARACTERS,
				mainComputer, SignalMethod.INCREASE_HITS_TAKEN);
		deadScreenManager.Connect(SignalKey.IS_EXPERIMENT_FINISHED,
				mainComputer, SignalMethod.IS_EXPERIMENT_FINISHED);
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeDeadScreenManager();
	}


	[Export]
	public NodePath deadScreenNP;

	[Export]
	public NodePath deadScreenManagerNP;


	private DeadScreen deadScreen;
	private Node deadScreenManager;
	private Node mainComputer;
}
