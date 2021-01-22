using Godot;


public class PlayerInputInterpreter : Node
{
	private void ComputeInputDirection(ref float axis, byte axisIndex)
	{
		bool axisN = Input.IsActionPressed(directionKeys[axisIndex, 0]);
		bool axisP = Input.IsActionPressed(directionKeys[axisIndex, 1]);

		if(axisP && !axisN)
			axis = 1;
		else if(axisN && !axisP)
			axis = -1;
	}

	private void HandleDirectionInput()
	{
		direction.x = direction.z = 0;

		if(!this.EmitSignal<bool>(this, SignalKey.CANNOT_MOVE))
		{
			ComputeInputDirection(ref direction.z, 0);
			ComputeInputDirection(ref direction.x, 1);
		}	
	}

	private void HandleLook(InputEventMouseMotion mouseMotion)
	{
		if(!this.EmitSignal<bool>(this, SignalKey.CANNOT_MOVE) &&
				mouseMotion != null)
		{
			Vector2 mmi = mouseMotion.Relative;
			EmitSignal(SignalKey.EXECUTE_LOOK, mmi.x, mmi.y);
		}
	}

	private void Initialize()
	{
		Input.SetMouseMode(Input.MouseMode.Captured);
		directionKeys = new string[,]{{PlayerInput.P1_UP, PlayerInput.P1_DOWN},
				{PlayerInput.P1_LEFT, PlayerInput.P1_RIGHT}};
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public override void _PhysicsProcess(float delta)
	{
		HandleDirectionInput();
	}

	public override void _Input(InputEvent inputEvent)
	{
		HandleLook(inputEvent as InputEventMouseMotion);
	}

	public Vector3 Direction
	{
		get
		{
			return direction;
		}
	}

	public bool ShouldWalk
	{
		get
		{
			return !this.EmitSignal<bool>(this, SignalKey.CANNOT_MOVE) &&
					Input.IsActionPressed(PlayerInput.P1_WALK);
		}
	}

	public bool ShouldInteract
	{
		get
		{
			return !this.EmitSignal<bool>(this, SignalKey.CANNOT_MOVE) &&
					Input.IsActionJustPressed(PlayerInput.P1_INTERACTION);
		}
	}


	private Vector3 direction;

	private string[,] directionKeys;
}
