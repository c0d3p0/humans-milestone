using Godot;


public class MainComputerInitializer : Node
{
	public void ConnectToExperimentManager(Node experimentManager)
	{
		mainSystem.AddUserSignal(SignalKey.ON_EXPERIMENT_LEVEL_CHANGED);
		mainSystem.AddUserSignal(SignalKey.ON_CLAIM_AWARD);

		mainSystem.Connect(SignalKey.ON_EXPERIMENT_LEVEL_CHANGED,
				experimentManager, SignalMethod.ON_EXPERIMENT_LEVEL_CHANGED);
		mainSystem.Connect(SignalKey.ON_CLAIM_AWARD,
				experimentManager, SignalMethod.ON_CLAIM_AWARD);
		mainSystem.Connect(SignalKey.CHEATING_CONSEQUENCES,
				experimentManager, SignalMethod.ON_CHEATING_CONSEQUENCES);
	}

	private void Initialize()
	{
		mainComputer = GetNode<MainComputer>(mainComputerNP);
		mainSystem = GetNode<MainSystem>(mainSystemNP);
		experimentDoorSystem = GetNode<ExperimentDoorSystem>(
				experimentDoorSystemNP);
		experimentComputerSystem = GetNode<ExperimentComputerSystem>(
				experimentComputerSystemNP);
		animationNSMP = GetNode<AnimationTree>(animationTreeNP).Get(
				"parameters/playback") as AnimationNodeStateMachinePlayback;
		animationStateMachinePlayer = GetNode<AnimationStateMachinePlayer>(
				animationStateMachinePlayerNP);
	}

