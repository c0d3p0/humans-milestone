using System.Text;

using Godot;


public class TheLawOfTheRectangleV1Puzzle : EnhancedBasePuzzle
{
	protected PuzzleContent CreateHintPage(byte passwordChar)
	{
		StringBuilder page = CreateRectangleContent(passwordChar);
		page = operation == FORWARD ? CreateForwardRectangle(page) :
				CreateBackwardRectangle(page);
		return new PuzzleContent(page.ToString().Trim());
	}

	protected StringBuilder CreateRectangleContent(byte passwordChar)
	{
		StringBuilder sb = GetDecimalCharacters();
		sb.Remove(passwordChar, 1);		
		sb = Shuffle(sb.ToString());
		sb.Insert(passwordChar, passwordChar);
		byte swap;
		char c;

		for(int i = 0; i < 10; i++)
		{
			if((sb[i] - '0' == i) && i != passwordChar)
			{
				do
				{
					swap = (byte) this.RandiRange(rng, 0, 9);
				}
				while(swap == passwordChar || swap == i);

				c = sb[i];
				sb[i] = sb[swap];
				sb[swap] = c;
			}
		}

		return sb;
	}

	protected StringBuilder CreateForwardRectangle(StringBuilder characters)
	{
		StringBuilder sb = new StringBuilder();

		for(int i = 0; i < 4; i++)
			sb.Append(" ").Append(characters[i]);

		sb.Append('\n').Append(characters[9]).Append("     ");
		sb.Append(characters[4]).Append('\n');

		for(int i = 8; i > 4; i--)
			sb.Append(characters[i]).Append(' ');

		return sb;
	}

	private StringBuilder CreateBackwardRectangle(StringBuilder characters)
	{
		StringBuilder sb = new StringBuilder();

		for(int i = 10; i > 6; i--)
			sb.Append(' ').Append(characters[i % 10]);

		sb.Append('\n').Append(characters[1]).Append("     ");
		sb.Append(characters[6]).Append('\n');

		for(int i = 2; i < 6; i++)
			sb.Append(characters[i]).Append(' ');

		return sb;
	}

	protected override void InitializePuzzle()
	{
		byte index = 0;
		byte[] answer = RandomizeNumbers(8, 0, 9);
		operation = (byte) this.RandiRange(rng, 0, 1);
		CopyNumericArrayToCorrectAnswer(answer);
		puzzleContentMap.Add(index++,
				new PuzzleContent("The Law Of The Rectangle v1"));

		for(byte i = 0; i < answer.Length; i++)
			puzzleContentMap.Add(index++, CreateHintPage(answer[i]));

		puzzleContentMap.Add(index++, CreateExtraPage1());
		puzzleContentMap.Add(index++, CreateExtraPage2());
		puzzleStatusPageId = index;

		if(OS.IsDebugBuild())
			PrintPuzzleDebug();

		operation = 255;
	}

	private PuzzleContent CreateExtraPage1()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("Lawyers and people nowadays interpret the law in ");
		sb.Append("several ways, twisting it to benefit themselves.");
		return new PuzzleContent(sb.ToString());
	}

	private PuzzleContent CreateExtraPage2()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("But everyone knows the law has only one ");
		sb.Append("interpretation, the one that brings justice.");
		return new PuzzleContent(sb.ToString());
	}

	protected void PrintPuzzleDebug()
	{
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
		GD.PushWarning("Puzzle: The Law of the Rectangle V1");
		GD.PushWarning("Operation: " + operation);
		GD.PushWarning("CorrectAnswer: " + new string(correctAnswer));
		GD.PushWarning("");
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
	}


	protected byte operation;

	protected const byte FORWARD = 0;
	protected const byte BACKWARD = 1;
}


/*
	Puzzle Idea


	1: 4 numbers from 0 to 9 will be randomly generated to represent the 4
	   digits password.

	2: Each of the 4 numbers will result in one text page. Each page will
	   help to find one digit of the password.

	3: The text page will contain the numbers from 0 to 9 in a rectangle
	   shape, like the example below but with numbers in a random order.
	   0123
	   9  4
	   8765

	4: The hint to find the password digit in the rectangle is to look at
	   the number that is in the correct position. All the numbers in the
		 example above is in their correct spot, but the page will be created
		 with all numbers in a wrong spot, except the one that represents the
		 password digit.
*/
