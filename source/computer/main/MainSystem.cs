using System.Text;

using Godot;
using Godot.Collections;


public class MainSystem : Node
{
	public void GetRoomInformation(byte roomId, RoomInformation roomInformation)
	{
		roomInformation.ClearAll();

		if(roomId < 3)
		{	
			Node n;

			for(byte i = 0; i < 3; i++)
			{
				if(doorMap.TryGetValue(GetObjectKey(roomId, i), out n))
				{
					roomInformation.unlockedDoors.Add(
							this.EmitSignal<bool>(n, SignalKey.IS_UNLOCKED));
				}		
			}

			for(byte i = 0; i < 4; i++)
			{
				if(puzzleComputerMap.TryGetValue(GetObjectKey(roomId, i), out n))
				{
					roomInformation.solvedPuzzles.Add(
							this.EmitSignal<bool>(n, SignalKey.IS_PUZZLE_SOLVED));
				}		
			}

			roomInformation.roomTitle = GetRoomName(roomId);
			roomInformation.validInformation = true;
		}
	}

	public void OnSolvingPuzzle(bool solved)
	{
		if(solved)
		{
			IncreaseExperimentLevel(1);

			if(experimentLevel < 12)
				EmitSignal(SignalKey.ON_CORRECT_PASSWORD);
			else
				EmitSignal(SignalKey.ON_ALL_PUZZLE_SOLVED);
		}
		else
			EmitSignal(SignalKey.ON_WRONG_PASSWORD);
	}

	public void StartExperiment()
	{
		experimentLevel = -1;
		EmitSignal(SignalKey.SET_PUZZLE_COMPUTER_PUZZLE, PUZZLE_MAIN_ROOM_ID,
				SAMPLE_PUZZLE_COMPUTER_ID);
		EmitSignal(SignalKey.SET_PUZZLE_COMPUTERS_RANDOM_PUZZLES);
		SetSecretDoorUnlocked(false);
		HandleCurrentExperimentLevel();
		experimentStartTime = OS.GetTicksMsec();
		HandleRandomSubjectID();
	}

	public void IncreaseExperimentLevel(sbyte amount)
	{
		experimentLevel += amount;

		if(experimentLevel > 11)
			experimentEndTime = OS.GetTicksMsec();
				
		HandleCurrentExperimentLevel();
		EmitSignal(SignalKey.UPDATE_ALL_EXPERIMENT_RESULT_COMPUTERS, subjectID,
				experimentStartTime, experimentEndTime, experimentLevel, hitsTaken);
		EmitSignal(SignalKey.ON_EXPERIMENT_LEVEL_CHANGED, experimentLevel);
	}

	public void OnClaimAward(byte score)
	{
		EmitSignal(SignalKey.SET_DOORS_UNLOCKED, false, false);

		if(score > 79)
			EmitSignal(SignalKey.HIGH_SCORE);
		else if (score > 59)
			EmitSignal(SignalKey.AVERAGE_SCORE);
		else
			EmitSignal(SignalKey.LOW_SCORE);
	}

	public void OnFirstInformationGiven() // Called by the_experiment animation
	{
		EmitSignal(SignalKey.SET_DOOR_UNLOCKED, MAIN_SMALL_ROOM_ID,
				FIRST_DOOR_ID, true);
		EmitSignal(SignalKey.ON_EXPERIMENT_STARTED);
	}

	public void OnExperimentUpdateTimeout()
	{
		if(experimentLevel > 1 && experimentLevel < 12)
		{
			LockRandomDoors();
			EmitSignal(SignalKey.ON_EXPERIMENT_UPDATE);
		}
	}

	public void OnCheatingAttempt()
	{
		if(!playerLockedMonster)
			EmitSignal(SignalKey.ANTI_CHEAT_WARNING);
	}

	public void OnCheatingConsequences()
	{
		if(!playerLockedMonster)
		{
			playerLockedMonster = true;
			EmitSignal(SignalKey.CHEATING_CONSEQUENCES);
		}
	}

	public void SetSecretDoorUnlocked(bool unlocked)
	{
		EmitSignal(SignalKey.SET_DOOR_UNLOCKED, MAIN_SMALL_ROOM_ID,
				SECRET_DOOR_ID, unlocked);
	}

	public void IncreaseHitsTaken()
	{
		hitsTaken++;
	}

	private void HandleCurrentExperimentLevel()
	{
		EmitSignal(SignalKey.SET_ROOM_PUZZLE_COMPUTERS_ACTIVE,
				0, experimentLevel > 7);
		EmitSignal(SignalKey.SET_ROOM_PUZZLE_COMPUTERS_ACTIVE,
				1, experimentLevel > -1);
		EmitSignal(SignalKey.SET_ROOM_PUZZLE_COMPUTERS_ACTIVE,
				2, experimentLevel > -1);

		if(experimentLevel < 0) // Experiment first level.
		{ 
			SetSecretDoorUnlocked(false);
			SetSecretRoomDoorsUnlocked(false);
			SetExperimentDoorsUnlocked(false);
		}
		else if(experimentLevel > 11) // Experiment end level.
		{
			SetSecretDoorUnlocked(true);
			SetExperimentDoorsUnlocked(true);
		}
		else	// Experiment mid levels.
		{
			SetSecretDoorUnlocked(false);
			LockRandomDoors();
		}
	}

