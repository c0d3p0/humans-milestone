using Godot;


public class TitleScreen : Node
{
	private void CreateNewMainComputer()
	{
		MainComputer newMainComputer = mainComputerPrefabPS.Instance() as MainComputer;
		sceneTreeRoot.CallDeferred("add_child", newMainComputer);
		newMainComputer.Connect("ready", this, "InitializeGame",
				null, (uint) ConnectFlags.Oneshot);
	}

	private void RemoveMainComputer()
	{
		Node mainComputer = sceneTreeRoot.GetNodeOrNull("MainComputer");

		if(mainComputer != null)
		{
			sceneTreeRoot.CallDeferred("remove_child", mainComputer);
			mainComputer.CallDeferred("queue_free");
		}
	}

	private void InitializeGame()
	{		
		globalValue.EmitSignal(SignalKey.PUT, "sceneToLoad", experimentFacilityPath);
		sceneTree.ChangeScene(loadScreenPath);
	}

	private void ShowCreditsScreen()
	{
		sceneTree.ChangeScene(creditsScreenPath);
	}

	private void Initialize()
	{
		Input.SetMouseMode(Input.MouseMode.Visible);
		sceneTree = GetTree();
		sceneTreeRoot = sceneTree.Root;
		globalValue = sceneTreeRoot.GetNode("GlobalValue");
	}

	public override void _EnterTree()
	{
		Initialize();
		RemoveMainComputer();
	}

	public void OnStartGameButtonPressed()
	{
		CreateNewMainComputer();
	}

	public void OnCreditsButtonPressed()
	{
		ShowCreditsScreen();
	}

	public void OnQuitGameButtonPressed()
	{
		GetTree().Quit();
	}


	[Export]
	public PackedScene mainComputerPrefabPS;

	[Export]
	public string experimentFacilityPath = "res://scene/experiment_facility.tscn";

	[Export]
	public string loadScreenPath = "res://scene/load_screen.tscn";

	[Export]
	public string creditsScreenPath = "res://scene/credits_screen.tscn";


	private SceneTree sceneTree;
	private Node sceneTreeRoot;
	private Node globalValue;
}
