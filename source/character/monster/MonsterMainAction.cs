using Godot;
using Godot.Collections;


public class MonsterMainAction : Node
{
	public void UpdateActive(bool active, Vector3 searchPosition)
	{
		UpdateActive(active);
		EmitSignal(SignalKey.CREATE_PATH_TO_SEARCH_POINT, searchPosition);
	}

	public void UpdateActive(bool active)
	{
		dead = false;
		SetActive(active);
		EmitSignal(SignalKey.CLEAR_TARGET_AND_ALL_PATHS);
		EmitSignal(active ? SignalKey.ON_CHARACTER_ACTIVE :
				SignalKey.ON_CHARACTER_INACTIVE);
	}

	public void OnPunchHit() // Called by attack animation
	{
		Node collider = rayCasts[1].GetCollider() as Node;

		if(collider != null && collider.IsInGroup("monster_prey"))
			collider.EmitSignal(SignalKey.ON_HIT);
		else
			SetActive(true);
	}

	public void Attack(Spatial collider)
	{
		if(active)
		{	
			SetActive(false);
			attacking = true;
			collider.EmitSignal(SignalKey.ON_CAUGHT, monsterCharacter);
			LookAt(collider.GlobalTransform.origin);
			EmitSignal(SignalKey.ON_ATTACK);
		}
	}

	public void LookAt(Vector3 position)
	{
		position.y = body.GlobalTransform.origin.y;
		body.LookAt(position, Vector3.Up);
	}

	public void ActivateKillerDevice()
	{
		if(!killerDeviceFaulty)
			Die();
	}

	private void HandleCatchPrey()
	{
		string preyGroup = "monster_prey";
		Spatial collider;

		for(int i = 0; i < rayCasts.Length; i++)
		{
			collider = rayCasts[i].GetCollider() as Spatial;

			if(collider != null)
			{
				if(active && collider.IsInGroup(preyGroup))
				{
					Attack(collider);
					break;
				}
				else if(attacking && IsDoorAndLocked(collider))
				{
					SetActive(true);
					break;
				}
			}
		}
	}

	private void Die()
	{
		dead = true;
		SetActive(false);
		EmitSignal(SignalKey.ON_CHARACTER_DEATH);
	}

	private bool IsDoorAndLocked(Spatial collider)
	{
		return collider.IsInGroup("door") && !this.EmitSignal<bool>(
				collider.GetParent(), SignalKey.IS_UNLOCKED);
	}

	private void SetActive(bool active)
	{
		this.active = active;
		attacking = false;
		EmitSignal(SignalKey.SET_CHARACTER_MOVE_ENABLED, active);
		monsterCharacter.Call(SignalMethod.GD_SET_PROCESS, active);
	}
	
	private void Initialize()
	{
		monsterCharacter = GetNode<Spatial>(monsterCharacterNP);
		body = GetNode<Spatial>(bodyNP);
	}

	private void InitializeRayCasts()
	{
		rayCasts = new RayCast[rayCastNPList.Count];

		for(int i = 0; i < rayCasts.Length; i++)
			rayCasts[i] = GetNode<RayCast>(rayCastNPList[i]);
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeRayCasts();
	}

	public override void _Process(float delta)
	{
		HandleCatchPrey();
	}

	public bool KillerDeviceFaulty
	{
		set
		{
			killerDeviceFaulty = value;
		}		
	}

	public bool ShouldWalk
	{
		get
		{
			return false;
		}		
	}

	public bool CannotMove
	{
		get
		{
			return !active || dead;
		}
	}


	[Export]
	public NodePath monsterCharacterNP;

	[Export]
	public NodePath bodyNP;

	[Export]
	public Array<NodePath> rayCastNPList;
	
	[Export]
	public bool active;

	[Export]
	public bool dead;


	private bool killerDeviceFaulty;

	private bool attacking;
	private Spatial monsterCharacter;
	private Spatial body;
	public RayCast[] rayCasts;
}
