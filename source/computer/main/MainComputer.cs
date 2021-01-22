using Godot;


public class MainComputer : Control
{
	public void GetRoomInformation(byte roomId, RoomInformation roomInformation)
	{
		mainSystem.GetRoomInformation(roomId, roomInformation);
	}

	public void OnSolvingPuzzle(bool solved)
	{
		mainSystem.OnSolvingPuzzle(solved);
	}
	
	public void StartExperiment()
	{
		mainSystem.StartExperiment();
	}

	public void OnClaimAward(byte score)
	{
		mainSystem.OnClaimAward(score);
	}

	public void OnCheatingAttempt()
	{
		mainSystem.OnCheatingAttempt();
	}

	public void OnCheatingConsequences()
	{
		mainSystem.OnCheatingConsequences();
	}

	public void AddDoor(Node door, byte roomId, byte doorId)
	{
		experimentDoorSystem.AddDoor(door, roomId, doorId);
	}

	public void AddPuzzleComputer(Node computer, byte roomId, byte computerId)
	{
		experimentComputerSystem.AddPuzzleComputer(computer, roomId, computerId);
	}

	public void AddInformationComputer(Node computer, byte roomId, byte computerId)
	{
		experimentComputerSystem.AddInformationComputer(computer, roomId, computerId);
	}

	public void AddExperimentResultComputer(Node computer, byte roomId, byte computerId)
	{
		experimentComputerSystem.AddExperimentResultComputer(computer, roomId, computerId);
	}

	public void IsExperimentFinished(Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET, mainSystem.ExperimentFinished);
	}

	public void ConnectToExperimentManager(Node experimentManager)
	{
		mainComputerInitializer.ConnectToExperimentManager(experimentManager);
	}

	public void IncreaseHitsTaken()
	{
		mainSystem.IncreaseHitsTaken();
	}
	
	private void Initialize()
	{
		mainSystem = GetNode<MainSystem>(mainSystemNP);
		experimentComputerSystem = GetNode<ExperimentComputerSystem>(
				experimentComputerSystemNP);
		experimentDoorSystem = GetNode<ExperimentDoorSystem>(
				experimentDoorSystemNP);
		mainComputerInitializer = GetNode<MainComputerInitializer>(
				mainComputerInitializerNP);
	}

	public override void _EnterTree()
	{
		Initialize();
	}


	[Export]
	public NodePath mainSystemNP;

	[Export]
	public NodePath experimentComputerSystemNP;

	[Export]
	public NodePath experimentDoorSystemNP;

	[Export]
	public NodePath mainComputerInitializerNP;


	private MainSystem mainSystem;
	private ExperimentComputerSystem experimentComputerSystem;
	private ExperimentDoorSystem experimentDoorSystem;
	private MainComputerInitializer mainComputerInitializer;
}
