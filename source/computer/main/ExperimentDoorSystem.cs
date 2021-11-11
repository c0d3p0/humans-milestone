using SCG = System.Collections.Generic;

using Godot;
using Godot.Collections;


public class ExperimentDoorSystem : Node
{
	public void LockRandomDoors(sbyte experimentLevel)
	{
		int lockLevel = experimentLevel / 2;
		byte amount = (byte) Mathf.Min(lockLevel, 4);
		SCG.IEnumerator<SCG.KeyValuePair<int, Node>> it = doorMap.GetEnumerator();
		randomDoorList.Clear();

		while(it.MoveNext()) // Add all experiment doors.
		{
			if(IsExperimentDoor(it.Current.Key))
				randomDoorList.Add(it.Current.Key);
		}

		while(amount-- > 0) // Remove randomly the doors that will be locked
			randomDoorList.RemoveAt(this.RandiRange(rng, 0, randomDoorList.Count - 1));

		it = doorMap.GetEnumerator();

		while(it.MoveNext()) // Execute system command in all experiment doors.
		{
			if(IsExperimentDoor(it.Current.Key))
			{
				it.Current.Value.EmitSignal(SignalKey.EXECUTE_SYSTEM_COMMAND,
						randomDoorList.Contains(it.Current.Key));
			}
		}

		if(lockLevel > 0)	
			experimentUpdateTimer.Start();
	}

	public void SetDoorsUnlocked(bool unlocked, bool experimentDoor = true)
	{
		SCG.KeyValuePair<int, Node> pair;
		SCG.IEnumerator<SCG.KeyValuePair<int, Node>> it =
				doorMap.GetEnumerator();

		while(it.MoveNext())
		{
			pair = it.Current;

			if(!experimentDoor || IsExperimentDoor(pair.Key))
				pair.Value.EmitSignal(SignalKey.EXECUTE_SYSTEM_COMMAND, unlocked);
		}
	}

	public void SetDoorUnlocked(byte roomId, byte doorId, bool unlocked)
	{
		Node d;

		if(doorMap.TryGetValue(GetDoorKey(roomId, doorId), out d))
			d.EmitSignal(SignalKey.EXECUTE_SYSTEM_COMMAND, unlocked);
	}

	public void AddDoor(Node door, byte roomId, byte doorId)
	{
		doorMap.Add(GetDoorKey(roomId, doorId), door);
	}

	private short GetDoorKey(byte roomId, byte objectId)
	{
		return (short) ((roomId * 100) + objectId);
	}

	private bool IsExperimentDoor(int doorKey)
	{
		return doorKey % 100 < 5;
	}

	private void Initialize()
	{
		rng = new RandomNumberGenerator();
		experimentUpdateTimer = GetNode<Timer>(experimentUpdateTimerNP);
		randomDoorList = new HashList<int>();
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public Dictionary<int, Node> DoorMap
	{
		set
		{
			doorMap = value;
		}
	}


	[Export]
	public NodePath experimentUpdateTimerNP;


	private Timer experimentUpdateTimer;

	private RandomNumberGenerator rng;
	private Dictionary<int, Node> doorMap;
	private HashList<int> randomDoorList;
}
