using Godot;
using Godot.Collections;


public class MonsterSensorSystem : Node
{
	private void UnlockDoor()
	{
		if(sensorMap.Count > 0)
		{
			door.EmitSignal(SignalKey.EXECUTE_SYSTEM_COMMAND, true);
			EmitSignal(SignalKey.ON_CHEATING_CONSEQUENCES);
		}
	}

	public void CheckMonsterLocked()
	{
		if(sensorMap.Count > 0 && !this.EmitSignal<bool>(door, SignalKey.IS_UNLOCKED))
		{
			if(unlockTimer.IsStopped())
			{
				unlockTimer.Start();
				EmitSignal(SignalKey.ON_CHEATING_ATTEMPT);
				GD.PushWarning("Cheating Attempt");
			}
		}
		else
			unlockTimer.Stop();
	}
			

	public void OnAreaEntered(Area area)
	{
		if(area.IsInGroup("monster"))
			sensorMap.Add(area.GetInstanceId(), area);
	}

	public void OnAreaExited(Area area)
	{
		if(area.IsInGroup("monster"))
			sensorMap.Remove(area.GetInstanceId());
	}

	private void Initialize()
	{
		unlockTimer = GetNode<Timer>(unlockTimerNP);
		sensorMap = new Dictionary<ulong, Node>();
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public override void _Process(float delta)
	{
		CheckMonsterLocked();
	}

	public Spatial Door
	{
		set
		{
			door = value;
		}
	}


	[Export]
	public NodePath unlockTimerNP;


	private Spatial door;
	private Timer unlockTimer;
	private Dictionary<ulong, Node> sensorMap;
}
