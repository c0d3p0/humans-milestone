using Godot;


public class MonsterCharacterDebug : Node
{
	private void HandleActivateMonster(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.Kp7)
		{
			monsterMainAction.UpdateActive(true);
			GD.PushWarning("Debug, MonsterCharacter activated!");
		}
	}

	private void HandleDeactivateMonster(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.Kp8)
		{
			monsterMainAction.UpdateActive(false);
			GD.PushWarning("Debug, MonsterCharacter deactivated!");
		}
	}

	private void HandleForceMonsterAttack(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.Kp9)
		{
			monsterMainAction.Attack(body);
			GD.PushWarning("Debug, MonsterCharacter forced to attack!");
		}
	}

	private void HandleActivateKillerDevice(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.KpAdd)
		{
			monsterMainAction.ActivateKillerDevice();
			GD.PushWarning("Debug, MonsterCharacter killer device activated!");
		}
	}

	private void HandleDebug(InputEventKey inputEventKey)
	{
		if(inputEventKey != null && inputEventKey.Pressed)
		{
			uint keyScancode = inputEventKey.Scancode;
			HandleActivateMonster(keyScancode);
			HandleDeactivateMonster(keyScancode);
			HandleForceMonsterAttack(keyScancode);
			HandleActivateKillerDevice(keyScancode);
		}
	}

	private void Initialize()
	{
		monsterMainAction = GetNode<MonsterMainAction>(monsterMainActionNP);
		body = GetNode<Spatial>(bodyNP);
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
	public NodePath bodyNP;

	[Export]
	public NodePath monsterMainActionNP;
	

	private MonsterMainAction monsterMainAction;
	private Spatial body;
}
