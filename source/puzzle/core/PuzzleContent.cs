using Godot;


public class PuzzleContent : Godot.Object
{
	public PuzzleContent(char character, string text, Texture texture)
	{
		this.character = character;
		this.text = text;
		this.texture = texture;
	}

	public PuzzleContent(char character, string text)
	{
		this.character = character;
		this.text = text;
	}

	public PuzzleContent(char character, Texture texture)
	{
		this.character = character;
		this.texture = texture;
	}

	public PuzzleContent(string text, Texture texture)
	{
		this.text = text;
		this.texture = texture;
	}

	public PuzzleContent(string text)
	{
		this.text = text;
	}


	public char character;
	public string text;
	public Texture texture;
}
