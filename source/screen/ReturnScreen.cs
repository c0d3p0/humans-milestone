using Godot;


public class ReturnScreen : Node
{
	private void HandleKeyboardInput(InputEventKey inputEventKey)
	{
		if(inputEventKey != null && inputEventKey.Pressed)
		{
			if(inputEventKey.Scancode == (uint) KeyList.Escape)
				HandleQuitGame();
		}
	}

	private void OnAnimationFinished(string animationName)
	{
		quitActive = false;
	}

	private void HandleQuitGame()
	{
		if(quitActive)
		{
			globalValue.EmitSignal(SignalKey.PUT, "sceneToLoad", titleScreenPath);
			GetTree().ChangeScene(loadScreenPath);
		}
		else
			animationPlayer.Play("fade");
	}

	private void Initialize()
	{
		globalValue = GetTree().Root.GetNode("GlobalValue");
		animationPlayer = GetNode<AnimationPlayer>(animationPlayerNP);
		quitActive = false;
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public override void _Input(InputEvent inputEvent)
	{
		HandleKeyboardInput(inputEvent as InputEventKey);
	}

	
	[Export]
	public NodePath animationPlayerNP;

	[Export]
	public string titleScreenPath;

	[Export]
	public string loadScreenPath;

	[Export]
	public bool quitActive;


	private Node globalValue;
	private AnimationPlayer animationPlayer;
}