	private void SetExperimentDoorsUnlocked(bool unlocked)
	{
		EmitSignal(SignalKey.SET_DOORS_UNLOCKED, unlocked, true);
		EmitSignal(SignalKey.SET_DOOR_UNLOCKED, MAIN_SMALL_ROOM_ID,
				FIRST_DOOR_ID, unlocked);
		EmitSignal(SignalKey.UPDATE_ALL_INFORMATION_COMPUTERS);
	}

	private void SetSecretRoomDoorsUnlocked(bool unlocked)
	{
		for(int i = 0; i < 10; i++)
			EmitSignal(SignalKey.SET_DOOR_UNLOCKED, SECRET_ROOM_ID, i, unlocked);

		if(unlocked)
			EmitSignal(SignalKey.ON_CLAIM_AWARD);
	}

	private void LockRandomDoors()
	{
		EmitSignal(SignalKey.LOCK_RANDOM_DOORS, experimentLevel);
		EmitSignal(SignalKey.UPDATE_ALL_INFORMATION_COMPUTERS);
	}

	private void HandleRandomSubjectID()
	{
		ushort[] ids = new ushort[]{1054, 2948, 3682, 4706};
		string[] animations = new string[]{SignalKey.HELLO_SUBJECT_1054,
				SignalKey.HELLO_SUBJECT_2948, SignalKey.HELLO_SUBJECT_3682,
				SignalKey.HELLO_SUBJECT_4706};
		int index = this.RandiRange(rng, 0, ids.Length - 1);
		subjectID = ids[index];
		EmitSignal(animations[index]);
	}

	private short GetObjectKey(byte roomId, byte objectId)
	{
		return (short) ((roomId * 100) + objectId);
	}

	private string GetRoomName(byte roomId)
	{
		StringBuilder sb = new StringBuilder();

		if(roomId == PUZZLE_MAIN_ROOM_ID)
			sb.Append("Puzzle Main Room (Exp. Lvl. ");
		else if(roomId == PUZZLE_ROOM_1_ID)
			sb.Append("Puzzle Room 1 (Exp. Lvl. ");
		else if(roomId == PUZZLE_ROOM_2_ID)
			sb.Append("Puzzle Room 2 (Exp. Lvl. ");

		if(sb.Length > 0)
			sb.Append(experimentLevel).Append(")");

		return sb.ToString();
	}

	private void Initialize()
	{
		doorMap = new Dictionary<short, Node>();
		puzzleComputerMap = new Dictionary<short, Node>();
		informationComputerMap = new Dictionary<short, Node>();
		experimentResultComputerMap = new Dictionary<short, Node>();
		rng = new RandomNumberGenerator();
	}

	public override void _EnterTree()
	{
		Initialize();
	}
	
	public Dictionary<short, Node> DoorMap
	{
		get
		{
			return doorMap;
		}
	}

	public Dictionary<short, Node> PuzzleComputerMap
	{
		get
		{
			return puzzleComputerMap;
		}
	}

	public Dictionary<short, Node> InformationComputerMap
	{
		get
		{
			return informationComputerMap;
		}
	}

	public Dictionary<short, Node> ExperimentResultComputerMap
	{
		get
		{
			return experimentResultComputerMap;
		}
	}

	public bool ExperimentFinished
	{
		get
		{
			return experimentLevel > 11;
		}
	}


	private Dictionary<short, Node> doorMap;
	private Dictionary<short, Node> puzzleComputerMap;
	private Dictionary<short, Node> informationComputerMap;
	private Dictionary<short, Node> experimentResultComputerMap;

	private RandomNumberGenerator rng;
	private ushort subjectID;
	private uint experimentStartTime;
	private uint experimentEndTime;
	private sbyte experimentLevel;
	private ushort hitsTaken;
	private bool playerLockedMonster;

	public const byte PUZZLE_MAIN_ROOM_ID = 0;
	private const byte PUZZLE_ROOM_1_ID = 1;
	private const byte PUZZLE_ROOM_2_ID = 2;
	private const byte INFO_ROOM_1_ID = 3;
	private const byte INFO_ROOM_2_ID = 4;
	private const byte SECRET_ROOM_ID = 5;
	private const byte MAIN_SMALL_ROOM_ID = 10;
	private const byte SMALL_ROOM_1_ID = 11;
	private const byte SMALL_ROOM_2_ID = 12;
	private const byte SMALL_ROOM_3_ID = 13;

	private const byte SECRET_DOOR_ID = 9;
	private const byte FIRST_DOOR_ID = 5;
	private const byte SAMPLE_PUZZLE_COMPUTER_ID = 9;
}
