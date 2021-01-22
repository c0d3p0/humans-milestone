using Godot;


public class LoadScreen : Node
{
	private void HandleChangeScene()
	{
		if(loadedScene != null)
		{
			taskRunner.Call("SetActive", false);
			taskRunner.Call("Clear");
			GetTree().ChangeSceneTo(loadedScene);
		}
	}

	private void LoadScene()
	{
		loadedScene = ResourceLoader.Load(scenePath) as PackedScene;
	}

	private void RequestScene()
	{
		taskRunner = GetNode(taskRunnerNodePath);
		taskRunner.Call("SetActive", false);
		taskRunner.Call("Clear");
		taskRunner.Call("Put", this, "LoadScene");
		taskRunner.Call("SetActive", true);
	}

	private void ObtainScenePath()
	{
		if(!splashScreen)
		{
			Node globalValue = GetNode(globalValueNodePath);
			scenePath = this.EmitSignal<string>(globalValue, SignalKey.GET, "sceneToLoad");
		}
		else
			scenePath = mainScenePath;
	}

	public override void _EnterTree()
	{
		ObtainScenePath();
		RequestScene();
	}

	public override void _Ready()
	{
		SetProcess(changeSceneAfterLoad);
	}

	public override void _Process(float delta)
	{
		HandleChangeScene();
	}


	[Export]
	public string taskRunnerNodePath = "/root/TaskRunner";

	[Export]
	public string globalValueNodePath = "/root/GlobalValue";

	[Export]
	public string mainScenePath = "res://scene/title_screen.tscn";

	[Export]
	public bool changeSceneAfterLoad = true;

	[Export]
	public bool splashScreen;


	private Node taskRunner;
	private string scenePath;
	private PackedScene loadedScene;
}


// {
// 	private void UpdateProgress()
// 	{
// 		if(riLoader != null)
// 		{	
// 			Error e = riLoader.Poll();

// 			if(e == Error.FileEof)
// 			{
// 				PackedScene ps = riLoader.GetResource() as PackedScene;
// 				riLoader.Dispose();
// 				riLoader = null;
// 				GetTree().ChangeSceneTo(ps);
// 			}
// 			else if(e == Error.Ok)
// 			{
// 				int progress = (riLoader.GetStage() * 100) / riLoader.GetStageCount();
// 				progressBar.Value = progress;
// 			}
// 		}
// 	}

// 	private void Initialize()
// 	{
// 		progressBar = GetNode<ProgressBar>(progressBarNP);
// 		Node globalValue = GetTree().Root.GetNode("GlobalValue");
// 		string scenePath = this.EmitSignal<string>(globalValue,
// 				SignalKey.GET, "sceneToLoad");

// 		if(scenePath != null)
// 			riLoader = ResourceLoader.LoadInteractive(scenePath);
// 	}

// 	public override void _EnterTree()
// 	{
// 		Initialize();
// 	}

// 	public override void _Process(float delta)
// 	{
// 		UpdateProgress();
// 	}


// 	[Export]
// 	public NodePath progressBarNP;

// 	private ProgressBar progressBar;
// 	private ResourceInteractiveLoader riLoader;
// }
