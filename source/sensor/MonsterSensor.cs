using Godot;


public class MonsterSensor : Node
{
	private void Initialize()
	{
		door = GetNode<Spatial>(doorNP);
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public Spatial Door
	{
		get
		{
			return door;
		}
	}

	[Export]
	public NodePath doorNP;
	

	private Spatial door;
}
