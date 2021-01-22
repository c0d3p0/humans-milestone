using Godot;


public class AnimationStateMachinePlayer : Node
{
	private void Initialize()
	{
		animationNSMP = GetNode<AnimationTree>(animationTreeNP).Get(
				"parameters/playback") as AnimationNodeStateMachinePlayback;
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public void TravelAndPlay(string animation)
	{
		if(animationNSMP.GetCurrentNode().Equals(animation))
			animationNSMP.Start(animation);
		else
			animationNSMP.Travel(animation);
	}


	[Export]
	public NodePath animationTreeNP;


	private AnimationNodeStateMachinePlayback animationNSMP;
}
