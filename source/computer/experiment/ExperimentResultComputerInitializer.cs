using Godot;


public class ExperimentResultComputerInitializer : Node
{
	private void Initialize()
	{
		computerTouchScreen = GetNode(computerTouchScreenNP);
		experimentResultComputer = GetNode(experimentResultComputerNP);
		experimentResultSystem = GetNode(experimentResultSystemNP);
		experimentResultSystemGUI = GetNode(experimentResultSystemGUINP);
		animationPlayer = GetNode(animationPlayerNP);
		mainComputer = GetTree().Root.GetNode("MainComputer");
	}

	private void InitializeExperimentResultComputer()
	{
		experimentResultComputer.AddUserSignal(SignalKey.UPDATE_EXPERIMENT_DATA_PAGE);
		experimentResultComputer.AddUserSignal(
				SignalKey.ADD_EXPERIMENT_RESULT_COMPUTER);

		experimentResultComputer.Connect(SignalKey.UPDATE_EXPERIMENT_DATA_PAGE,
				experimentResultComputer, SignalMethod.UPDATE_EXPERIMENT_DATA_PAGE);
		experimentResultComputer.Connect(SignalKey.ADD_EXPERIMENT_RESULT_COMPUTER,
				mainComputer, SignalMethod.ADD_EXPERIMENT_RESULT_COMPUTER);
	}

	private void InitializeComputerTouchScreen()
	{
		computerTouchScreen.AddUserSignal(SignalKey.GET_BUTTON_BY_TOUCH);
		computerTouchScreen.AddUserSignal(SignalKey.ON_BUTTON_TOUCHED);

		computerTouchScreen.Connect(SignalKey.GET_BUTTON_BY_TOUCH,
				experimentResultComputer, SignalMethod.GET_BUTTON_BY_TOUCH);
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

	private void InitializeExperimentResultSystem()
	{
		experimentResultSystem.AddUserSignal(SignalKey.UPDATE_GUI_STATE);
		experimentResultSystem.AddUserSignal(SignalKey.GET_LABEL);
		experimentResultSystem.AddUserSignal(SignalKey.GET_BUTTON);
		experimentResultSystem.AddUserSignal(SignalKey.ON_CLAIM_AWARD);

		experimentResultSystem.Connect(SignalKey.UPDATE_GUI_STATE,
				experimentResultSystemGUI, SignalMethod.UPDATE_GUI_STATE);
		experimentResultSystem.Connect(SignalKey.GET_LABEL,
				experimentResultComputer, SignalMethod.GET_LABEL);
		experimentResultSystem.Connect(SignalKey.GET_BUTTON,
				experimentResultComputer, SignalMethod.GET_BUTTON);
		experimentResultSystem.Connect(SignalKey.ON_CLAIM_AWARD,
				mainComputer, SignalMethod.ON_CLAIM_AWARD);
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeExperimentResultComputer();
		InitializeComputerTouchScreen();
		InitializeTouchArea();
		InitializeExperimentResultSystem();
	}


	[Export]
	public NodePath experimentResultComputerNP;

	[Export]
	public NodePath computerTouchScreenNP;

	[Export]
	public NodePath touchAreaNP;

	[Export]
	public NodePath experimentResultSystemNP;

	[Export]
	public NodePath experimentResultSystemGUINP;

	[Export]
	public NodePath animationPlayerNP;
	

	private Node mainComputer;
	private Node experimentResultComputer;
	private Node computerTouchScreen;
	private Node experimentResultSystem;
	private Node experimentResultSystemGUI;
	private Node animationPlayer;
}
