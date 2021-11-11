using System.Text;

using Godot;
using Godot.Collections;


public abstract class BasePuzzle : Godot.Object, Puzzle
{
	public bool GoToNextPuzzleContentPage()
	{
		if(currentPuzzleContentPage < puzzleContentMap.Count - 1)
		{	
			currentPuzzleContentPage++;
			return true;
		}
		
		return false;
	}

	public bool GoToPreviousPuzzleContentPage()
	{
		if(currentPuzzleContentPage > 0)
		{
			currentPuzzleContentPage--;
			return true;
		}

		return false;
	}

	public bool GoToNextPuzzleInputPage()
	{
		if(currentPuzzleInputPage < puzzleInputPageMap.Count - 1)
			currentPuzzleInputPage++;
		else
			currentPuzzleInputPage = 0;
		
		return puzzleInputPageMap.Count > 1;
	}

	public PuzzleContent GetCurrentPuzzleContentPage()
	{
		return puzzleContentMap[currentPuzzleContentPage];
	} 

	public PuzzleInputPage GetCurrentPuzzleInputPage()
	{
		return puzzleInputPageMap[currentPuzzleInputPage];
	}

	public PuzzleContent GetPuzzleInput(byte key)
	{
		return puzzleInputPageMap[currentPuzzleInputPage].GetPuzzleInput(key);
	}

	public sbyte AddUserInput(PuzzleContent puzzleInput)
	{
		if(userInputMap.Count < 12)
		{
			userInputMap.Add(puzzleInput);
			return (sbyte) (userInputMap.Count - 1);
		}

		return -1;
	}

	public sbyte RemoveLastUserInput()
	{
		if(userInputMap.Count > 0)
		{
			userInputMap.RemoveAt(userInputMap.Count - 1);
			return (sbyte) userInputMap.Count;
		}
		
		return -1;
	}

	public void ClearAnswer()
	{
		userInputMap.Clear();
	}

	public virtual void TrySolvePuzzle()
	{		
		if(solved = IsUserAnswerCorrect())
		{
			StringBuilder sb = new StringBuilder("Solved\nThe password is ");
			sb.Append(correctAnswer);
			puzzleContentMap[puzzleStatusPageId].text = sb.ToString();
		}
	}

	protected bool IsUserAnswerCorrect()
	{
		ProcessUserAnswer();

		if(correctAnswer.Length == userAnswer.Length)
		{
			for(int i = 0; i < correctAnswer.Length; i++)
			{
				if(userAnswer[i] != correctAnswer[i])
					return false;
			}

			return true;
		}

		return false;
	}

	protected virtual void ProcessUserAnswer()
	{
		userAnswer = new char[userInputMap.Count];

		for(int i = 0; i < userInputMap.Count; i++)
			userAnswer[i] = userInputMap[i].character;

		if(OS.IsDebugBuild())
			PrintCorrectAnswer();
	}

	protected virtual void CreatePuzzleStatusPage()
	{
		puzzleContentMap.Add(puzzleStatusPageId, new PuzzleContent("Unsolved"));
	}

	public bool Solved
	{
		get
		{
			return solved;
		}
	}

	public int ContentPageAmount
	{
		get
		{
			return puzzleContentMap.Count;
		}
	}

	public int InputPageAmount
	{
		get
		{
			return puzzleInputPageMap.Count;
		}
	}
	
	private void InitializeAttributes()
	{
		currentPuzzleContentPage = 0;
		currentPuzzleInputPage = 0;
		puzzleContentMap = new Dictionary<int, PuzzleContent>();
		puzzleInputPageMap = new Dictionary<int, PuzzleInputPage>();
		userInputMap = new Array<PuzzleContent>();
		rng = new RandomNumberGenerator();
	}

	public BasePuzzle()
	{
		InitializeAttributes();
		InitializePuzzle();
		InitializePuzzleInputsMap();
		CreatePuzzleStatusPage();
	}

	protected void PrintCorrectAnswer()
	{
		GD.PushWarning(puzzleContentMap[0].text);

		if(correctAnswer != null)
		{
			StringBuilder sb = new StringBuilder();

			for(int i = 0; i < correctAnswer.Length; i++)
				sb.Append(correctAnswer[i]);

			Godot.GD.PushWarning("CorrectAnswer: " + sb.ToString());
		}
		else
			Godot.GD.PushWarning("CorrectAnswer: null");

		Godot.GD.PushWarning(" ");
	}


	protected abstract void InitializePuzzle();
	protected abstract void InitializePuzzleInputsMap();


	protected bool solved;
	protected byte puzzleStatusPageId;

	protected char[] correctAnswer;
	protected char[] userAnswer;
	protected Array<PuzzleContent> userInputMap;

	protected Dictionary<int, PuzzleContent> puzzleContentMap;
	protected Dictionary<int, PuzzleInputPage> puzzleInputPageMap;

	protected RandomNumberGenerator rng;

	private byte currentPuzzleContentPage;
	private byte currentPuzzleInputPage;
}
