using Godot;
using Godot.Collections;


public class DoorLockSystemGUI : BaseSystemGUI
{
	public void OnButtonPressed()
	{
		if(door != null)
		{
			bool unlock = !this.EmitSignal<bool>(door, SignalKey.IS_UNLOCKED);
			door.EmitSignal(SignalKey.EXECUTE_HUMAN_COMMAND, unlock);
			UpdateUnlockButtonText(unlock);
		}
	}

	private void UpdateUnlockButtonText(bool unlocked, bool forceUpdate = false)
	{
		if(this.unlocked != unlocked || forceUpdate)
		{
			lockButton.Text = unlocked ? "Lock" : "Unlock";
			this.unlocked = unlocked;
		}
	}

	private void InitializeButtomMap()
	{
		lockButton = GetNode<Button>(lockButtonNP);
		buttonMap = new Dictionary<int, Button>();
		buttonMap.Add(SystemGUIID.BTN_KB_KEY_OK, lockButton);
	}

	private void InitializeTouchAreaMap()
	{
		Node lockArea = GetNode(lockAreaNP);
		touchAreaMap = new Dictionary<ulong, byte>();
		touchAreaMap.Add(lockArea.GetInstanceId(), SystemGUIID.BTN_KB_KEY_OK);
	}

	public override void _EnterTree()
	{
		InitializeButtomMap();
		InitializeTouchAreaMap();
	}

	public override void _Ready()
	{
		UpdateUnlockButtonText(this.EmitSignal<bool>(door,
				SignalKey.IS_UNLOCKED), true);
	}

	public override void _Process(float delta)
	{
		UpdateUnlockButtonText(this.EmitSignal<bool>(door,
				SignalKey.IS_UNLOCKED));
	}

	public Node Door
	{
		set
		{
			door = value;
		}
	}


	[Export]
	public NodePath lockButtonNP;

	[Export]
	public NodePath lockAreaNP;


	private Button lockButton;
	private Node door;

	private bool unlocked;
}
