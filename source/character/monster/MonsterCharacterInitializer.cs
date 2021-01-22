using Godot;


public class MonsterCharacterInitializer : Node
{
	private void Initialize()
	{
		monsterCharacter = GetNode<MonsterCharacter>(monsterCharacterNP);
		characterMove = GetNode<CharacterMove>(characterMoveNP);
		monsterMainAction = GetNode<MonsterMainAction>(monsterMainActionNP);
		monsterAI = GetNode<MonsterAI>(monsterAINP);
		animationNSMP = GetNode<AnimationTree>(animationTreeNP).Get(
				"parameters/playback") as AnimationNodeStateMachinePlayback;
	}

	private void InitializeMonsterCharacter()
	{
		monsterCharacter.AddUserSignal(SignalKey.UPDATE_ACTIVE);
		monsterCharacter.AddUserSignal(SignalKey.ACTIVATE_KILLER_DEVICE);

		monsterCharacter.Connect(SignalKey.UPDATE_ACTIVE,
				monsterMainAction, SignalMethod.UPDATE_ACTIVE);
		monsterCharacter.Connect(SignalKey.ACTIVATE_KILLER_DEVICE,
				monsterMainAction, SignalMethod.ACTIVATE_KILLER_DEVICE);
	}
	
	private void InitializeMonsterMainAction()
	{
		monsterMainAction.KillerDeviceFaulty = monsterCharacter.killerDeviceFaulty;

		monsterMainAction.AddUserSignal(SignalKey.SET_CHARACTER_MOVE_ENABLED);
		monsterMainAction.AddUserSignal(SignalKey.CLEAR_TARGET_AND_ALL_PATHS);
		monsterMainAction.AddUserSignal(SignalKey.CREATE_PATH_TO_SEARCH_POINT);
		monsterMainAction.AddUserSignal(SignalKey.ON_CHARACTER_ACTIVE);
		monsterMainAction.AddUserSignal(SignalKey.ON_CHARACTER_INACTIVE);
		monsterMainAction.AddUserSignal(SignalKey.ON_CHARACTER_DEATH);
		monsterMainAction.AddUserSignal(SignalKey.ON_ATTACK);

		monsterMainAction.Connect(SignalKey.SET_CHARACTER_MOVE_ENABLED,
				monsterCharacter, SignalMethod.SET_CHARACTER_MOVE_ENABLED);
		monsterMainAction.Connect(SignalKey.CLEAR_TARGET_AND_ALL_PATHS,
				monsterAI, SignalMethod.CLEAR_TARGET_AND_ALL_PATHS);
		monsterMainAction.Connect(SignalKey.CREATE_PATH_TO_SEARCH_POINT,
				monsterAI, SignalMethod.CREATE_PATH_TO_SEARCH_POINT);
		monsterMainAction.Connect(SignalKey.ON_CHARACTER_ACTIVE, animationNSMP,
				SignalMethod.GD_TRAVEL, this.CreateSingleBind("monster_looking"));
		monsterMainAction.Connect(SignalKey.ON_CHARACTER_INACTIVE, animationNSMP,
				SignalMethod.GD_TRAVEL, this.CreateSingleBind("monster_idle"));
		monsterMainAction.Connect(SignalKey.ON_CHARACTER_DEATH, animationNSMP,
				SignalMethod.GD_TRAVEL, this.CreateSingleBind("monster_death"));
		monsterMainAction.Connect(SignalKey.ON_ATTACK, animationNSMP,
				SignalMethod.GD_TRAVEL, this.CreateSingleBind("monster_attack"));
	}

	private void InitializeMonsterAI()
	{
		monsterAI.SearchTargetList = monsterCharacter.SearchTargetList;
		monsterAI.Navigation = monsterCharacter.Navigation;

		monsterAI.AddUserSignal(SignalKey.CANNOT_MOVE);
		monsterAI.AddUserSignal(SignalKey.LOOK_AT);

		monsterAI.Connect(SignalKey.CANNOT_MOVE, monsterCharacter,
				SignalMethod.CANNOT_MOVE);
		monsterAI.Connect(SignalKey.LOOK_AT, monsterMainAction,
				SignalMethod.LOOK_AT);
	}

	private void InitializeCharacterMove()
	{
		characterMove.RunSpeed = monsterCharacter.runSpeed;
		characterMove.Acceleration = monsterCharacter.acceleration;
		characterMove.Deacceleration = monsterCharacter.deacceleration;
		characterMove.GroundSurfacePositionY = monsterCharacter.groundSurfacePositionY;

		characterMove.AddUserSignal(SignalKey.GET_DIRECTION);
		characterMove.AddUserSignal(SignalKey.SHOULD_WALK);
		characterMove.AddUserSignal(SignalKey.ON_RUNNING);
		characterMove.AddUserSignal(SignalKey.ON_IDLING);

		characterMove.Connect(SignalKey.GET_DIRECTION,
				monsterCharacter, SignalMethod.GET_DIRECTION);
		characterMove.Connect(SignalKey.SHOULD_WALK,
				monsterCharacter, SignalMethod.SHOULD_WALK);
		characterMove.Connect(SignalKey.ON_RUNNING, animationNSMP,
				SignalMethod.GD_TRAVEL, this.CreateSingleBind("monster_run"));
		characterMove.Connect(SignalKey.ON_IDLING, animationNSMP,
				SignalMethod.GD_TRAVEL, this.CreateSingleBind("monster_looking"));
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeMonsterCharacter();
		InitializeMonsterMainAction();
		InitializeMonsterAI();
		InitializeCharacterMove();
	}


	[Export]
	public NodePath monsterCharacterNP;

	[Export]
	public NodePath monsterMainActionNP;

	[Export]
	public NodePath monsterAINP;

	[Export]
	public NodePath characterMoveNP;

	[Export]
	public NodePath animationTreeNP;


	private MonsterCharacter monsterCharacter;
	private MonsterMainAction monsterMainAction;
	private MonsterAI monsterAI;
	private CharacterMove characterMove;
	private AnimationNodeStateMachinePlayback animationNSMP;
}
