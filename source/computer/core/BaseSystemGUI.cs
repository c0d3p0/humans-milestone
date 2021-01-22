using Godot;
using Godot.Collections;


public abstract class BaseSystemGUI : Node
{
	public Button GetButtonByTouch(ulong areaInstanceId)
	{
		byte buttonId;

		if(touchAreaMap.TryGetValue(areaInstanceId, out buttonId))
			return GetButton(buttonId);

		return null;
	}

	public Button GetButton(byte buttonId)
	{
		Button button;

		if(buttonMap.TryGetValue(buttonId, out button))
			return button;

		return null;
	}

	public Label GetLabel(byte labelId)
	{
		Label label;
		
		if(labelMap.TryGetValue(labelId, out label))
			return label;

		return null;
	}

	public TextureRect GetTextureRect(byte textureRectId)
	{
		TextureRect tr;
		
		if(textureRectMap.TryGetValue(textureRectId, out tr))
			return tr;

		return null;
	}


	protected Dictionary<byte, Button> buttonMap;
	protected Dictionary<byte, Label> labelMap;
	protected Dictionary<byte, TextureRect> textureRectMap;
	protected Dictionary<ulong, byte> touchAreaMap;
}