	private void InitializeMainSystem()
	{
		mainSystem.AddUserSignal(SignalKey.SET_DOOR_UNLOCKED);
		mainSystem.AddUserSignal(SignalKey.SET_DOORS_UNLOCKED);
		mainSystem.AddUserSignal(SignalKey.LOCK_RANDOM_DOORS);

		mainSystem.AddUserSignal(SignalKey.SET_PUZZLE_COMPUTER_PUZZLE);
		mainSystem.AddUserSignal(SignalKey.SET_PUZZLE_COMPUTERS_RANDOM_PUZZLES);
		mainSystem.AddUserSignal(SignalKey.SET_ROOM_PUZZLE_COMPUTERS_ACTIVE);
		mainSystem.AddUserSignal(SignalKey.UPDATE_ALL_INFORMATION_COMPUTERS);
		mainSystem.AddUserSignal(SignalKey.UPDATE_ALL_EXPERIMENT_RESULT_COMPUTERS);

		mainSystem.AddUserSignal(SignalKey.ON_EXPERIMENT_STARTED);

		mainSystem.AddUserSignal(SignalKey.HELLO_SUBJECT_1054);
		mainSystem.AddUserSignal(SignalKey.HELLO_SUBJECT_2948);
		mainSystem.AddUserSignal(SignalKey.HELLO_SUBJECT_3682);
		mainSystem.AddUserSignal(SignalKey.HELLO_SUBJECT_4706);
		mainSystem.AddUserSignal(SignalKey.ON_WRONG_PASSWORD);
		mainSystem.AddUserSignal(SignalKey.ON_CORRECT_PASSWORD);
		mainSystem.AddUserSignal(SignalKey.ON_EXPERIMENT_UPDATE);
		mainSystem.AddUserSignal(SignalKey.ON_ALL_PUZZLE_SOLVED);
		mainSystem.AddUserSignal(SignalKey.HIGH_SCORE);
		mainSystem.AddUserSignal(SignalKey.AVERAGE_SCORE);
		mainSystem.AddUserSignal(SignalKey.LOW_SCORE);
		mainSystem.AddUserSignal(SignalKey.SPEECH_END);
		mainSystem.AddUserSignal(SignalKey.ANTI_CHEAT_WARNING);
		mainSystem.AddUserSignal(SignalKey.CHEATING_CONSEQUENCES);

		mainSystem.Connect(SignalKey.SET_DOOR_UNLOCKED,
				experimentDoorSystem, SignalMethod.SET_DOOR_UNLOCKED);
		mainSystem.Connect(SignalKey.SET_DOORS_UNLOCKED,
				experimentDoorSystem, SignalMethod.SET_DOORS_UNLOCKED);
		mainSystem.Connect(SignalKey.LOCK_RANDOM_DOORS,
				experimentDoorSystem, SignalMethod.LOCK_RANDOM_DOORS);

		mainSystem.Connect(SignalKey.SET_PUZZLE_COMPUTER_PUZZLE,
				experimentComputerSystem, SignalMethod.SET_PUZZLE_COMPUTER_PUZZLE);
		mainSystem.Connect(SignalKey.SET_PUZZLE_COMPUTERS_RANDOM_PUZZLES,
				experimentComputerSystem, SignalMethod.SET_PUZZLE_COMPUTERS_RANDOM_PUZZLES);
		mainSystem.Connect(SignalKey.SET_ROOM_PUZZLE_COMPUTERS_ACTIVE,
				experimentComputerSystem, SignalMethod.SET_ROOM_PUZZLE_COMPUTERS_ACTIVE);
		mainSystem.Connect(SignalKey.UPDATE_ALL_INFORMATION_COMPUTERS,
				experimentComputerSystem, SignalMethod.UPDATE_ALL_INFORMATION_COMPUTERS);
		mainSystem.Connect(SignalKey.UPDATE_ALL_EXPERIMENT_RESULT_COMPUTERS,
				experimentComputerSystem, SignalMethod.UPDATE_ALL_EXPERIMENT_RESULT_COMPUTERS);

		mainSystem.Connect(SignalKey.ON_EXPERIMENT_STARTED, GetNode(bgMusicAPNP),
				SignalMethod.GD_PLAY, this.CreateSingleBind("bg_music"));

		mainSystem.Connect(SignalKey.HELLO_SUBJECT_1054, animationStateMachinePlayer,
				SignalMethod.TRAVEL_AND_PLAY, this.CreateSingleBind("hello_1054"));
		mainSystem.Connect(SignalKey.HELLO_SUBJECT_2948, animationStateMachinePlayer,
				SignalMethod.TRAVEL_AND_PLAY, this.CreateSingleBind("hello_2948"));
		mainSystem.Connect(SignalKey.HELLO_SUBJECT_3682, animationStateMachinePlayer,
				SignalMethod.TRAVEL_AND_PLAY, this.CreateSingleBind("hello_3682"));
		mainSystem.Connect(SignalKey.HELLO_SUBJECT_4706, animationStateMachinePlayer,
				SignalMethod.TRAVEL_AND_PLAY, this.CreateSingleBind("hello_4706"));
		mainSystem.Connect(SignalKey.ON_WRONG_PASSWORD, animationStateMachinePlayer,
				SignalMethod.TRAVEL_AND_PLAY, this.CreateSingleBind("wrong_password"));
		mainSystem.Connect(SignalKey.ON_CORRECT_PASSWORD, animationStateMachinePlayer,
				SignalMethod.TRAVEL_AND_PLAY, this.CreateSingleBind("correct_password"));
		mainSystem.Connect(SignalKey.ON_EXPERIMENT_UPDATE, animationStateMachinePlayer,
				SignalMethod.TRAVEL_AND_PLAY, this.CreateSingleBind("experiment_update"));
		mainSystem.Connect(SignalKey.ON_ALL_PUZZLE_SOLVED, animationStateMachinePlayer,
				SignalMethod.TRAVEL_AND_PLAY, this.CreateSingleBind("congratulations"));
		mainSystem.Connect(SignalKey.HIGH_SCORE, animationStateMachinePlayer,
				SignalMethod.TRAVEL_AND_PLAY, this.CreateSingleBind("high_score"));
		mainSystem.Connect(SignalKey.AVERAGE_SCORE, animationStateMachinePlayer,
				SignalMethod.TRAVEL_AND_PLAY, this.CreateSingleBind("average_score"));
		mainSystem.Connect(SignalKey.LOW_SCORE, animationStateMachinePlayer,
				SignalMethod.TRAVEL_AND_PLAY, this.CreateSingleBind("low_score"));
		mainSystem.Connect(SignalKey.SPEECH_END, animationStateMachinePlayer,
				SignalMethod.TRAVEL_AND_PLAY, this.CreateSingleBind("speech_end"));
		mainSystem.Connect(SignalKey.ANTI_CHEAT_WARNING, animationStateMachinePlayer,
				SignalMethod.TRAVEL_AND_PLAY, this.CreateSingleBind("anti_cheat_warning"));
		mainSystem.Connect(SignalKey.CHEATING_CONSEQUENCES, animationStateMachinePlayer,
				SignalMethod.TRAVEL_AND_PLAY, this.CreateSingleBind("cheating_consequences"));
	}

	private void InitializeExperimentDoorSystem()
	{
		experimentDoorSystem.DoorMap = mainSystem.DoorMap;
	}

	private void InitializeExperimentComputerSystem()
	{
		experimentComputerSystem.PuzzleComputerMap =
				mainSystem.PuzzleComputerMap;
		experimentComputerSystem.InformationComputerMap =
				mainSystem.InformationComputerMap;
		experimentComputerSystem.ExperimentResultComputerMap =
				mainSystem.ExperimentResultComputerMap;
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeMainSystem();
		InitializeExperimentDoorSystem();
		InitializeExperimentComputerSystem();
	}


	[Export]
	public NodePath mainComputerNP;

	[Export]
	public NodePath mainSystemNP;

	[Export]
	public NodePath experimentDoorSystemNP;

	[Export]
	public NodePath experimentComputerSystemNP;

	[Export]
	public NodePath bgMusicAPNP;

	[Export]
	public NodePath animationTreeNP;

	[Export]
	public NodePath animationStateMachinePlayerNP;


	private MainComputer mainComputer;
	private MainSystem mainSystem;
	private ExperimentDoorSystem experimentDoorSystem;
	private ExperimentComputerSystem experimentComputerSystem;
	private AnimationNodeStateMachinePlayback animationNSMP;
	private AnimationStateMachinePlayer animationStateMachinePlayer;
}
