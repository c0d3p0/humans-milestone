using Godot;


public class ExperimentManagerDebug : Node
{
	private void HandleActivateDefaultLightMode(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.KpDivide)
		{
			DirectionalLight dl = worldEnvironment.GetChild<DirectionalLight>(0);
			dl.LightColor = new Color(16f / 255f, 24f / 255f, 32f / 255f);
			GD.PushWarning("Debug, DirectionalLight Color " +
					"set to its default (16, 24, 32) value!");
		}
	}

	private void HandleToggleNiceNightLightMode(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.KpMultiply)
		{
			DirectionalLight dl = worldEnvironment.GetChild<DirectionalLight>(0);
			dl.LightColor = new Color(32f / 255f, 48f / 255f, 64f / 255f);
			GD.PushWarning("Debug, DirectionalLight Color " +
					"set to (32, 48, 64) value!");
		}
	}

	private void HandleToggleDayLightMode(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.KpSubtract)
		{
			DirectionalLight dl = worldEnvironment.GetChild<DirectionalLight>(0);
			dl.LightColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);
			GD.PushWarning("Debug, DirectionalLight Color " +
					"set to (255, 255, 255) value!");
		}
	}


	private void HandleDebug(InputEventKey inputEventKey)
	{
		if(inputEventKey != null && inputEventKey.Pressed)
		{
			uint keyScancode = inputEventKey.Scancode;
			HandleActivateDefaultLightMode(keyScancode);
			HandleToggleNiceNightLightMode(keyScancode);
			HandleToggleDayLightMode(keyScancode);
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		HandleDebug(inputEvent as InputEventKey);
	}

	public override void _Ready()
	{
		SetProcessInput(OS.IsDebugBuild());
	}

	public WorldEnvironment WorldEnvironment
	{
		set
		{
			worldEnvironment = value;
		}
	}


	private WorldEnvironment worldEnvironment;
}


// Old light was 18, 22, 36.