using Godot;


public class ExperimentManagerDebug : Node
{
	private void HandleIncreaseAmbientLightSkyContribution(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.KpDivide)
		{
			worldEnvironment.Environment.AmbientLightSkyContribution += 0.1f;
			GD.PushWarning("Debug, WorldEnvironment AmbientLight" +
					" SkyContribution increased by 0.1!");
		}
	}

	private void HandleDecreaseAmbientLightSkyContribution(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.KpMultiply)
		{
			worldEnvironment.Environment.AmbientLightSkyContribution -= 0.1f;
			GD.PushWarning("Debug, WorldEnvironment AmbientLight" +
					" SkyContribution decreased by 0.1!");
		}
	}

	private void HandleDefaultAmbientLightSkyContribution(uint keyScancode)
	{
		if(keyScancode == (uint) KeyList.KpSubtract)
		{
			worldEnvironment.Environment.AmbientLightSkyContribution = 0.99f;
			GD.PushWarning("Debug, WorldEnvironment AmbientLight" +
					" SkyContribution set to its default (0.99) value!");
		}
	}

	private void HandleDebug(InputEventKey inputEventKey)
	{
		if(inputEventKey != null && inputEventKey.Pressed)
		{
			uint keyScancode = inputEventKey.Scancode;
			HandleIncreaseAmbientLightSkyContribution(keyScancode);
			HandleDecreaseAmbientLightSkyContribution(keyScancode);
			HandleDefaultAmbientLightSkyContribution(keyScancode);
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
