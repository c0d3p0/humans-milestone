using Godot;


public class MonsterSensorInitializer : Node
{
	private void Initialize()
	{
		monsterSensor = GetNode<MonsterSensor>(monsterSensorNP);
		monsterSensorSystem = GetNode<MonsterSensorSystem>(monsterSensorSystemNP);
		mainComputer = GetTree().Root.GetNode("MainComputer");
	}

	private void InitializeMonsterSensorSystem()
	{
		monsterSensorSystem.Door = monsterSensor.Door;

		monsterSensorSystem.AddUserSignal(SignalKey.ON_CHEATING_ATTEMPT);
		monsterSensorSystem.AddUserSignal(SignalKey.ON_CHEATING_CONSEQUENCES);

		monsterSensorSystem.Connect(SignalKey.ON_CHEATING_ATTEMPT,
				mainComputer, SignalMethod.ON_CHEATING_ATTEMPT);
		monsterSensorSystem.Connect(SignalKey.ON_CHEATING_CONSEQUENCES,
				mainComputer, SignalMethod.ON_CHEATING_CONSEQUENCES);
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeMonsterSensorSystem();
	}


	[Export]
	public NodePath monsterSensorNP;

	[Export]
	public NodePath monsterSensorSystemNP;


	private Node mainComputer;
	private MonsterSensor monsterSensor;
	private MonsterSensorSystem monsterSensorSystem;
}
