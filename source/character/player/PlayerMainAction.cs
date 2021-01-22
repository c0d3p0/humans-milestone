using Godot;


public class PlayerMainAction : Node
{
	public void UpdateActive(Vector3 position)
	{
		playerCharacter.Translation = position;
		UpdateActive();
	}

	public void UpdateActive()
	{
		dead = false;
		EmitSignal(SignalKey.ON_CHARACTER_ACTIVE);
	}

	public void Panic(Spatial panicCauser)
	{
		if(!dead && !invincible)
		{
			SetActive(false);
			Vector3 bodyLookAt = panicCauser.GlobalTransform.origin;
			bodyLookAt.y = body.GlobalTransform.origin.y;
			body.LookAt(bodyLookAt, Vector3.Up);
			Vector3 headLookAt = bodyLookAt;
			headLookAt.y = head.GlobalTransform.origin.y;
			head.LookAt(headLookAt, Vector3.Up);
			EmitSignal(SignalKey.ON_PANICKED);
		}
	}

	public void Die()
	{
		if(!invincible)
		{
			dead = true;
			SetActive(false);
			EmitSignal(SignalKey.ON_CHARACTER_DEATH);
		}
	}

	public void SetActive(bool active)
	{
		this.active = active;
		EmitSignal(SignalKey.SET_CHARACTER_MOVE_ENABLED, active);
	}

	private void OnPlayerDead() // Called by death animation
	{
		playerCharacter.EmitSignal(SignalKey.SHOW_DEAD_SCREEN);
	}

	private void Initialize()
	{
		playerCharacter = GetNode<Spatial>(playerCharacterNP);
		body = GetNode<Spatial>(bodyNP);
		head = GetNode<Spatial>(headNP);
		invincible = false;
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public bool CannotMove
	{
		get
		{
			return !active || dead;
		}
	}

	public bool Invincible
	{
		get
		{
			return invincible;
		}
		set
		{
			invincible = value;
		}
	}


	[Export]
	public NodePath playerCharacterNP;
	
	[Export]
	public NodePath headNP;

	[Export]
	public NodePath bodyNP;

	[Export]
	public bool active;

	[Export]
	public bool dead;


	private Spatial playerCharacter;
	private Spatial body;
	private Spatial head;
	private bool invincible;
}
