using Godot;


public class CreditsScreen : Node
{
	public void GoToTitleScreen()
	{
		GetTree().ChangeScene("res://scene/title_screen.tscn");
	}

	public void ObtainNodes()
	{
		animationStateMachine = GetNode<AnimationTree>(animationTreeNP).Get(
				"parameters/playback") as AnimationNodeStateMachinePlayback;
	}

	public override void _EnterTree()
	{
		ObtainNodes();
	}

	public override void _Ready()
	{
		animationStateMachine.Travel("game_title");
	}


	[Export]
	public NodePath animationTreeNP;

	
	private AnimationNodeStateMachinePlayback animationStateMachine;
}
