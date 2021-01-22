using Godot;


public class InformationComputerInitializer : Node
{
	private void Initialize()
	{
		computerTouchScreen = GetNode(computerTouchScreenNP);
		informationComputer = GetNode<InformationComputer>(informationComputerNP);
		informationSystem = GetNode(informationSystemNP);
		animationPlayer = GetNode(animationPlayerNP);
		mainComputer = GetTree().Root.GetNode("MainComputer");
	}

	private void InitializeInformationComputer()
	{
		informationComputer.AddUserSignal(SignalKey.ADD_INFORMATION_COMPUTER);
		informationComputer.AddUserSignal(SignalKey.UPDATE_INFORMATION_PAGE);

		informationComputer.Connect(SignalKey.ADD_INFORMATION_COMPUTER,
				mainComputer, SignalMethod.ADD_INFORMATION_COMPUTER);
		informationComputer.Connect(SignalKey.UPDATE_INFORMATION_PAGE,
				informationComputer, SignalMethod.UPDATE_INFORMATION_PAGE);
	}

	private void InitializeComputerTouchScreen()
	{
		computerTouchScreen.AddUserSignal(SignalKey.GET_BUTTON_BY_TOUCH);
		computerTouchScreen.AddUserSignal(SignalKey.ON_BUTTON_TOUCHED);

		computerTouchScreen.Connect(SignalKey.GET_BUTTON_BY_TOUCH,
				informationComputer, SignalMethod.GET_BUTTON_BY_TOUCH);
		computerTouchScreen.Connect(SignalKey.ON_BUTTON_TOUCHED, animationPlayer,
				SignalMethod.GD_STOP, this.CreateSingleBind(true));
		computerTouchScreen.Connect(SignalKey.ON_BUTTON_TOUCHED, animationPlayer,
				SignalMethod.GD_PLAY, this.CreateSingleBind("button_touch"));
	}

	private void InitializeTouchArea()
	{
		Node ta = GetNode(touchAreaNP);
		Node area;

		for(int i = 0; i < ta.GetChildCount() - 1; i++)
		{	
			area = ta.GetChild(i);
			area.AddUserSignal(SignalKey.ON_INTERACTION_RECEIVED);
			area.Connect(SignalKey.ON_INTERACTION_RECEIVED,
					computerTouchScreen, SignalMethod.ON_INTERACTION_RECEIVED);
		}
	}

	private void InitializeInformationSystem()
	{
		informationSystem.AddUserSignal(SignalKey.GET_LABEL);
		informationSystem.AddUserSignal(SignalKey.GET_ROOM_INFORMATION);

		informationSystem.Connect(SignalKey.GET_LABEL,
				informationComputer, SignalMethod.GET_LABEL);
		informationSystem.Connect(SignalKey.GET_ROOM_INFORMATION,
				mainComputer, SignalMethod.GET_ROOM_INFORMATION);
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeInformationComputer();
		InitializeComputerTouchScreen();
		InitializeTouchArea();
		InitializeInformationSystem();
	}


	[Export]
	public NodePath informationComputerNP;

	[Export]
	public NodePath computerTouchScreenNP;

	[Export]
	public NodePath touchAreaNP;

	[Export]
	public NodePath informationSystemNP;

	[Export]
	public NodePath animationPlayerNP;


	private Node mainComputer;
	private InformationComputer informationComputer;
	private Node computerTouchScreen;
	private Node informationSystem;
	private Node animationPlayer;
}
