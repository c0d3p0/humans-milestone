using SCG = System.Collections.Generic;

using Godot;
using Godot.Collections;


public class PlayerInteraction : Node
{
	private void Interact()
	{
		if(this.EmitSignal<bool>(this, SignalKey.SHOULD_INTERACT))
		{
			Vector3 from = head.GlobalTransform.origin;
			Vector3 to = interactionReach.GlobalTransform.origin;
			PhysicsDirectSpaceState pds = head.GetWorld().DirectSpaceState;
			Godot.Collections.Dictionary result = pds.IntersectRay(
					from, to, interactExclusionList, interactionLayer, true, true);
			string key = "collider";

			if(result.Contains(key))
			{
				Node collider = result[key] as Node;
				collider.EmitSignal(SignalKey.ON_INTERACTION_RECEIVED, collider);
			}

			EmitSignal(SignalKey.ON_INTERACTING);
		}
	}

	private void Initialize()
	{
		head = GetNode<Spatial>(headNP);
		interactionReach = GetNode<Spatial>(interactionReachNP);
	}

	private void InitializeInteractExclusionList()
	{
		interactExclusionList = new Godot.Collections.Array();
		SCG.IEnumerator<NodePath> it = interactExclusionNPList.GetEnumerator();

		while(it.MoveNext())
			interactExclusionList.Add(GetNode<Spatial>(it.Current));
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeInteractExclusionList();
	}

	public override void _Process(float deltaTime)
	{
		Interact();
	}



	[Export]
	public NodePath headNP;

	[Export]
	public NodePath interactionReachNP;

	[Export]
	public Array<NodePath> interactExclusionNPList;

	[Export]
	public uint interactionLayer;


	private Spatial head;
	private Spatial interactionReach;
	private Godot.Collections.Array interactExclusionList;
}
