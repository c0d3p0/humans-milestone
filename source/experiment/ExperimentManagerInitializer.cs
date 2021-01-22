using Godot;


public class ExperimentManagerInitializer : Node
{
	private void Initialize()
	{
		mainComputer = GetTree().Root.GetNode("MainComputer");
		experimentManager = GetNode<ExperimentManager>(experimentManagerNP);
		experimentCharacterManager = GetNode<ExperimentCharacterManager>(
				experimentCharacterManagerNP);
		experimentManagerDebug = GetNode<ExperimentManagerDebug>(
				experimentManagerDebugNP);
	}

	private void InitializeExperimentManager()
	{
		experimentManager.AddUserSignal(SignalKey.CONNECT_TO_EXPERIMENT_MANAGER);
		experimentManager.AddUserSignal(SignalKey.START_EXPERIMENT);
		experimentManager.Connect(SignalKey.CONNECT_TO_EXPERIMENT_MANAGER,
				mainComputer, SignalMethod.CONNECT_TO_EXPERIMENT_MANAGER);
		experimentManager.Connect(SignalKey.START_EXPERIMENT,
				mainComputer, SignalMethod.START_EXPERIMENT);
	}

	private void InitializeExperimentCharacterManager()
	{
		experimentCharacterManager.PlayerCharacter =
				experimentManager.PlayerCharacter;
		experimentCharacterManager.MonsterCharacterList =
				experimentManager.MonsterCharacterList;
		experimentCharacterManager.SecretMonsterCharacterList =
				experimentManager.SecretMonsterCharacterList;
		experimentCharacterManager.PlayerRespawnSpatialList =
				experimentManager.PlayerRespawnSpatialList;
		experimentCharacterManager.MonsterStartSpatialList =
				experimentManager.MonsterStartSpatialList;
		experimentCharacterManager.MonsterRespawnSpatialList =
				experimentManager.MonsterRespawnSpatialList;
	}

	private void InitializeExperimentManagerDebug()
	{
		experimentManagerDebug.WorldEnvironment = experimentManager.WorldEnvironment;
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeExperimentManager();
		InitializeExperimentCharacterManager();
		InitializeExperimentManagerDebug();
	}


	[Export]
	public NodePath experimentManagerNP;

	[Export]
	public NodePath experimentCharacterManagerNP;

	[Export]
	public NodePath experimentManagerDebugNP;


	private Node mainComputer;
	private ExperimentManager experimentManager;
	private ExperimentCharacterManager experimentCharacterManager;
	private ExperimentManagerDebug experimentManagerDebug;
}
