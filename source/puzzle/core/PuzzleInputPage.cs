using Godot;
using Godot.Collections;


public class PuzzleInputPage : Godot.Object
{
	public PuzzleInputPage()
	{
		puzzleInputMap = new Dictionary<int, PuzzleContent>();
	}

	public void AddPuzzleInput(byte key, char c)
	{
		puzzleInputMap.Add(key, new PuzzleContent(c, c.ToString()));
	}

	public void AddPuzzleInput(byte key, char c, Texture image)
	{
		puzzleInputMap.Add(key, new PuzzleContent(c, c.ToString(), image));
	}

	public PuzzleContent GetPuzzleInput(byte key)
	{
		if(puzzleInputMap.ContainsKey(key))
			return puzzleInputMap[key];

		return null;
	}


	public Dictionary<int, PuzzleContent> puzzleInputMap;
}
