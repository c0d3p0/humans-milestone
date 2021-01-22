using SCG = System.Collections.Generic;

using Godot;
using Godot.Collections;


public class MonsterAI : Node
{
	public void ClearTargetAndAllPaths()
	{
		ClearPathToSearchPoint();
		ClearPathToTarget();
		target = null;
	}

	public void CreatePathToSearchPoint(Vector3 position)
	{
		ClearPathToSearchPoint();
		pathToSearchPoint = navigation.GetSimplePath(
				monsterCharacter.GlobalTransform.origin, position);
	}

	public void OnLookAroundFinished()
	{
		UpdatePathToTarget();
		UpdatePathToSearchPoint();
		searchAroundTimer.Start();
	}
 
	private void HandleSight()
	{
		if(preTarget != null && IsInSightFieldOfView(preTarget))
		{
			Vector3 preyPosition = preTarget.GlobalTransform.origin;
			preyPosition.y = sightPosition.GlobalTransform.origin.y;
			Spatial collider = DoRayCastAtPrey(preyPosition);
			
			if(collider == null)
			{
				preyPosition.y = -0.75f;	// Height close to the feet.
				collider = DoRayCastAtPrey(preyPosition);

				if(collider == null)
				{
					target = null;
					return;
				}
			}

			target = preTarget;
			ignoreNoiseTimer.Stop();
		}
		else
			target = null;
	}	

	private void FollowBestPath()
	{
		if(pathToTarget != null && pathToTargetIndex < pathToTarget.Length)
		{
			FollowPath(ref pathToTarget, ref pathToTargetIndex);
			UpdatePathToTarget();
		}
		else
			FollowPath(ref pathToSearchPoint, ref pathToSearchPointIndex);
	}

	private void FollowPath(ref Vector3[] path, ref int pathIndex)
	{
		lookAroundTimer.Stop();

		while(pathIndex < path.Length)
		{
			Vector3 currentDistance = monsterCharacter.GlobalTransform.origin.
					DirectionTo(path[pathIndex]).Abs();

			if(currentDistance.x > 0.5f || currentDistance.z > 0.5f)
			{
				EmitSignal(SignalKey.LOOK_AT, path[pathIndex]);
				direction = Vector3.Forward;
				break;
			}	
			else
				pathIndex++;
		}

		if(pathIndex >= path.Length)
			ClearPath(ref path, ref pathIndex);
	}

	private void DecideWhatToDo()
	{
		direction = Vector3.Zero;

		if(CanMove())
		{
			if(pathToTarget != null || target != null)
			{
				UpdatePathToTarget();
				FollowBestPath();
			}	
			else if(pathToSearchPoint != null || !searchAroundTimer.IsStopped())
			{
				UpdatePathToSearchPoint();
				FollowBestPath();
			}
			else if(lookAroundTimer.IsStopped())
				lookAroundTimer.Start();
		}
	}

	private void HandlePossibleDeadPath()
	{
		Node collider = rayCastCenter.GetCollider() as Node;

		if(collider != null)
		{
			bool doorLocked = IsDoorAndLocked(collider);

			if(doorLocked || collider.IsInGroup("monster"))
			{
				target = null;
				ClearPathToTarget();
				ClearPathToSearchPoint();
			}
			
			if(doorLocked)
				ignoreNoiseTimer.Start();
		}
	}

	private void HandleDropPathIfStuck()
	{
		Vector3 cpr = monsterCharacter.GlobalTransform.origin.Round();

		if(cpr.IsEqualApprox(lastRoundedPosition) && lookAroundTimer.IsStopped())
		{
			lastPositionRoundedFrameAmount++;

			if(lastPositionRoundedFrameAmount > lastPositionRoundedMaximumFrames)
			{
				ClearPathToSearchPoint();
				ClearPathToTarget();
				lastPositionRoundedFrameAmount = 0;
			}
		}
		else
		{
			lastPositionRoundedFrameAmount = 0;
			lastRoundedPosition = cpr;
		}
	}

	private void UpdatePathToTarget()
	{
		if(target != null)
		{
			ClearPathToSearchPoint();

			if(pathToTarget == null ||
					pathToTargetIndex >= pathToTarget.Length - 2)
			{
				ClearPathToTarget();
				pathToTarget = navigation.GetSimplePath(monsterCharacter.
						GlobalTransform.origin, target.GlobalTransform.origin);
			}
		}
	}

	private void UpdatePathToSearchPoint()
	{
		if(target == null && pathToTarget == null &&
				pathToSearchPoint == null && searchTargetList.Count > 0)
		{
			Spatial st = this.GetRandomItem<Spatial>(searchTargetList, rng);
			CreatePathToSearchPoint(st.GlobalTransform.origin);
		}
	}

	private Spatial DoRayCastAtPrey(Vector3 preyPosition)
	{
		Vector3 sight = sightPosition.GlobalTransform.origin;
		PhysicsDirectSpaceState pds = body.GetWorld().DirectSpaceState;
		Godot.Collections.Dictionary result = pds.IntersectRay(sight,
				preyPosition, sightExclusionList, 2147483647, true, false);
		string key = "collider";

		if(result.Contains(key))
		{
			Spatial collider = result[key] as Spatial;

			if(collider.IsInGroup("monster_prey"))
				return collider;
		}

		return null;
	}

