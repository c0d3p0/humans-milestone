using Godot;


public class PlayerCharacterInitializer : Node
{
	private void Initialize()
	{
		playerCharacter = GetNode<PlayerCharacter>(playerCharacterNP);
		characterMove = GetNode<CharacterMove>(characterMoveNP);
		characterLook = GetNode<CharacterLook>(characterLookNP);
		playerInteraction = GetNode(playerInteractionNP);
		playerMainAction = GetNode<PlayerMainAction>(playerMainActionNP);
		playerArea = GetNode(playerAreaNP);
		aimAnimationPlayer = GetNode<AnimationPlayer>(aimAnimationPlayerNP);
		animationNSMP = GetNode<AnimationTree>(animationTreeNP).Get(
				"parameters/playback") as AnimationNodeStateMachinePlayback;
		playerInputInterpreter = GetNode<PlayerInputInterpreter>(
				playerInputInterpreterNP);
	}

	private void InitializePlayerCharacter()
	{
		playerCharacter.AddUserSignal(SignalKey.UPDATE_ACTIVE);
		playerCharacter.AddUserSignal(SignalKey.SHOW_DEAD_SCREEN);
		
		playerCharacter.Connect(SignalKey.UPDATE_ACTIVE, playerMainAction,
				SignalMethod.UPDATE_ACTIVE);
		playerCharacter.Connect(SignalKey.SHOW_DEAD_SCREEN,
				playerCharacter.DeadScreen, SignalMethod.SHOW_DEAD_SCREEN);
	}
	
	private void InitializePlayerMainAction()
	{
		playerMainAction.AddUserSignal(SignalKey.SET_CHARACTER_MOVE_ENABLED);
		playerMainAction.AddUserSignal(SignalKey.ON_CHARACTER_ACTIVE);
		playerMainAction.AddUserSignal(SignalKey.ON_PANICKED);
		playerMainAction.AddUserSignal(SignalKey.ON_CHARACTER_DEATH);

		playerMainAction.Connect(SignalKey.SET_CHARACTER_MOVE_ENABLED,
				playerCharacter, SignalMethod.SET_CHARACTER_MOVE_ENABLED);
		playerMainAction.Connect(SignalKey.ON_CHARACTER_ACTIVE, animationNSMP,
				SignalMethod.GD_TRAVEL, this.CreateSingleBind("active"));
		playerMainAction.Connect(SignalKey.ON_PANICKED, animationNSMP,
				SignalMethod.GD_TRAVEL, this.CreateSingleBind("panic"));
		playerMainAction.Connect(SignalKey.ON_CHARACTER_DEATH, animationNSMP,
				SignalMethod.GD_TRAVEL, this.CreateSingleBind("death"));
	}

	private void InitializePlayerInputInterpreter()
	{
		playerInputInterpreter.AddUserSignal(SignalKey.EXECUTE_LOOK);
		playerInputInterpreter.AddUserSignal(SignalKey.CANNOT_MOVE);

		playerInputInterpreter.Connect(SignalKey.EXECUTE_LOOK,
				characterLook, SignalMethod.EXECUTE_LOOK);
		playerInputInterpreter.Connect(SignalKey.CANNOT_MOVE,
				playerCharacter, SignalMethod.CANNOT_MOVE);
	}

	private void InitializeCharacterMove()
	{
		characterMove.RunSpeed = playerCharacter.runSpeed;
		characterMove.WalkSpeed = playerCharacter.walkSpeed;
		characterMove.Acceleration = playerCharacter.acceleration;
		characterMove.Deacceleration = playerCharacter.deacceleration;
		characterMove.GroundSurfacePositionY = playerCharacter.groundSurfacePositionY;

		characterMove.AddUserSignal(SignalKey.GET_DIRECTION);
		characterMove.AddUserSignal(SignalKey.SHOULD_WALK);
		characterMove.AddUserSignal(SignalKey.ON_RUNNING);
		characterMove.AddUserSignal(SignalKey.ON_WALKING);
		characterMove.AddUserSignal(SignalKey.ON_IDLING);

		characterMove.Connect(SignalKey.GET_DIRECTION,
				playerCharacter, SignalMethod.GET_DIRECTION);
		characterMove.Connect(SignalKey.SHOULD_WALK,
				playerCharacter, SignalMethod.SHOULD_WALK);
		characterMove.Connect(SignalKey.ON_RUNNING, animationNSMP,
				SignalMethod.GD_TRAVEL, this.CreateSingleBind("run"));
		characterMove.Connect(SignalKey.ON_WALKING, animationNSMP,
				SignalMethod.GD_TRAVEL, this.CreateSingleBind("walk"));
		characterMove.Connect(SignalKey.ON_IDLING, animationNSMP,
				SignalMethod.GD_TRAVEL, this.CreateSingleBind("idle"));
	}

	private void InitializeCharacterLook()
	{
		characterLook.LookSpeed = playerCharacter.lookSpeed;
		characterLook.LookUpMaxAngle = playerCharacter.lookUpMaxAngle;
		characterLook.LookDownMaxAngle = playerCharacter.lookDownMaxAngle;
	}

	private void InitializePlayerInteraction()
	{
		playerInteraction.AddUserSignal(SignalKey.SHOULD_INTERACT);
		playerInteraction.AddUserSignal(SignalKey.ON_INTERACTING);

		playerInteraction.Connect(SignalKey.SHOULD_INTERACT,
				playerCharacter, SignalMethod.SHOULD_INTERACT);
		playerInteraction.Connect(SignalKey.ON_INTERACTING, aimAnimationPlayer,
				SignalMethod.GD_PLAY, this.CreateSingleBind("interact"));
	}

	private void InitializePlayerArea()
	{
		playerArea.AddUserSignal(SignalKey.ON_HIT);
		playerArea.AddUserSignal(SignalKey.ON_CAUGHT);

		playerArea.Connect(SignalKey.ON_HIT, playerMainAction, SignalMethod.DIE);
		playerArea.Connect(SignalKey.ON_CAUGHT,
				playerMainAction, SignalMethod.PANIC);
	}
		
	public override void _EnterTree()
	{
		Initialize();
		InitializePlayerCharacter();
		InitializePlayerMainAction();
		InitializePlayerInputInterpreter();
		InitializeCharacterMove();
		InitializeCharacterLook();
		InitializePlayerInteraction();
		InitializePlayerArea();
	}
	

	[Export]
	public NodePath playerCharacterNP;

	[Export]
	public NodePath playerMainActionNP;

	[Export]
	public NodePath playerInputInterpreterNP;

	[Export]
	public NodePath characterMoveNP;

	[Export]
	public NodePath characterLookNP;

	[Export]
	public NodePath playerInteractionNP;

	[Export]
	public NodePath playerAreaNP;

	[Export]
	public NodePath animationTreeNP;

	[Export]
	public NodePath aimAnimationPlayerNP;


	private PlayerCharacter playerCharacter;
	private PlayerMainAction playerMainAction;
	private PlayerInputInterpreter playerInputInterpreter;
	private CharacterMove characterMove;
	private CharacterLook characterLook;
	private Node playerInteraction;
	private Node playerArea;
	private AnimationNodeStateMachinePlayback animationNSMP;
	private AnimationPlayer aimAnimationPlayer;
}
