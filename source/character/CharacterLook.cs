using Godot;


public class CharacterLook : Node
{
	public void ExecuteLook(float headMoveX, float headMoveY)
	{
		if(headMoveX != 0f || headMoveY != 0f)
		{
			float headRotation = head.RotationDegrees.x +
					(Mathf.Deg2Rad(-headMoveY) * lookSpeed);
			float bodyRotation = body.RotationDegrees.y +
					(Mathf.Deg2Rad(-headMoveX) * lookSpeed);

			if(headRotation > lookUpMaxAngle)
				headRotation = lookUpMaxAngle;
			else if(headRotation < lookDownMaxAngle)
				headRotation = lookDownMaxAngle;

			body.RotationDegrees = new Vector3(0f, bodyRotation, 0f);
			head.RotationDegrees = new Vector3(headRotation, 0f, 0f);
		}
	}

	private void Initialize()
	{
		body = GetNode<Spatial>(bodyNP);
		head = GetNode<Spatial>(headNP);
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public float LookSpeed
	{
		set
		{
			lookSpeed = value;
		}
	}

	public float LookUpMaxAngle
	{
		set
		{
			lookUpMaxAngle = value;
		}
	}

	public float LookDownMaxAngle
	{
		set
		{
			lookDownMaxAngle = value;
		}
	}


	[Export]
	public NodePath bodyNP;

	[Export]
	public NodePath headNP;


	private float lookSpeed;
	private float lookUpMaxAngle;
	private float lookDownMaxAngle;


	private Spatial body;
	private Spatial head;
}