	private bool IsInSightFieldOfView(Spatial spatial)
	{
		Vector3 mPos = monsterCharacter.GlobalTransform.origin;
		Vector3 pPos = spatial.GlobalTransform.origin;
		Vector3 mFacing = Vector3.Forward.Rotated(Vector3.Up, body.Rotation.y);
		mPos.y = 0f;
		pPos.y = 0f;
		Vector3 pmDistance = mPos.DirectionTo(pPos);
		float angle = Mathf.Rad2Deg(Mathf.Acos(mFacing.Dot(pmDistance)));
		return angle < (sightFieldOfView / 2f);
	}

	public void HandlePreSight(Area area)
	{
		if(area.IsInGroup("monster_prey"))
			preTarget = area;
	}

	private void HandleOutOfSight(Area area)
	{
		if(area.IsInGroup("monster_prey"))
		{
			target = null;
			preTarget = null;
		}
	}

	private void HandleHearing(Area area)
	{
		if(ShouldHear() && target == null && area.IsInGroup("noise"))
			CreatePathToSearchPoint(area.GlobalTransform.origin);
	}

	private bool CanMove()
	{
		return !this.EmitSignal<bool>(this, SignalKey.CANNOT_MOVE);
	}

	private bool ShouldHear()
	{
		return CanMove() && ignoreNoiseTimer.IsStopped();
	}

	private bool IsDoorAndLocked(Node collider)
	{
		return collider.IsInGroup("door") && !this.EmitSignal<bool>(
				collider.GetParent(), SignalKey.IS_UNLOCKED);
	}

	private void ClearPathToTarget()
	{
		ClearPath(ref pathToTarget, ref pathToTargetIndex);
	}

	private void ClearPathToSearchPoint()
	{
		ClearPath(ref pathToSearchPoint, ref pathToSearchPointIndex);
	}

	private void ClearPath(ref Vector3[] path, ref int pathIndex)
	{
		path = null;
		pathIndex = 0;
	}

	private void Initialize()
	{
		monsterCharacter = GetNode<Spatial>(monsterCharacterNP);
		body = GetNode<Spatial>(bodyNP);
		lookAroundTimer = GetNode<Timer>(lookAroundTimerNP);
		searchAroundTimer = GetNode<Timer>(searchAroundTimerNP);
		ignoreNoiseTimer = GetNode<Timer>(ignoreNoiseTimerNP);
		rayCastCenter = GetNode<RayCast>(rayCastCenterNP);
		sightPosition =  GetNode<Spatial>(sightPositionNP);
		rng = new RandomNumberGenerator();
	}

	private void InitializeSightExclusionList()
	{
		sightExclusionList =  new Godot.Collections.Array();
		SCG.IEnumerator<NodePath> it = sightExclusionNPList.GetEnumerator();

		while(it.MoveNext())
			sightExclusionList.Add(GetNode<Spatial>(it.Current));
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeSightExclusionList();
	}

	public override void _Ready()
	{
		SetProcess(false);
	}

	public override void _Process(float delta)
	{
		HandlePossibleDeadPath();
		HandleDropPathIfStuck();
		DecideWhatToDo();
	}

	public override void _PhysicsProcess(float delta)
	{
		HandleSight();
	}

	public void OnSightAreaEntered(Area area)
	{
		HandlePreSight(area);
	}

	public void OnSightAreaExited(Area area)
	{
		HandleOutOfSight(area);
	}

	public void OnMonsterAreaEntered(Area area)
	{
		HandleHearing(area);
	}

	public Vector3 Direction
	{
		get
		{
			return direction;
		}		
	}

	public Array<Spatial> SearchTargetList
	{
		set
		{
			searchTargetList = value;
		}		
	}

	public Navigation Navigation
	{
		set
		{
			navigation = value;
		}		
	}



	[Export]
	public NodePath monsterCharacterNP;

	[Export]
	public NodePath bodyNP;

	[Export]
	public NodePath lookAroundTimerNP;

	[Export]
	public NodePath searchAroundTimerNP;

	[Export]
	public NodePath ignoreNoiseTimerNP;

	[Export]
	public NodePath rayCastCenterNP;

	[Export]
	public NodePath sightPositionNP;

	[Export]
	public Array<NodePath> sightExclusionNPList;
	
	[Export]
	public float sightFieldOfView = 100f;

	[Export]
	private int lastPositionRoundedMaximumFrames = 300;



	private Spatial monsterCharacter;
	private Spatial body;
	private Timer lookAroundTimer;
	private Timer ignoreNoiseTimer;
	private Timer searchAroundTimer;
	private RayCast rayCastCenter;
	private Spatial sightPosition;
	public Godot.Collections.Array sightExclusionList;

	private Vector3 direction;
	private Array<Spatial> searchTargetList;
	private Navigation navigation;

	private Spatial target;
	private Spatial preTarget;
	private Vector3[] pathToTarget;
	private Vector3[] pathToSearchPoint;
	private Vector3 lastRoundedPosition;
	private int pathToTargetIndex;
	private int pathToSearchPointIndex;
	private int lastPositionRoundedFrameAmount;
	private RandomNumberGenerator rng;
}
