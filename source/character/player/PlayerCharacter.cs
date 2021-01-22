using Godot;


public class PlayerCharacter : KinematicBody
{
	public void GetDirection(Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET, playerInputInterpreter.Direction);
	}

	public void ShouldWalk(Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET, playerInputInterpreter.ShouldWalk);
	}

	public void ShouldInteract(Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET, playerInputInterpreter.ShouldInteract);
	}

	public void CannotMove(Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET, playerMainAction.CannotMove);
	}

	public void SetCharacterMoveEnabled(bool enabled)
	{
		characterMove.enabled = enabled;
	}

	private void Initialize()
	{
		deadScreen = GetNode(deadScreenNP);
		playerMainAction = GetNode<PlayerMainAction>(playerMainActionNP);
		characterMove = GetNode<CharacterMove>(characterMoveNP);
		playerInputInterpreter = GetNode<PlayerInputInterpreter>(
				playerInputInterpreterNP);
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public Node DeadScreen
	{
		get
		{
			return deadScreen;
		}
	}


	[Export]
	public NodePath deadScreenNP;

	[Export]
	public NodePath playerInputInterpreterNP;

	[Export]
	public NodePath playerMainActionNP;
	
	[Export]
	public NodePath characterMoveNP;

	[Export]
	public float runSpeed = 4.8f;

	[Export]
	public float walkSpeed = 1.3f;

	[Export]    
	public float acceleration = 8f;

	[Export]
	public float deacceleration = 18f;

	[Export]
	public float groundSurfacePositionY = 0f;

	[Export]
	public float lookSpeed = 4.5f;

	[Export]
	public float lookUpMaxAngle = 80f;

	[Export]
	public float lookDownMaxAngle = -80f;


	private Node deadScreen;
	private PlayerInputInterpreter playerInputInterpreter;
	private PlayerMainAction playerMainAction;
	private CharacterMove characterMove;
}
