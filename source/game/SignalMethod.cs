public class SignalMethod
{
	// LoadScreen
	public const string SET_SCENE_PATH = "SetScenePath";
	//

	// GlobalValue
	public const string GET = "Get";
	public const string PUT = "Put";
	public const string REMOVE = "Remove";
	//
	
	// MainComputer
	public const string GET_ROOM_INFORMATION = "GetRoomInformation";
	public const string ON_SOLVING_PUZZLE = "OnSolvingPuzzle";
	public const string ADD_DOOR = "AddDoor";
	public const string ADD_PUZZLE_COMPUTER = "AddPuzzleComputer";
	public const string ADD_INFORMATION_COMPUTER = "AddInformationComputer";
	public const string ADD_EXPERIMENT_RESULT_COMPUTER = "AddExperimentResultComputer";
	public const string START_EXPERIMENT = "StartExperiment";
	public const string IS_EXPERIMENT_FINISHED = "IsExperimentFinished";
	public const string ON_CLAIM_AWARD = "OnClaimAward";
	public const string CONNECT_TO_EXPERIMENT_MANAGER = "ConnectToExperimentManager";
	public const string INCREASE_HITS_TAKEN = "IncreaseHitsTaken";
	public const string ON_CHEATING_ATTEMPT = "OnCheatingAttempt";
	public const string ON_CHEATING_CONSEQUENCES = "OnCheatingConsequences";
	//

	// ExperimentDoorSystem
	public const string LOCK_RANDOM_DOORS = "LockRandomDoors";
	public const string SET_DOORS_UNLOCKED = "SetDoorsUnlocked";
	public const string SET_DOOR_UNLOCKED = "SetDoorUnlocked";
	//

	// ExperimentComputerSystem
	public const string SET_PUZZLE_COMPUTERS_RANDOM_PUZZLES = "SetPuzzleComputersRandomPuzzles";
	public const string SET_PUZZLE_COMPUTER_PUZZLE = "SetPuzzleComputerPuzzle";
	public const string SET_ROOM_PUZZLE_COMPUTERS_ACTIVE = "SetRoomPuzzleComputersActive";
	public const string UPDATE_ALL_INFORMATION_COMPUTERS = "UpdateAllInformationComputers";
	public const string UPDATE_ALL_EXPERIMENT_RESULT_COMPUTERS = "UpdateAllExperimentResultComputers";
	//

	// Door
	public const string IS_UNLOCKED = "IsUnlocked";
	public const string EXECUTE_HUMAN_COMMAND = "ExecuteHumanCommand";
	public const string EXECUTE_SYSTEM_COMMAND = "ExecuteSystemCommand";
	//

	// PuzzleComputer, InformationComputer
	public const string GET_BUTTON_BY_TOUCH = "GetButtonByTouch";
	public const string GET_LABEL = "GetLabel";
	//

	// PuzzleComputer, ExperimentResultComputer
	public const string GET_BUTTON = "GetButton";
	//

	// PuzzleComputer
	public const string GET_TEXTURE_RECT = "GetTextureRect";
	public const string SET_SYSTEM_ACTIVE = "SetSystemActive";
	public const string SET_PUZZLE = "SetPuzzle";
	public const string IS_PUZZLE_SOLVED = "IsPuzzleSolved";
	public const string UPDATE_GUI_STATE = "UpdateGUIState";
	//

	// InformationComputer
	public const string UPDATE_INFORMATION_PAGE = "UpdateInformationPage";
	//

	// ExperimentResultComputer 
	public const string UPDATE_EXPERIMENT_DATA_PAGE = "UpdateExperimentDataPage";
	// 

	// ComputerTouchScreen
	public const string ON_INTERACTION_RECEIVED = "OnInteractionReceived";
	//

	// PlayerCharacter, MonsterCharacter
	public const string GET_DIRECTION = "GetDirection";
	public const string CANNOT_MOVE = "CannotMove";
	public const string SET_CHARACTER_MOVE_ENABLED = "SetCharacterMoveEnabled";
	//

	// PlayerCharacter
	public const string SHOULD_WALK = "ShouldWalk";
	public const string SHOULD_INTERACT = "ShouldInteract";
	//

	// CharacterLook
	public const string EXECUTE_LOOK = "ExecuteLook";
	//

	// PlayerMainAction, MonsterMainAction
	public const string UPDATE_ACTIVE = "UpdateActive";
	//

	// MonsterCharacter
	public const string ACTIVATE_KILLER_DEVICE = "ActivateKillerDevice";
	//
	

	// PlayerMainAction
	public const string PANIC = "Panic";
	public const string DIE = "Die";
	//

	// MonsterAI
	public const string CREATE_PATH_TO_SEARCH_POINT = "CreatePathToSearchPoint";
	public const string CLEAR_TARGET_AND_ALL_PATHS = "ClearTargetAndAllPaths";

	
	//

	// MonsterMainAction
	public const string LOOK_AT = "LookAt";
	//

	// ExperimentManager
	public const string RESTART_CHARACTERS = "RestartCharacters";
	public const string ON_EXPERIMENT_LEVEL_CHANGED = "OnExperimentLevelChanged";
	//

	// DeadScreen
	public const string SHOW_DEAD_SCREEN = "ShowDeadScreen";
	//

	// AnimationStateMachinePlayer
	public const string TRAVEL_AND_PLAY = "TravelAndPlay";
	//

	// SoundPlayer3D
	public const string PLAY = "Play";
	public const string PLAY_IF_STOPPED = "PlayIfStopped";
	//

	// Native
	public const string GD_SET_PROCESS = "set_process";
	public const string GD_PLAY = "play";
	public const string GD_TRAVEL = "travel";
	public const string GD_STOP = "stop";
	//
}
