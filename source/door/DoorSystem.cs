using System.Collections.Generic;

using Godot;

public class DoorSystem : Node
{
	public void ExecuteHumanCommand(bool unlock)
	{
		if(NotSystemLocked)
			ChangeDoorState(unlock ? HUMAN_UNLOCKED : HUMAN_LOCKED);
	}

	public void ExecuteSystemCommand(bool unlock)
	{
		ChangeDoorState(unlock ? SYSTEM_UNLOCKED : SYSTEM_LOCKED);
	}

	private void ChangeDoorState(byte desiredDoorState)
	{
		bool desiredUnlocked = IsUnlockedState(desiredDoorState);

		if(Unlocked != desiredUnlocked)
		{
			doorState = desiredDoorState;
			EmitSignal(desiredUnlocked ? SignalKey.ON_DOOR_UNLOCKED :
					SignalKey.ON_DOOR_LOCKED);
		}
	}

	private void HandleMove(float direction, float limit, float delta)
	{
		if(staticBody.Translation.y != limit)
		{
			float limitDistance = Mathf.Abs(limit - staticBody.Translation.y);
			float vel = limitDistance >= moveSpeed ? moveSpeed : limitDistance;
			staticBody.Translate(new Vector3(0, vel * direction, 0));
			EmitSignal(SignalKey.ON_DOOR_MOVE);
		}
	}

	private void HandleDoorSensor(float delta)
	{
		if(Unlocked && sensorDetectionSet.Count > 0)
			HandleOpenMove(delta);
		else
			HandleCloseMove(delta);
	}

	private void HandleOpenMove(float delta)
	{
		if(staticBody.Translation.y > openLimitY)
			HandleMove(-1, openLimitY, delta);
	}

	private void HandleCloseMove(float delta)
	{
		if(staticBody.Translation.y < closeLimitY)
			HandleMove(1, closeLimitY, delta);
	}

	public void OnAreaEntered(Area area)
	{
		sensorDetectionSet.Add(area.GetInstanceId());
	}

	public void OnAreaExited(Area area)
	{
		sensorDetectionSet.Remove(area.GetInstanceId());
	}

	private bool IsUnlockedState(byte doorStateToCheck)
	{
		return doorStateToCheck % 2 == 1;
	}

	private void Initialize()
	{
		sensorDetectionSet = new HashSet<ulong>();
		staticBody = GetNode<StaticBody>(staticBodyNP);
	}

	public override void _PhysicsProcess(float physicsDelta)
	{
		HandleDoorSensor(physicsDelta);
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public override void _Ready()
	{
		ExecuteHumanCommand(true);
	}

	public bool Unlocked
	{
		get
		{	
			return doorState % 2 == 1;
		}
	}
	
	private bool NotSystemLocked
	{
		get
		{	
			return doorState < 4;
		}
	}


	[Export]
	public NodePath staticBodyNP;

	[Export]
	public float moveSpeed = 0.1f;

	[Export]
	public float openLimitY = -2f;

	[Export]
	public float closeLimitY = 0f;


	private HashSet<ulong> sensorDetectionSet;
	private byte doorState;

	private StaticBody staticBody;

	private const byte HUMAN_UNLOCKED = 1;
	private const byte HUMAN_LOCKED = 2;
	private const byte SYSTEM_UNLOCKED = 3;
	private const byte SYSTEM_LOCKED = 4;
}
