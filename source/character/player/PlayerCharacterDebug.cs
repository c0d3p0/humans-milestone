using Godot;


public class PlayerCharacterDebug : Node
{
	private void HandleToggleInvincible(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.Kp0)
		{
			playerMainAction.Invincible = !playerMainAction.Invincible;
			GD.PushWarning("Debug, PlayerCharacter.Invincible: " +
					playerMainAction.Invincible);
		}
	}

	private void HandleDebug(InputEventKey inputEventKey)
	{
		if(inputEventKey != null && inputEventKey.Pressed)
			HandleToggleInvincible(inputEventKey.Scancode);
	}

	private void Initialize()
	{
		playerMainAction = GetNode<PlayerMainAction>(playerMainActionNP);
	}

	public override void _Input(InputEvent inputEvent)
	{
		HandleDebug(inputEvent as InputEventKey);
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public override void _Ready()
	{
		SetProcessInput(OS.IsDebugBuild());
	}


	[Export]
	public NodePath playerMainActionNP;


	private PlayerMainAction playerMainAction;
}
