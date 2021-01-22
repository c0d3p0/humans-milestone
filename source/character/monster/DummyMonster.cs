using Godot;


public class DummyMonster : Node
{
	public override void _EnterTree()
	{
		animationPlayer = GetNode<AnimationPlayer>(animationPlayerNP);
	}

	public override void _Ready()
	{
		animationPlayer.Play("move");
	}


	[Export]
	public NodePath animationPlayerNP;

	private AnimationPlayer animationPlayer;
}
