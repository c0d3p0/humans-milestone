using Godot;


public class DoorLockComputerInitializer : Node
{
	private void Initialize()
	{
		doorLockComputer = GetNode<DoorLockComputer>(doorLockComputerNP);
		computerTouchScreen = GetNode(computerTouchScreenNP);
		animationPlayer = GetNode(animationPlayerNP);
	}

	private void  InitializeComputerTouchScreen()
	{
		computerTouchScreen.AddUserSignal(SignalKey.GET_BUTTON_BY_TOUCH);
		computerTouchScreen.AddUserSignal(SignalKey.ON_BUTTON_TOUCHED);
		
		computerTouchScreen.Connect(SignalKey.GET_BUTTON_BY_TOUCH,
				doorLockComputer, SignalMethod.GET_BUTTON_BY_TOUCH);
		computerTouchScreen.Connect(SignalKey.ON_BUTTON_TOUCHED, animationPlayer,
				SignalMethod.GD_STOP, this.CreateSingleBind(true));
		computerTouchScreen.Connect(SignalKey.ON_BUTTON_TOUCHED, animationPlayer,
				SignalMethod.GD_PLAY, this.CreateSingleBind("button_touch"));
	}

	private void InitializeTouchArea()
	{
		Node lockArea = GetNode(lockAreaNP);
		lockArea.AddUserSignal(SignalKey.ON_INTERACTION_RECEIVED);
		lockArea.Connect(SignalKey.ON_INTERACTION_RECEIVED,
				computerTouchScreen, SignalMethod.ON_INTERACTION_RECEIVED);
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeComputerTouchScreen();
		InitializeTouchArea();
	}
	

	[Export]
	public NodePath doorLockComputerNP;
	
	[Export]
	public NodePath computerTouchScreenNP;

	[Export]
	public NodePath lockAreaNP;

	[Export]
	public NodePath animationPlayerNP;


	private DoorLockComputer doorLockComputer;
	private Node computerTouchScreen;
	private Node animationPlayer;
}
