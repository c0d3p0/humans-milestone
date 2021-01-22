using SCG = System.Collections.Generic;

using Godot;
using Godot.Collections;


public class PuzzleSystemGUI : BaseSystemGUI
{
	public void UpdateGUIState(bool active)
	{
		SCG.IEnumerator<SCG.KeyValuePair<byte, Button>> it = buttonMap.GetEnumerator();
			
		while(it.MoveNext())
			it.Current.Value.Disabled = !active;
	
		answerPanel.Visible = active;
		puzzleContentPanel.Visible = active;
		touchKeyboardPanel.Visible = active;
		inactivePanel.Visible = !active;
	}

  private void InitializeMaps()
  {
		buttonMap = new Dictionary<byte, Button>();
		labelMap = new Dictionary<byte, Label>();
		textureRectMap =  new Dictionary<byte, TextureRect>();
    touchAreaMap = new Dictionary<ulong, byte>();
		inactivePanel = GetNode<PanelContainer>(inactivePanelNP);
  }

	private void InitializeAnswerPanel()
	{
		answerPanel = GetNode<PanelContainer>(answerPanelNP);
		Node pwdc = answerPanel.GetChild(0);
    Node child;
		
		for(byte i = SystemGUIID.LBL_PWD_C0;
        i <= SystemGUIID.LBL_PWD_C11; i++)
		{
			child = pwdc.GetChild(i - SystemGUIID.LBL_PWD_C0);
			labelMap.Add(i, child.GetChild<Label>(0));
			textureRectMap.Add(i, child.GetChild<TextureRect>(1));
		}
	}

	private void InitializePuzzleContentPanel()
	{
    puzzleContentPanel = GetNode<PanelContainer>(puzzleContentPanelNP);
		Node cc = puzzleContentPanel.GetChild(0);
		Node pp = cc.GetChild(0);
		labelMap.Add(SystemGUIID.LBL_PAGE_CONTENT, pp.GetChild<Label>(0));
		textureRectMap.Add(SystemGUIID.LBL_PAGE_CONTENT, pp.GetChild<TextureRect>(1));
		buttonMap.Add(SystemGUIID.BTN_PREV_PAGE, cc.GetChild<Button>(1));
		buttonMap.Add(SystemGUIID.BTN_NEXT_PAGE, cc.GetChild<Button>(2));
	}

	private void InitializeTouchKeyboardPanel()
	{
		touchKeyboardPanel = GetNode<PanelContainer>(touchKeyboardPanelNP);
		Node tck = touchKeyboardPanel.GetChild(0);
		Node child;
		
		for(byte i = SystemGUIID.BTN_KB_KEY0;
        i <= SystemGUIID.BTN_KB_KEY_ALT; i++)
		{
			child = tck.GetChild(i - SystemGUIID.BTN_KB_KEY0);
			textureRectMap.Add(i, child.GetChild<TextureRect>(0));
			buttonMap.Add(i, child.GetChild<Button>(1));
		}
	}

  private void InitializeTouchArea()
  {
    Node ta = GetNode(touchAreaNP);
    Node pa = ta.GetChild(0);
    Node kba = ta.GetChild(1);
    touchAreaMap.Add(pa.GetChild(0).GetInstanceId(),
        SystemGUIID.BTN_PREV_PAGE);
    touchAreaMap.Add(pa.GetChild(1).GetInstanceId(),
        SystemGUIID.BTN_NEXT_PAGE);
    Node child;

    for(byte i = SystemGUIID.BTN_KB_KEY0;
        i <= SystemGUIID.BTN_KB_KEY_ALT; i++)
    {
      child = kba.GetChild(i - SystemGUIID.BTN_KB_KEY0);
      touchAreaMap.Add(child.GetInstanceId(), i);
    }
  }

  public override void _EnterTree()
  {
    InitializeMaps();
    InitializeAnswerPanel();
    InitializePuzzleContentPanel();
    InitializeTouchKeyboardPanel();
    InitializeTouchArea();
  }


	[Export]
	public NodePath answerPanelNP;

	[Export]
	public NodePath puzzleContentPanelNP;

	[Export]
	public NodePath touchKeyboardPanelNP;

	[Export]
	public NodePath inactivePanelNP;

  [Export]
	public NodePath touchAreaNP;


	private PanelContainer answerPanel;
	private PanelContainer puzzleContentPanel;
	private PanelContainer touchKeyboardPanel;
	private PanelContainer inactivePanel;
}
