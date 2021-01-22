using Godot;


public class DeadScreen : Control
{
	public void ShowDeadScreen()
	{
		deadScreenManager.ShowDeadScreen();
	}

	private void Initialize()
	{
		experimentManager = GetNode(experimentManagerNP);
		deadScreenManager = GetNode<DeadScreenManager>(deadScreenManagerNP);
	}

	public override void _EnterTree()
	{
		Initialize();
	}


	public Node ExperimentManager
	{
		get
		{
			return experimentManager;
		}
	}


	[Export]
	public NodePath experimentManagerNP;

	[Export]
	public NodePath deadScreenManagerNP;


	private Node experimentManager;
	private DeadScreenManager deadScreenManager;
}
