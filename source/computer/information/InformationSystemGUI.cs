using Godot;
using Godot.Collections;


public class InformationSystemGUI : BaseSystemGUI
{
	private void InitializeButtonMap()
	{
		Node nbc = GetNode(navigationButtonControlNP);
		buttonMap = new Dictionary<byte, Button>();
		buttonMap.Add(SystemGUIID.BTN_PREV_PAGE, nbc.GetChild<Button>(0));
		buttonMap.Add(SystemGUIID.BTN_NEXT_PAGE, nbc.GetChild<Button>(1));
	}

	private void InitializeLabelMap()
	{
		labelMap = new Dictionary<byte, Label>();
		labelMap.Add(SystemGUIID.LBL_ROOM_TITLE,
				GetNode<Label>(roomNameLabelNP));
		labelMap.Add(SystemGUIID.LBL_DOORS_STATUS,
				GetNode<Label>(doorsLabelNP));
		labelMap.Add(SystemGUIID.LBL_PUZZLES_STATUS,
				GetNode<Label>(puzzlesLabelNP));
	}

	private void InitializeTouchAreaMap()
	{
		Node ta = GetNode(touchAreaNP);
		touchAreaMap = new Dictionary<ulong, byte>();
		touchAreaMap.Add(ta.GetChild(0).GetInstanceId(),
				SystemGUIID.BTN_PREV_PAGE);
		touchAreaMap.Add(ta.GetChild(1).GetInstanceId(),
				SystemGUIID.BTN_NEXT_PAGE);
	}

	public override void _EnterTree()
	{
		InitializeButtonMap();
		InitializeLabelMap();
		InitializeTouchAreaMap();
	}


	[Export]
	public NodePath roomNameLabelNP;

	[Export]
	public NodePath doorsLabelNP;

	[Export]
	public NodePath puzzlesLabelNP;

	[Export]
	public NodePath navigationButtonControlNP;

	[Export]
	public NodePath touchAreaNP;
}
