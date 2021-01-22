using Godot;


public class DeadScreenManager : Node
{
	public void ShowDeadScreen()
	{
		if(!this.EmitSignal<bool>(this, SignalKey.IS_EXPERIMENT_FINISHED))
			animationStateMachine.Travel("death_fade");
		else
			animationStateMachine.Travel("death_end");
	}

	public void ContinueGame()
	{
		EmitSignal(SignalKey.RESTART_CHARACTERS);
	}

	public void GoToCreditScreen()
	{
		SceneTree sceneTree = GetTree();
		RemoveMainComputer(sceneTree.Root);
		sceneTree.ChangeScene(creditsScreenPath);
	}

	private void RemoveMainComputer(Node sceneTreeRoot)
	{
		Node mainComputer = sceneTreeRoot.GetNodeOrNull("MainComputer");

		if(mainComputer != null)
		{
			sceneTreeRoot.CallDeferred("remove_child", mainComputer);
			mainComputer.CallDeferred("queue_free");
		}
	}

	private void Initialize()
	{
		animationStateMachine = GetNode<AnimationTree>(animationTreeNP).Get(
				"parameters/playback") as AnimationNodeStateMachinePlayback;
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public override void _Ready()
	{
		animationStateMachine.Travel("fade_in");
	}


	[Export]
	public NodePath animationTreeNP;

	[Export]
	public string creditsScreenPath = "res://scene/credits_screen.tscn";

	// [Export]
	// public PackedScene creditsScreenPS;

	

	private AnimationNodeStateMachinePlayback animationStateMachine;
}
