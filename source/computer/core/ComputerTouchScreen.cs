using Godot;


public class ComputerTouchScreen : Node
{
	public void OnInteractionReceived(Area area)
	{
		if(touchTimer.IsStopped())
		{
			Button b = this.EmitSignal<Button>(this,
					SignalKey.GET_BUTTON_BY_TOUCH, area.GetInstanceId());
					
			if(b != null && !b.Disabled && b.Visible)
			{
				GenerateTouchScreenClick(b.RectGlobalPosition, true);
				screenViewport.SetInputAsHandled();
				EmitSignal(SignalKey.ON_BUTTON_TOUCHED);
			}
		}
	}

	private void GenerateTouchScreenClick(Vector2 position, bool pressed)
	{
		touchEvent.ButtonIndex = 1;
		touchEvent.Position = position;
		touchEvent.Pressed = pressed;
		screenViewport.Input(touchEvent);

		if(pressed)
			touchTimer.Start();
	}

	public void OnReleaseTouch()
	{
		GenerateTouchScreenClick(Vector2.Zero, false);
	}

	private void Initialize()
	{
		touchTimer = GetNode<Timer>(touchTimerNP);
		screenViewport = GetNode<Viewport>(screenViewportNP);
		touchEvent = new InputEventMouseButton();
	}

	public override void _EnterTree()
	{
		Initialize();
	}


	[Export]
	public NodePath screenViewportNP;

	[Export]
	public NodePath touchTimerNP;


	private Timer touchTimer;
	private Viewport screenViewport;
	private InputEventMouseButton touchEvent;
}
