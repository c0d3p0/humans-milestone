using Godot;
using Godot.Collections;


public class MonsterCharacter : KinematicBody
{
	public void SetProcessBehavior(bool active)
	{
		monsterAI.SetProcess(active);
	}

	public void GetDirection(Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET, monsterAI.Direction);
	}

	public void ShouldWalk(Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET, monsterMainAction.ShouldWalk);
	}

	public void CannotMove(Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET, monsterMainAction.CannotMove);
	}

	public void SetCharacterMoveEnabled(bool enabled)
	{
		characterMove.enabled = enabled;
	}

	private void Initialize()
	{
		monsterMainAction = GetNode<MonsterMainAction>(monsterMainActionNP);
		monsterAI = GetNode<MonsterAI>(monsterAINP);
		navigation = GetNode<Navigation>(navigationNP);
		characterMove = GetNode<CharacterMove>(characterMoveNP);
	}

	private void InitializeSearchTargetList()
	{
		searchTargetList = new Array<Spatial>();
		Godot.Collections.Array c = GetNode<Spatial>(searchSpotNP).GetChildren();
		System.Collections.IEnumerator it = c.GetEnumerator();

		while(it.MoveNext())
			searchTargetList.Add((Spatial) it.Current);
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeSearchTargetList();
	}

	public Navigation Navigation
	{
		get
		{
			return navigation;	
		}	
	}

	public Array<Spatial> SearchTargetList
	{
		get
		{
			return searchTargetList;	
		}	
	}


	[Export]
	public NodePath monsterAINP;

	[Export]
	public NodePath monsterMainActionNP;

	[Export]
	public NodePath characterMoveNP;

	[Export]
	public float runSpeed = 4.5f;

	[Export]    
	public float acceleration = 8f;

	[Export]
	public float deacceleration = 18f;

	[Export]
	public float groundSurfacePositionY = 0f;

	[Export]
	public NodePath navigationNP;

	[Export]
	public NodePath searchSpotNP;

	[Export]
	public bool killerDeviceFaulty;
	

	private Navigation navigation;
	private Array<Spatial> searchTargetList;


	private MonsterAI monsterAI;
	private MonsterMainAction monsterMainAction;
	private CharacterMove characterMove;
}
