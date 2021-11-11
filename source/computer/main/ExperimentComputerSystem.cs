using SCG = System.Collections.Generic;

using Godot;
using Godot.Collections;


public class ExperimentComputerSystem : Node
{
	public void SetPuzzleComputersRandomPuzzles()
	{
		HashList<int> puzzleIdList = new HashList<int>();
		int index;

		for(byte i = 1; i < PUZZLE_AMOUNT; i++)
			puzzleIdList.Add(i);

		for(byte i = 0; i < 3; i++)
		{
			index = this.RandiRange(rng, 0, puzzleIdList.Count - 1);
			SetRoomPuzzleComputersPuzzle(i, puzzleIdList[index]);
			puzzleIdList.RemoveAt(index);
		}

		puzzleIdList.Free();
	}
	
	public void SetPuzzleComputerPuzzle(byte roomId, byte computerId)
	{
		short id = GetComputerKey(roomId, computerId);
		Node c;

		if(puzzleComputerMap.TryGetValue(id, out c))
			c.EmitSignal(SignalKey.SET_PUZZLE, CreatePuzzle(0));
	}

	public void SetRoomPuzzleComputersActive(byte roomId, bool active)
	{
		Node c;

		for(byte i = 0; i < 5; i++)
		{
			if(puzzleComputerMap.TryGetValue(GetComputerKey(roomId, i), out c))
				c.EmitSignal(SignalKey.SET_SYSTEM_ACTIVE, active);
		}
	}
	
	public void UpdateAllInformationComputers()
	{
		SCG.IEnumerator<SCG.KeyValuePair<int, Node>> it =
				informationComputerMap.GetEnumerator();

		while(it.MoveNext())
			it.Current.Value.EmitSignal(SignalKey.UPDATE_INFORMATION_PAGE, 0);
	}

	public void UpdateAllExperimentResultComputers(short subjectID,
			uint experimentStartTime, uint experimentEndTime,
			sbyte experimentLevel, ushort hitsTaken)
	{
		ExperimentResultData erd = GetExperimentResultData(subjectID,
				experimentStartTime, experimentEndTime, experimentLevel, hitsTaken);
		SCG.IEnumerator<SCG.KeyValuePair<int, Node>> it =
				experimentResultComputerMap.GetEnumerator();

		while(it.MoveNext())
			it.Current.Value.EmitSignal(SignalKey.UPDATE_EXPERIMENT_DATA_PAGE, erd);
	}

	public void AddPuzzleComputer(Node computer,
			byte roomId, byte computerId)
	{
		puzzleComputerMap.Add(GetComputerKey(roomId, computerId), computer);
	}

	public void AddInformationComputer(Node computer,
			byte roomId, byte computerId)
	{
		informationComputerMap.Add(GetComputerKey(roomId, computerId), computer);
	}

	public void AddExperimentResultComputer(Node computer,
			byte roomId, byte computerId)
	{
		experimentResultComputerMap.Add(GetComputerKey(roomId, computerId), computer);
	}

	private void SetRoomPuzzleComputersPuzzle(int roomId, int puzzleId)
	{
		short computerId;
		Node pcn;

		for(byte pcId = 0; pcId < 5; pcId++)
		{
			computerId = GetComputerKey(roomId, pcId);

			if(puzzleComputerMap.TryGetValue(computerId, out pcn))
				pcn.EmitSignal(SignalKey.SET_PUZZLE, CreatePuzzle(puzzleId));
		}
	}

	private Puzzle CreatePuzzle(int puzzleId)
	{
		if(puzzleId == 0)
			return new AnyDividedBy0Puzzle();
		else if(puzzleId == 1)
			return new TheLawOfTheRectangleV1Puzzle();
		else if(puzzleId == 2)
			return new KeysToTheTreasureV1Puzzle();
		else if(puzzleId == 3)
			return new NotSoHiddenV1Puzzle();
		else if(puzzleId == 4)
			return new KnowledgeGivenByTheBookV1Puzzle();
		else if(puzzleId == 5)
			return new CarvedInTheCoffinsV1Puzzle();
		else if(puzzleId == 6)
			return new GuidedByTheEvidenceV1Puzzle();
		else if(puzzleId == 7)
			return new CrazyOperationV2Puzzle();

		return null;
	}

	private ExperimentResultData GetExperimentResultData(short subjectID,
			uint experimentStartTime, uint experimentEndTime,
			sbyte experimentLevel, ushort hitsTaken)
	{
		ExperimentResultData erd = new ExperimentResultData();
		erd.subjectID = subjectID;
		erd.experimentTime = (long) (experimentEndTime - experimentStartTime);
		erd.puzzleSolved = (byte) (experimentLevel + 1);
		erd.allPuzzlesSolved = experimentLevel >= 12;
		erd.hitsTaken = hitsTaken;
		erd.CalculateScore();
		return erd;
	}

	private short GetComputerKey(int roomId, int objectId)
	{
		return (byte) ((roomId * 100) + objectId);
	}

	// CHECK: Unused method.
	private bool IsExperimentPuzzleComputer(short computerKey)
	{
		return computerKey % 100 < 5;
	}

	private void Initialize()
	{
		rng = new RandomNumberGenerator();
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public Dictionary<int, Node> PuzzleComputerMap
	{
		set
		{
			puzzleComputerMap = value;
		}
	}

	public Dictionary<int, Node> InformationComputerMap
	{
		set
		{
			informationComputerMap = value;
		}
	}

	public Dictionary<int, Node> ExperimentResultComputerMap
	{
		set
		{
			experimentResultComputerMap = value;
		}
	}

	
	private const byte PUZZLE_AMOUNT = 8;
	
	private RandomNumberGenerator rng;

	private Dictionary<int, Node> puzzleComputerMap;
	private Dictionary<int, Node> informationComputerMap;
	private Dictionary<int, Node> experimentResultComputerMap;
}
