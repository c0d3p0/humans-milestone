using SCG = System.Collections.Generic;

using Godot;
using Godot.Collections;


public class ExperimentResultSystemGUI : BaseSystemGUI
{
	public void UpdateGUIState(bool active)
	{
		SCG.IEnumerator<SCG.KeyValuePair<byte, Button>> it = buttonMap.GetEnumerator();
			
		while(it.MoveNext())
			it.Current.Value.Disabled = !active;
	
		titlePanel.Visible = active;
		resultDataPanel.Visible = active;
		awardPanel.Visible = active;
		inactivePanel.Visible = !active;	
	}

	private void InitializeButtonMap()
	{
		buttonMap = new Dictionary<byte, Button>();
		buttonMap.Add(SystemGUIID.BTN_KB_KEY_OK,
				awardPanel.GetChild(0).GetChild(1).GetChild<Button>(1));
	}

	private void InitializeLabelMap()
	{
		labelMap = new Dictionary<byte, Label>();
		labelMap.Add(SystemGUIID.LBL_EXPERIMENT_RESULT_DATA,
				resultDataPanel.GetChild(0).GetChild<Label>(1));
		labelMap.Add(SystemGUIID.LBL_AWARD_INFO,
				awardPanel.GetChild(0).GetChild(1).GetChild<Label>(0));
	}

	private void InitializeTouchAreaMap()
	{
		Node ta = GetNode(touchAreaNP);
		touchAreaMap = new Dictionary<ulong, byte>();
		touchAreaMap.Add(ta.GetInstanceId(), SystemGUIID.BTN_KB_KEY_OK);
	}

	private void Initialize()
	{
		Node rootPanel = GetNode(rootPanelNP);
		titlePanel = rootPanel.GetChild<Control>(0);
		resultDataPanel = rootPanel.GetChild<Control>(1);
		awardPanel = rootPanel.GetChild<Control>(2);
		inactivePanel = rootPanel.GetChild<Control>(3);
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeButtonMap();
		InitializeLabelMap();
		InitializeTouchAreaMap();
	}



	[Export]
	public NodePath rootPanelNP;

	[Export]
	public NodePath touchAreaNP;


	private Control titlePanel;
	private Control resultDataPanel;
	private Control awardPanel;
	private Control inactivePanel;
}
