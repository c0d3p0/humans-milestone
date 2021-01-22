using Godot;


public class CharacterMove : Node
{
	private void HandleMovement(float delta)
	{
		shouldWalk = this.EmitSignal<bool>(this, SignalKey.SHOULD_WALK);
		direction = this.EmitSignal<Vector3>(this, SignalKey.GET_DIRECTION);
		moveSpeed = GetMoveSpeed();
		look = direction.Rotated(Vector3.Up, body.Rotation.y);
		velocity.y = 0;
		velocityLimit.y = 0f;

		if(direction.x != 0f && direction.z != 0f)
		{
			velocityLimit.x = look.x * moveSpeed * diagonalMoveFactor;
			velocityLimit.z = look.z * moveSpeed * diagonalMoveFactor;
		}
		else
		{
			velocityLimit.x = look.x * moveSpeed;
			velocityLimit.z = look.z * moveSpeed;
		}

		velocity = velocity.LinearInterpolate(velocityLimit, delta * GetAcceleration());
		velocity = kinematicBody.MoveAndSlide(velocity, Vector3.Up, false, 4, 0, false);

		if(direction.Length() > 0)
			EmitSignal(shouldWalk ? SignalKey.ON_WALKING : SignalKey.ON_RUNNING);
		else
			EmitSignal(SignalKey.ON_IDLING);
	}

	private void HandlePushObjects()
	{
		int collisionAmount = kinematicBody.GetSlideCount();
		KinematicCollision kc;
		RigidBody rb;
		float pushForce = velocity.Length() * pushStrength * 0.1f;

		for(int i = 0; i < collisionAmount; i++)
		{
			kc = kinematicBody.GetSlideCollision(i);
			rb = kc.Collider as RigidBody;

			if(rb != null)
				rb.ApplyCentralImpulse(-kc.Normal * pushForce);
		}		
	}

	private void FixPositionY()
	{
		if(kinematicBody.Translation.y != groundSurfacePositionY)
		{
			float dist = Mathf.Abs(kinematicBody.Translation.y - groundSurfacePositionY);
			float dir = kinematicBody.Translation.y < groundSurfacePositionY ? 1 : -1;
			kinematicBody.MoveAndCollide(new Vector3(0f, dist * dir, 0f));
		}
	}

	public float GetMoveSpeed()
	{
		if(shouldWalk)
			return walkSpeed;
		else
			return runSpeed;
	}

	private float GetAcceleration()
	{
		if(direction.Dot(velocityLimit) > 0)
			return acceleration;
		else
			return deacceleration;
	}

	private void Initialize()
	{
		kinematicBody = GetNode<KinematicBody>(kinematicBodyNP);
		body = GetNode<Spatial>(bodyNP);
		kinematicBody.MoveLockY = true;
	}

	public override void _PhysicsProcess(float physicsDelta)
	{
		if(enabled)
		{	
			HandleMovement(physicsDelta);
			HandlePushObjects();
		}

		FixPositionY();
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public float RunSpeed
	{
		set
		{
			runSpeed = value;
		}
	}

	public float WalkSpeed
	{
		set
		{
			walkSpeed = value;
		}
	}

	public float Acceleration
	{
		set
		{
			acceleration = value;
		}
	}

	public float Deacceleration
	{
		set
		{
			deacceleration = value;
		}
	}

	public float GroundSurfacePositionY
	{
		set
		{
			groundSurfacePositionY = value;
		}
	}


	[Export]
	public NodePath kinematicBodyNP;

	[Export]
	public NodePath bodyNP;

	[Export]
	public bool enabled = true;

	[Export]
	public float pushStrength = 50f;


	private KinematicBody kinematicBody;
	private Spatial body;
	private Vector3 velocity;
	private Vector3 velocityLimit;
	private Vector3 direction;
	private Vector3 look;
	private float moveSpeed;
	private bool shouldWalk;

	private float diagonalMoveFactor = 0.7071f;

	private float runSpeed;
	private float walkSpeed;
	private float acceleration;
	private float deacceleration;
	private float groundSurfacePositionY;	
}
