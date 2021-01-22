using Godot;


public class PuzzleComputer : Node
{
	public void SetSystemActive(bool active)
	{
		puzzleSystem.Active = active;
	}

	public void SetPuzzle(Godot.Object puzzle)
	{
		puzzleSystem.Puzzle = puzzle as Puzzle;
	}

	public void IsPuzzleSolved(Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET, puzzleSystem.Solved);
	}

	public void GetButton(byte buttonId, Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET, puzzleSystemGUI.GetButton(buttonId));
	}

	public void GetButtonByTouch(ulong areaInstanceId, Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET,
				puzzleSystemGUI.GetButtonByTouch(areaInstanceId));
	}

	public void GetLabel(byte labelId, Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET, puzzleSystemGUI.GetLabel(labelId));
	}

	public void GetTextureRect(byte textureRectId, Godot.Object signalData)
	{
		signalData.EmitSignal(SignalKey.SET,
				puzzleSystemGUI.GetTextureRect(textureRectId));
	}

	private void Initialize()
	{
		puzzleSystemGUI = GetNode<PuzzleSystemGUI>(puzzleSystemGUINP);
		puzzleSystem = GetNode<PuzzleSystem>(puzzleSystemNP);
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public override void _Ready()
	{
		EmitSignal(SignalKey.ADD_PUZZLE_COMPUTER, this, roomId, computerId);
	}

	
	[Export]
	public NodePath puzzleSystemGUINP;

	[Export]
	public NodePath puzzleSystemNP;

	[Export]
	public byte roomId;

	[Export]
	public byte computerId;

	private PuzzleSystemGUI puzzleSystemGUI;
	private PuzzleSystem puzzleSystem;
}
