using Godot;


public class PuzzleComputerInitializer : Node
{
	private void Initialize()
	{
		puzzleSystemGUI = GetNode(puzzleSystemGUINP);
		puzzleComputer = GetNode<PuzzleComputer>(puzzleComputerNP);
		computerTouchScreen = GetNode(computerTouchScreenNP);
		puzzleSystem = GetNode(puzzleSystemNP);
		animationPlayer = GetNode(animationPlayerNP);
		mainComputer = GetTree().Root.GetNode("MainComputer");
	}

	private void InitializePuzzleComputer()
	{
		puzzleComputer.AddUserSignal(SignalKey.SET_PUZZLE);
		puzzleComputer.AddUserSignal(SignalKey.IS_PUZZLE_SOLVED);
		puzzleComputer.AddUserSignal(SignalKey.SET_SYSTEM_ACTIVE);
		puzzleComputer.AddUserSignal(SignalKey.ADD_PUZZLE_COMPUTER);

		puzzleComputer.Connect(SignalKey.SET_PUZZLE,
				puzzleComputer, SignalMethod.SET_PUZZLE);
		puzzleComputer.Connect(SignalKey.IS_PUZZLE_SOLVED,
				puzzleComputer, SignalMethod.IS_PUZZLE_SOLVED);
		puzzleComputer.Connect(SignalKey.SET_SYSTEM_ACTIVE,
				puzzleComputer, SignalMethod.SET_SYSTEM_ACTIVE);
		puzzleComputer.Connect(SignalKey.ADD_PUZZLE_COMPUTER,
				mainComputer, SignalMethod.ADD_PUZZLE_COMPUTER);
	}

	private void  InitializeComputerTouchScreen()
	{
		computerTouchScreen.AddUserSignal(SignalKey.GET_BUTTON_BY_TOUCH);
		computerTouchScreen.AddUserSignal(SignalKey.ON_BUTTON_TOUCHED);

		computerTouchScreen.Connect(SignalKey.GET_BUTTON_BY_TOUCH,
				puzzleComputer, SignalMethod.GET_BUTTON_BY_TOUCH);
		computerTouchScreen.Connect(SignalKey.ON_BUTTON_TOUCHED, animationPlayer,
				SignalMethod.GD_STOP, this.CreateSingleBind(true));
		computerTouchScreen.Connect(SignalKey.ON_BUTTON_TOUCHED, animationPlayer,
				SignalMethod.GD_PLAY, this.CreateSingleBind("button_touch"));
	}

	private void InitializeTouchArea()
	{
		Node ta = GetNode(touchAreaNP);
		Node ts, area;
		
		for(int i = 0; i < ta.GetChildCount() - 1; i++)
		{	
			ts = ta.GetChild(i);

			for(int j = 0; j < ts.GetChildCount(); j++)
			{
				area = ts.GetChild(j);
				area.AddUserSignal(SignalKey.ON_INTERACTION_RECEIVED);
				area.Connect(SignalKey.ON_INTERACTION_RECEIVED,
						computerTouchScreen, SignalMethod.ON_INTERACTION_RECEIVED);
			}
		}
	}
	
	private void InitializePuzzleSystem()
	{
		puzzleSystem.AddUserSignal(SignalKey.UPDATE_GUI_STATE);
		puzzleSystem.AddUserSignal(SignalKey.GET_BUTTON);
		puzzleSystem.AddUserSignal(SignalKey.GET_LABEL);
		puzzleSystem.AddUserSignal(SignalKey.GET_TEXTURE_RECT);
		puzzleSystem.AddUserSignal(SignalKey.ON_SOLVING_PUZZLE);

		puzzleSystem.Connect(SignalKey.UPDATE_GUI_STATE,
				puzzleSystemGUI, SignalMethod.UPDATE_GUI_STATE);
		puzzleSystem.Connect(SignalKey.GET_BUTTON, puzzleComputer,
				SignalMethod.GET_BUTTON);
		puzzleSystem.Connect(SignalKey.GET_LABEL, puzzleComputer,
				SignalMethod.GET_LABEL);
		puzzleSystem.Connect(SignalKey.GET_TEXTURE_RECT,
				puzzleComputer, SignalMethod.GET_TEXTURE_RECT);
		puzzleSystem.Connect(SignalKey.ON_SOLVING_PUZZLE,
				mainComputer, SignalMethod.ON_SOLVING_PUZZLE);
	}
	
	public override void _EnterTree()
	{
		Initialize();
		InitializePuzzleComputer();
		InitializeComputerTouchScreen();
		InitializeTouchArea();
		InitializePuzzleSystem();
	}


	[Export]
	public NodePath puzzleComputerNP;

	[Export]
	public NodePath computerTouchScreenNP;

	[Export]
	public NodePath touchAreaNP;

	[Export]
	public NodePath puzzleSystemNP;

	[Export]
	public NodePath puzzleSystemGUINP;

	[Export]
	public NodePath animationPlayerNP;


	private Node mainComputer;
	private PuzzleComputer puzzleComputer;
	private Node computerTouchScreen;
	private Node puzzleSystem;
	private Node puzzleSystemGUI;
	private Node animationPlayer;
}
